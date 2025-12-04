namespace escuchify_api.Modelos;

public class Disco
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public int AnioLanzamiento { get; set; }
    public string TipoDisco { get; set; }

    public int ArtistaId { get; set; } 
    public Artista? Artista { get; set; } // Navegación al Artista

    // Canciones vinculadas
    public List<Cancion> Canciones { get; set; } = new List<Cancion>();

    public Disco(int id, string titulo, int anioLanzamiento, string tipoDisco, int artistaId)
    {
        Id = id;
        Titulo = titulo;
        AnioLanzamiento = anioLanzamiento;
        TipoDisco = tipoDisco;
        ArtistaId = artistaId;
    }

    public override string ToString()
    {
        return $"Nombre: {Titulo}, Año de Lanzamiento: {AnioLanzamiento}, Tipo de Disco: {TipoDisco}";
    }

}


