using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using escuchify_api.Modelos;
using escuchify_api.DTOs; // Importar DTOs

namespace escuchify_api.Controllers;

[Route("[controller]")]
[ApiController]
public class DiscosController : ControllerBase
{
    private readonly AppDbContext _context;

    public DiscosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Disco>>> GetDiscos()
    {
        return await _context.Discos.Include(d => d.Artista).ToListAsync();
    }

    // GET: /discos/5 (Ahora devuelve el DTO con la duración calculada)
    [HttpGet("{id}")]
    public async Task<ActionResult<DiscoDetalleDto>> GetDisco(int id)
    {
        var disco = await _context.Discos
            .Include(d => d.Artista)
            .Include(d => d.Canciones)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (disco == null) return NotFound();

        // --- CÁLCULO DE DURACIÓN ---
        int segundosTotales = 0;
        foreach (var cancion in disco.Canciones)
        {
            try {
                var partes = cancion.Duracion.Split(':');
                if (partes.Length == 2)
                {
                    int min = int.Parse(partes[0]);
                    int seg = int.Parse(partes[1]);
                    segundosTotales += (min * 60) + seg;
                }
            } catch { /* Ignoramos formatos inválidos */ }
        }

        TimeSpan tiempo = TimeSpan.FromSeconds(segundosTotales);
        string duracionFormateada = tiempo.ToString(@"hh\:mm\:ss"); 

        // --- MAPEO ---
        var discoDto = new DiscoDetalleDto
        {
            Id = disco.Id,
            Titulo = disco.Titulo,
            AnioLanzamiento = disco.AnioLanzamiento,
            TipoDisco = disco.TipoDisco,
            NombreArtista = disco.Artista?.Nombre ?? "Desconocido",
            Canciones = disco.Canciones,
            DuracionTotal = duracionFormateada 
        };

        return Ok(discoDto);
    }

    [HttpPost]
    public async Task<ActionResult<Disco>> PostDisco(Disco disco)
    {
        // VALIDACIÓN 1: Artista existente
        if (disco.ArtistaId > 0 && await _context.Artistas.FindAsync(disco.ArtistaId) == null)
        {
            return BadRequest("El ArtistaId especificado no existe.");
        }

        // VALIDACIÓN 2: Año futuro
        if (disco.AnioLanzamiento > DateTime.Now.Year)
        {
            return BadRequest($"El año de lanzamiento no puede ser mayor a {DateTime.Now.Year}.");
        }

        // VALIDACIÓN 3: Duplicado (Mismo nombre para el mismo artista)
        var existeDisco = await _context.Discos
            .AnyAsync(d => d.ArtistaId == disco.ArtistaId && d.Titulo.ToLower() == disco.Titulo.ToLower());

        if (existeDisco)
        {
            return BadRequest($"Este artista ya tiene un disco llamado '{disco.Titulo}'.");
        }

        _context.Discos.Add(disco);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetDisco", new { id = disco.Id }, disco);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutDisco(int id, Disco disco)
    {
        if (id != disco.Id && disco.Id != 0) return BadRequest();

        var discoExistente = await _context.Discos.FindAsync(id);
        if (discoExistente == null) return NotFound();

        // Validar año al editar
        if (disco.AnioLanzamiento > DateTime.Now.Year)
        {
            return BadRequest($"El año de lanzamiento no puede ser mayor a {DateTime.Now.Year}.");
        }

        discoExistente.Titulo = disco.Titulo;
        discoExistente.AnioLanzamiento = disco.AnioLanzamiento;
        discoExistente.TipoDisco = disco.TipoDisco;

        if (disco.ArtistaId > 0 && await _context.Artistas.FindAsync(disco.ArtistaId) != null)
        {
            discoExistente.ArtistaId = disco.ArtistaId;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDisco(int id)
    {
        var disco = await _context.Discos.FindAsync(id);
        if (disco == null) return NotFound();

        _context.Discos.Remove(disco);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}