using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CollinEventplanner.Models;

namespace CollinEventplanner.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId);


            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Participant)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.ParticipantId);


            base.OnModelCreating(modelBuilder);


            // Dummy data om tickets te testen, migratie -> AddDummy 
            // Zonder accounts staat in de index de waarde op 1 --> value="1"
            modelBuilder.Entity<Participant>().HasData(
                new Participant
                {
                    ParticipantId = 1,
                    Name = "Test",
                    Email = "test@test.com"
                 }
             );
        }
    }
}