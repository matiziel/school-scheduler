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
            //context.Database.EnsureDeleted();

            context.Teachers.Add(new Teacher("kowalski", ""));
            context.Teachers.Add(new Teacher("nowak", ""));
            context.Teachers.Add(new Teacher("mazurek", ""));
            context.Teachers.Add(new Teacher("clarkson", ""));
            context.Teachers.Add(new Teacher("may", ""));
            context.Teachers.Add(new Teacher("hammond", ""));

            for (int i = 0; i < 45; i++) {
                context.Slots.Add(new Slot(i, ""));
            }
            context.Rooms.Add(new Room("111", ""));
            context.Rooms.Add(new Room("112", ""));
            context.Rooms.Add(new Room("113", ""));
            context.Rooms.Add(new Room("114", ""));
            context.Rooms.Add(new Room("115", ""));
            context.Rooms.Add(new Room("116", ""));
            context.Rooms.Add(new Room("117", ""));

            context.Subjects.Add(new Subject("eng", ""));
            context.Subjects.Add(new Subject("phys", ""));
            context.Subjects.Add(new Subject("mat", ""));
            context.Subjects.Add(new Subject("esp", ""));



            context.ClassGroups.Add(new ClassGroup("1a", ""));
            context.ClassGroups.Add(new ClassGroup("2a", ""));
            context.ClassGroups.Add(new ClassGroup("3a", ""));
            context.ClassGroups.Add(new ClassGroup("4a", ""));
            context.ClassGroups.Add(new ClassGroup("5a", ""));
            context.ClassGroups.Add(new ClassGroup("6a", ""));
            context.ClassGroups.Add(new ClassGroup("7a", ""));



            context.SaveChanges();

            var activityFirst = new Activity();
            activityFirst.Room = context.Rooms.FirstOrDefault(r => r.Name == "111");
            activityFirst.Slot = context.Slots.FirstOrDefault(s => s.Index == 0);
            activityFirst.Teacher = context.Teachers.FirstOrDefault(t => t.Name == "kowalski");
            activityFirst.Subject = context.Subjects.FirstOrDefault(s => s.Name == "mat");
            activityFirst.ClassGroup = context.ClassGroups.FirstOrDefault(c => c.Name == "1a");
            context.Activities.Add(activityFirst);

            var activitySecond = new Activity();
            activitySecond.Room = context.Rooms.FirstOrDefault(r => r.Name == "113");
            activitySecond.Slot = context.Slots.FirstOrDefault(s => s.Index == 0);
            activitySecond.Teacher = context.Teachers.FirstOrDefault(t => t.Name == "mazurek");
            activitySecond.Subject = context.Subjects.FirstOrDefault(s => s.Name == "eng");
            activitySecond.ClassGroup = context.ClassGroups.FirstOrDefault(c => c.Name == "2a");
            context.Activities.Add(activitySecond);

            var activityThird = new Activity();
            activityThird.Room = context.Rooms.FirstOrDefault(r => r.Name == "112");
            activityThird.Slot = context.Slots.FirstOrDefault(s => s.Index == 1);
            activityThird.Teacher = context.Teachers.FirstOrDefault(t => t.Name == "nowak");
            activityThird.Subject = context.Subjects.FirstOrDefault(s => s.Name == "phys");
            activityThird.ClassGroup = context.ClassGroups.FirstOrDefault(c => c.Name == "2a");
            context.Activities.Add(activityThird);

            context.SaveChanges();
            return context;
        }
    }

}