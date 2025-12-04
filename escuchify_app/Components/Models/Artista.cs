namespace escuchify_app.Components.Models;

// NOTA: Es importante que las propiedades coincidan con el backend.
public class Artista
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Nacionalidad { get; set; }
    public string? Biografia { get; set; }
    public string? GeneroPrincipal { get; set; }
    
    // Lista de Discos asociados (puede ser null o vacía)
    public List<Disco>? Discos { get; set; }
}