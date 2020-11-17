using Microsoft.EntityFrameworkCore;
using Model;

namespace Persistence {
    public class ApplicationDbContext : DbContext, IApplicationDbContext {
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

            modelBuilder.Entity<Room>().HasKey(r => r.Id);

            modelBuilder.Entity<Slot>().HasKey(s => s.Id);

            modelBuilder.Entity<Subject>().HasKey(s => s.Id);

            modelBuilder.Entity<Teacher>().HasKey(r => r.Id);
        }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ClassGroup> ClassGroups { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
    }
}