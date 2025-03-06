using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SportSphere.Shared.Models;

namespace SportSphere.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<SportModel> Sports { get; set; }
        public DbSet<VenueModel> Venues { get; set; }
        public DbSet<EventModel> Events { get; set; }
        public DbSet<LocationModel> Locations { get; set; }
        public DbSet<UserFavoriteSportModel> UserFavoriteSports { get; set; }
        public DbSet<EventParticipantModel> EventParticipants { get; set; }
        public DbSet<VenueSportModel> VenueSports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure table names to match SQL schema
            modelBuilder.Entity<UserModel>().ToTable("Users");
            modelBuilder.Entity<SportModel>().ToTable("Sports");
            modelBuilder.Entity<VenueModel>().ToTable("Venues");
            modelBuilder.Entity<EventModel>().ToTable("Events");
            modelBuilder.Entity<LocationModel>().ToTable("Locations");
            modelBuilder.Entity<UserFavoriteSportModel>().ToTable("UserFavoriteSports");
            modelBuilder.Entity<EventParticipantModel>().ToTable("EventParticipants");
            modelBuilder.Entity<VenueSportModel>().ToTable("VenueSports");

            // Explicitly ignore navigation properties
            modelBuilder.Entity<UserModel>()
                .Ignore(u => u.FavoriteSports)
                .Ignore(u => u.CreatedEvents)
                .Ignore(u => u.ParticipatingEvents);

            modelBuilder.Entity<SportModel>()
                .Ignore(s => s.Practitioners)
                .Ignore(s => s.Venues)
                .Ignore(s => s.Events);

            modelBuilder.Entity<VenueModel>()
                .Ignore(v => v.SupportedSports)
                .Ignore(v => v.OfferedSports)
                .Ignore(v => v.HostedEvents);

            modelBuilder.Entity<EventModel>()
                .Ignore(e => e.Participants);

            // Configure decimal properties with precision and scale
            modelBuilder.Entity<VenueModel>()
                .Property(v => v.Rating)
                .HasPrecision(3, 2);

            modelBuilder.Entity<VenueModel>()
                .Property(v => v.PricePerHour)
                .HasPrecision(10, 2);

            modelBuilder.Entity<EventModel>()
                .Property(e => e.Price)
                .HasPrecision(10, 2);

            // Configure composite keys for many-to-many relationships
            modelBuilder.Entity<UserFavoriteSportModel>()
                .HasKey(ufs => new { ufs.UserId, ufs.SportId });

            modelBuilder.Entity<EventParticipantModel>()
                .HasKey(ep => new { ep.EventId, ep.UserId });

            modelBuilder.Entity<VenueSportModel>()
                .HasKey(vs => new { vs.VenueId, vs.SportId });

            // Configure one-to-many relationship between User and Event (CreatedEvents)
            modelBuilder.Entity<EventModel>()
                .HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between Sport and Event
            modelBuilder.Entity<EventModel>()
                .HasOne(e => e.Sport)
                .WithMany()
                .HasForeignKey(e => e.SportId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between Venue and Event
            modelBuilder.Entity<EventModel>()
                .HasOne(e => e.Venue)
                .WithMany()
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-one relationship between User and Location
            modelBuilder.Entity<UserModel>()
                .HasOne(u => u.DefaultLocation)
                .WithOne()
                .HasForeignKey<UserModel>(u => u.DefaultLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-one relationship between Venue and Location
            modelBuilder.Entity<VenueModel>()
                .HasOne(v => v.Location)
                .WithOne()
                .HasForeignKey<VenueModel>(v => v.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-one relationship between Event and Location
            modelBuilder.Entity<EventModel>()
                .HasOne(e => e.Location)
                .WithOne()
                .HasForeignKey<EventModel>(e => e.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public static IModel CreateModel()
        {
            var builder = new ModelBuilder();
            
            // Configure the model
            new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()).OnModelCreating(builder);
            
            return builder.FinalizeModel();
        }
    }
} 