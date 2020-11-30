using Microsoft.EntityFrameworkCore;
using Persistence;
using Model;
using System.Linq;

namespace UnitTests {
    public class PrepareData {
        public static ApplicationDbContext GetDbContext() {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            ApplicationDbContext context = new ApplicationDbContext(options);

            context.Teachers.Add(new Teacher("kowalski", ""));
            context.Teachers.Add(new Teacher("nowak", ""));
            context.Teachers.Add(new Teacher("mazurek", ""));

            for (int i = 0; i < 5; i++) {
                context.Slots.Add(new Slot(i, ""));
            }
            context.Rooms.Add(new Room("111", ""));
            context.Rooms.Add(new Room("112", ""));
            context.Rooms.Add(new Room("113", ""));

            context.Subjects.Add(new Subject("eng", ""));
            context.Subjects.Add(new Subject("phys", ""));
            context.Subjects.Add(new Subject("mat", ""));

            context.ClassGroups.Add(new ClassGroup("1a", ""));
            context.ClassGroups.Add(new ClassGroup("2a", ""));
            context.ClassGroups.Add(new ClassGroup("3a", ""));

            context.SaveChanges();

            var activityFirst = new Activity();
            activityFirst.Room = context.Rooms.FirstOrDefault(r => r.Name == "111");
            activityFirst.Slot = context.Slots.FirstOrDefault(s => s.Index == 0);
            activityFirst.Teacher = context.Teachers.FirstOrDefault(t => t.Name == "kowalski");
            activityFirst.Subject = context.Subjects.FirstOrDefault(s => s.Name == "mat");
            activityFirst.ClassGroup = context.ClassGroups.FirstOrDefault(c => c.Name == "1a");
            context.Activities.Add(activityFirst);

            var activitySecond = new Activity();
            activitySecond.Room = context.Rooms.FirstOrDefault(r => r.Name == "112");
            activitySecond.Slot = context.Slots.FirstOrDefault(s => s.Index == 1);
            activitySecond.Teacher = context.Teachers.FirstOrDefault(t => t.Name == "nowak");
            activitySecond.Subject = context.Subjects.FirstOrDefault(s => s.Name == "phys");
            activitySecond.ClassGroup = context.ClassGroups.FirstOrDefault(c => c.Name == "2a");
            context.Activities.Add(activitySecond);

            context.SaveChanges();
            return context;
        }
    }

}