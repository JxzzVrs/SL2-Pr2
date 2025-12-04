using Microsoft.EntityFrameworkCore;
using escuchify_api.Modelos;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cancion> Canciones { get; set; }
    public DbSet<Disco> Discos { get; set; }
    public DbSet<Artista> Artistas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Llamar al método base

        // Relación 1 a N (Artista -> Discos)
        modelBuilder.Entity<Disco>()
            .HasOne(d => d.Artista) // Un disco tiene un artista
            .WithMany(a => a.Discos) // Un artista tiene muchos discos
            .HasForeignKey(d => d.ArtistaId); // La clave foránea es ArtistaId

        // Relación 1 a N (Disco -> Canciones)
        modelBuilder.Entity<Disco>()
            .HasMany(d => d.Canciones)
            .WithOne(c => c.Disco)
            .HasForeignKey(c => c.DiscoId);
    }

}