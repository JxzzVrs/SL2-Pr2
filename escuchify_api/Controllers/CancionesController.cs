using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using escuchify_api.Modelos;

namespace escuchify_api.Controllers;

[Route("[controller]")]
[ApiController]
public class CancionesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CancionesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cancion>>> GetCanciones()
    {
        return await _context.Canciones
            .Include(c => c.Disco)
            .ThenInclude(d => d!.Artista)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cancion>> GetCancion(int id)
    {
        var cancion = await _context.Canciones
            .Include(c => c.Disco)
            .ThenInclude(d => d!.Artista)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cancion == null) return NotFound();

        return cancion;
    }

    [HttpPost]
    public async Task<ActionResult<Cancion>> PostCancion(Cancion cancion)
    {
        // VALIDACIÓN 1: El disco debe existir
        if (cancion.DiscoId > 0 && await _context.Discos.FindAsync(cancion.DiscoId) == null)
        {
            return BadRequest("El DiscoId especificado no existe.");
        }

        // VALIDACIÓN 2: Formato de duración (MM:SS)
        if (!cancion.Duracion.Contains(":") || cancion.Duracion.Split(':').Length != 2)
        {
            return BadRequest("La duración debe tener el formato 'MM:SS' (ej: 3:45).");
        }

        // VALIDACIÓN 3: No duplicar canción en el mismo disco
        var existeCancion = await _context.Canciones
            .AnyAsync(c => c.DiscoId == cancion.DiscoId && c.Titulo.ToLower() == cancion.Titulo.ToLower());

        if (existeCancion)
        {
            return BadRequest($"El disco ya contiene una canción llamada '{cancion.Titulo}'.");
        }

        _context.Canciones.Add(cancion);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCancion", new { id = cancion.Id }, cancion);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCancion(int id, Cancion cancion)
    {
        if (id != cancion.Id && cancion.Id != 0) return BadRequest();

        var cancionExistente = await _context.Canciones.FindAsync(id);
        if (cancionExistente == null) return NotFound();

        // Validar duración al editar
        if (!cancion.Duracion.Contains(":") || cancion.Duracion.Split(':').Length != 2)
        {
            return BadRequest("La duración debe tener el formato 'MM:SS'.");
        }

        cancionExistente.Titulo = cancion.Titulo;
        cancionExistente.Duracion = cancion.Duracion;
        cancionExistente.Genero = cancion.Genero;

        if (cancion.DiscoId > 0 && await _context.Discos.FindAsync(cancion.DiscoId) != null)
        {
            cancionExistente.DiscoId = cancion.DiscoId;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCancion(int id)
    {
        var cancion = await _context.Canciones.FindAsync(id);
        if (cancion == null) return NotFound();

        _context.Canciones.Remove(cancion);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}