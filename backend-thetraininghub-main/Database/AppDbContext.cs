using System.Collections.Generic;
using AA2_CS.Model;
using Microsoft.EntityFrameworkCore;
namespace AA2_CS.Database;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<UserDTO> UserDTOs { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<UserRoom> UserRooms { get; set; }
    public DbSet<Model.Task> Tasks { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Mapeo de tablas a minúsculas
        modelBuilder.Entity<User>().ToTable("users");

        modelBuilder.Entity<Plan>().ToTable("plans");
        modelBuilder.Entity<Item>().ToTable("items");
        modelBuilder.Entity<Purchase>().ToTable("purchases");
        modelBuilder.Entity<Room>().ToTable("rooms");
        modelBuilder.Entity<UserRoom>().ToTable("usersrooms");
        modelBuilder.Entity<Model.Task>().ToTable("tasks");

        // Configuración de clave compuesta para la tabla intermedia UserRoom
        modelBuilder.Entity<UserRoom>()
            .HasKey(ur => new { ur.userid, ur.roomid });

        modelBuilder.Entity<UserRoom>()
            .Property(ur => ur.userid).HasColumnName("userid");
        modelBuilder.Entity<UserRoom>()
            .Property(ur => ur.roomid).HasColumnName("roomid");

        // Configurar relaciones sin usar propiedades de navegación
        modelBuilder.Entity<UserRoom>()
            .HasOne(ur => ur.User)  // Relación con User
            .WithMany()  // No necesidad de la propiedad de navegación en User
            .HasForeignKey(ur => ur.userid)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRoom>()
            .HasOne(ur => ur.Room)  // Relación con Room
            .WithMany()  // No necesidad de la propiedad de navegación en Room
            .HasForeignKey(ur => ur.roomid)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}