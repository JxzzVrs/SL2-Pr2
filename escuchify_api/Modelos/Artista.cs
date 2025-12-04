namespace escuchify_api.Modelos;

public class Artista
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty; // Nombre del Artista
    public string? Nacionalidad { get; set; } // Propiedad opcional
    public string? Biografia { get; set; } // Propiedad opcional
    public string? GeneroPrincipal { get; set; } // Género musical principal
    
    // Relación 1 a N con Disco
    public List<Disco>? Discos { get; set; } = new List<Disco>(); 
}