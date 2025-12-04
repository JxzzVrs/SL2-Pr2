using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using escuchify_api.Modelos;

namespace escuchify_api.Controllers;

[Route("[controller]")]
[ApiController]
public class ArtistasController : ControllerBase
{
    private readonly AppDbContext _context;

    public ArtistasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Artista>>> GetArtistas()
    {
        return await _context.Artistas.Include(a => a.Discos).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Artista>> GetArtista(int id)
    {
        var artista = await _context.Artistas
            .Include(a => a.Discos)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (artista == null) return NotFound();

        return artista;
    }

    [HttpPost]
    public async Task<ActionResult<Artista>> PostArtista(Artista artista)
    {
        // VALIDACIÓN: Nombre duplicado (Esta la mantenemos porque es útil y no usa Helpers)
        var existeNombre = await _context.Artistas
            .AnyAsync(a => a.Nombre.ToLower() == artista.Nombre.ToLower());

        if (existeNombre)
        {
            return BadRequest($"El artista '{artista.Nombre}' ya existe.");
        }

        // ELIMINADO: Validación de género con ListadoGeneros

        _context.Artistas.Add(artista);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetArtista", new { id = artista.Id }, artista);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutArtista(int id, Artista artista)
    {
        if (id != artista.Id && artista.Id != 0) return BadRequest();

        var artistaExistente = await _context.Artistas.FindAsync(id);
        if (artistaExistente == null) return NotFound();

        // ELIMINADO: Validación de género con ListadoGeneros

        artistaExistente.Nombre = artista.Nombre;
        artistaExistente.Nacionalidad = artista.Nacionalidad;
        artistaExistente.Biografia = artista.Biografia;
        artistaExistente.GeneroPrincipal = artista.GeneroPrincipal;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArtista(int id)
    {
        var artista = await _context.Artistas.FindAsync(id);
        if (artista == null) return NotFound();

        _context.Artistas.Remove(artista);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}