using Microsoft.EntityFrameworkCore;
using Model;

namespace Persistence {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Activity>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Activity>()
                .HasOne(a => a.ClassGroup)
                .WithMany(c => c.Activities)
                .IsRequired();

            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Room)
                .WithMany(r => r.Activities)
                .IsRequired();
            
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Slot)
                .WithMany(s => s.Activities)
                .IsRequired();
            
            modelBuilder.Entity<Activity>()
                 .HasOne(a => a.Subject)
                 .WithMany(s => s.Activities)
                 .IsRequired();

            modelBuilder.Entity<Activity>()
                 .HasOne(a => a.Teacher)
                 .WithMany(t => t.Activities)
                 .IsRequired();

            modelBuilder.Entity<ClassGroup>().HasKey(c => c.Id);
            modelBuilder.Entity<ClassGroup>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<ClassGroup>().HasIndex(c => c.Name).IsUnique();

            modelBuilder.Entity<Room>().HasKey(r => r.Id);
            modelBuilder.Entity<Room>().Property(r => r.Name).IsRequired();
            modelBuilder.Entity<Room>().HasIndex(r => r.Name).IsUnique();

            modelBuilder.Entity<Slot>().HasKey(s => s.Id);
            modelBuilder.Entity<Slot>().Property(r => r.Name).IsRequired();
            modelBuilder.Entity<Slot>().HasIndex(s => s.Name).IsUnique();

            modelBuilder.Entity<Subject>().HasKey(s => s.Id);
            modelBuilder.Entity<Subject>().Property(t => t.Name).IsRequired();
            modelBuilder.Entity<Subject>().HasIndex(s => s.Name).IsUnique();

            modelBuilder.Entity<Teacher>().HasKey(t => t.Id);
            modelBuilder.Entity<Teacher>().Property(t => t.Name).IsRequired();
            modelBuilder.Entity<Teacher>().HasIndex(t => t.Name).IsUnique();
        }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ClassGroup> ClassGroups { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
    }
}