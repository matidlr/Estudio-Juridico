// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Cada DbSet representa una tabla en MySQL
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Abogado> Abogados => Set<Abogado>();
    public DbSet<Caso> Casos => Set<Caso>();
    public DbSet<Actualizacion> Actualizaciones => Set<Actualizacion>();
    public DbSet<Archivo> Archivos => Set<Archivo>();
    public DbSet<Prueba> Pruebas => Set<Prueba>();
    public DbSet<Comentario> Comentarios => Set<Comentario>();
    public DbSet<PreferenciasNotificacion> Preferencias => Set<PreferenciasNotificacion>();
    public DbSet<Recordatorio> Recordatorios => Set<Recordatorio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Email único — no puede haber dos usuarios con el mismo email
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Un Usuario tiene UN Cliente (relación 1 a 1)
        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Cliente)
            .WithOne(c => c.Usuario)
            .HasForeignKey<Cliente>(c => c.UsuarioId);

        // Un Cliente tiene MUCHOS Casos
        modelBuilder.Entity<Caso>()
            .HasOne(c => c.Cliente)
            .WithMany(cl => cl.Casos)
            .HasForeignKey(c => c.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Un Cliente tiene UNA configuración de preferencias
        modelBuilder.Entity<Cliente>()
            .HasOne(c => c.Preferencias)
            .WithOne(p => p.Cliente)
            .HasForeignKey<PreferenciasNotificacion>(p => p.ClienteId);
    }
}