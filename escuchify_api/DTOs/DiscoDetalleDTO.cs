using escuchify_api.Modelos;

namespace escuchify_api.DTOs;

public class DiscoDetalleDto
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public int AnioLanzamiento { get; set; }
    public string TipoDisco { get; set; }
    public string NombreArtista { get; set; }
    public List<Cancion> Canciones { get; set; }
    
    // Propiedad calculada (no está en la DB)
    public string DuracionTotal { get; set; } 
}