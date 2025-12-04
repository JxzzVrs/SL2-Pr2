// escuchify_app/Components/Models/Cancion.cs

using System.ComponentModel.DataAnnotations;

namespace escuchify_app.Components.Models;

public class Cancion
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título de la canción es obligatorio.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La duración es obligatoria.")]
    public string Duracion { get; set; } = string.Empty;

    public string? Genero { get; set; }

    // Clave Foránea (FK) para Disco
    [Required(ErrorMessage = "Debe asignar la canción a un disco.")]
    public int DiscoId { get; set; }
    
    // Propiedad de navegación (incluye el objeto Disco completo)
    public Disco? Disco { get; set; } 
}