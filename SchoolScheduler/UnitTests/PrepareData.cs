using Microsoft.EntityFrameworkCore;
using Persistence;
using Model;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests {
    public class PrepareData {
        public static ApplicationDbContext GetDbContext() {
            var options = CreateNewContextOptions();
            var context = new ApplicationDbContext(options);


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

            var activityFirst = new Activity {
                Room = context.Rooms.FirstOrDefault(r => r.Name == "111"),
                Slot = context.Slots.FirstOrDefault(s => s.Index == 0),
                Teacher = context.Teachers.FirstOrDefault(t => t.Name == "kowalski"),
                Subject = context.Subjects.FirstOrDefault(s => s.Name == "mat"),
                ClassGroup = context.ClassGroups.FirstOrDefault(c => c.Name == "1a")
            };
            context.Activities.Add(activityFirst);

            var activitySecond = new Activity {
                Room = context.Rooms.FirstOrDefault(r => r.Name == "113"),
                Slot = context.Slots.FirstOrDefault(s => s.Index == 0),
                Teacher = context.Teachers.FirstOrDefault(t => t.Name == "mazurek"),
                Subject = context.Subjects.FirstOrDefault(s => s.Name == "eng"),
                ClassGroup = context.ClassGroups.FirstOrDefault(c => c.Name == "2a")
            };
            context.Activities.Add(activitySecond);

            var activityThird = new Activity {
                Room = context.Rooms.FirstOrDefault(r => r.Name == "112"),
                Slot = context.Slots.FirstOrDefault(s => s.Index == 1),
                Teacher = context.Teachers.FirstOrDefault(t => t.Name == "nowak"),
                Subject = context.Subjects.FirstOrDefault(s => s.Name == "phys"),
                ClassGroup = context.ClassGroups.FirstOrDefault(c => c.Name == "2a")
            };
            context.Activities.Add(activityThird);

            context.SaveChanges();
            return context;
        }

        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions() {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: "Test").UseInternalServiceProvider(serviceProvider);
            return builder.Options;
        }
    }
}