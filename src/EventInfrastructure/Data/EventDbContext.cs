using EventCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventInfrastructure.Data;

public class EventDbContext(DbContextOptions<EventDbContext> options) : DbContext(options)
{
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Registration> Registrations => Set<Registration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Konfigurera sånt som convetions inte hanterar automatiskt

        // Event-konfigurering
        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Location).HasMaxLength(300);
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.Property(r => r.ParticipantName).HasMaxLength(100);
            entity.Property(r => r.ParticipantEmail)
                .HasMaxLength(256)
                .HasAnnotation("Collation", "NOCASE"); // SQLite case-insensitive collation

            // Unikt index för att förhindra dubbla registreringar per event.
            // Detta finns i vår domänmodell också, men vi lägger till det här 
            // också för att säkerställa dataintegritet på databassidan.
            // Med NOCASE collation hanteras tex gurkan@email.se som samma som gUrKan@email.se
            entity.HasIndex(r => new { r.EventId, r.ParticipantEmail })
                .IsUnique();
        });
    }
}
