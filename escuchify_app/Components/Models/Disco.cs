using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace escuchify_app.Components.Models;

public class Disco
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El título es obligatorio")]
    public string Titulo { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El año de lanzamiento es obligatorio")]
    [Range(1900, 2025, ErrorMessage = "El año de lanzamiento debe estar entre 1900 y 2025")]
    public int AnioLanzamiento { get; set; }
    
    [Required(ErrorMessage = "El tipo de disco es obligatorio")]
    public string TipoDisco { get; set; } = string.Empty;

    // Clave Foránea (FK) para Artista
    [Required(ErrorMessage = "El disco debe tener un artista asociado.")]
    public int ArtistaId { get; set; } 
    
    // Propiedades de navegación
    public Artista? Artista { get; set; }
    public List<Cancion> Canciones { get; set; } = new List<Cancion>();
}


