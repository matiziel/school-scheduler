using Microsoft.EntityFrameworkCore;
using Model;

namespace Persistence {
    public interface IApplicationDbContext {

        DbSet<Activity> Activities { get; set; }
        DbSet<ClassGroup> ClassGroups { get; set; }
        DbSet<Room> Rooms { get; set; }
        DbSet<Slot> Slots { get; set; }
        DbSet<Subject> Subjects { get; set; }
        DbSet<Teacher> Teachers { get; set; }

    }
}