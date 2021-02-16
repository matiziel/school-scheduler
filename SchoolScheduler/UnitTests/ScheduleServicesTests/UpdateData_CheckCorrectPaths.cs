using Xunit;
using Application;
using Persistence;
using Contracts.DataTransferObjects.Activities;
using System.Linq;
using Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace UnitTests.ScheduleServicesTests {
    public class UpdateData_CheckCorrectPaths {
        private IQueryable<Activity> GetActivities(ApplicationDbContext context) {
            return context.Activities
                .Include(a => a.Slot)
                .Include(a => a.Teacher)
                .Include(a => a.Room)
                .Include(a => a.ClassGroup)
                .Include(a => a.Subject);
        }

        [Theory]
        [InlineData("kowalski", "111", "mat", "1a", 12)]
        [InlineData("nowak", "112", "phys", "2a", 13)]
        [InlineData("kowalski", "111", "eng", "1a", 13)]
        [InlineData("nowak", "112", "phys", "2a", 14)]
        [InlineData("mazurek", "112", "mat", "3a", 15)]
        public async Task CreateActivityAsync_CorrectUpdateOnContext(
            string teacher, string room, string subject, string classGroup, int slot) {

            using var context = PrepareData.GetDbContext(); var activityDTO = new ActivityCreateDTO() {
                ClassGroup = classGroup,
                Room = room,
                Slot = slot,
                Subject = subject,
                Teacher = teacher
            };
            var service = new ActivitiesService(context);
            var value = await service.CreateActivityAsync(activityDTO);
            var activity = GetActivities(context).FirstOrDefault(
                a => a.Teacher.Name == teacher &&
                    a.Subject.Name == subject &&
                    a.Slot.Index == slot &&
                    a.Room.Name == room &&
                    a.ClassGroup.Name == classGroup
            );
            Assert.True(value.IsRight);
            Assert.NotNull(activity);
        }

        [Theory]
        [InlineData("mazurek", "111", "eng", "1a")]
        [InlineData("kowalski", "114", "phys", "3a")]
        [InlineData("mazurek", "113", "esp", "3a")]
        [InlineData("kowalski", "111", "mat", "4a")]
        [InlineData("kowalski", "114", "phys", "1a")]

        public async Task EditActivityAsync_CorrectUpdateOnContext(
            string teacher, string room, string subject, string classGroup) {

            using var context = PrepareData.GetDbContext(); var activity = context.Activities.Include(a => a.Slot).FirstOrDefault();
            int id = activity.Id;
            int slot = activity.Slot.Index;
            var activityDTO = new ActivityEditDTO() {
                Id = id,
                ClassGroup = classGroup,
                Room = room,
                Slot = slot,
                Subject = subject,
                Teacher = teacher
            };
            var service = new ActivitiesService(context);
            var value = await service.EditActivityAsync(activity.Id, activityDTO);
            var activityToCheck = GetActivities(context).FirstOrDefault(
                a =>
                    a.Id == id &&
                    a.Teacher.Name == teacher &&
                    a.Room.Name == room &&
                    a.Subject.Name == subject &&
                    a.ClassGroup.Name == classGroup
                );
            Assert.True(value.IsRight);
            Assert.NotNull(activityToCheck);
        }
        [Theory]
        [InlineData(0, "1a", "kowalski", "111")]
        [InlineData(1, "2a", "nowak", "112")]
        public async Task DeleteActivityAsync_CorrectUpdateOnContext(
            int slot, string classgroup, string teacher, string room) {

            using var context = PrepareData.GetDbContext(); var activityToDelete = GetActivities(context).FirstOrDefault(
                a => a.Slot.Index == slot &&
                    a.Room.Name == room &&
                    a.Teacher.Name == teacher &&
                    a.ClassGroup.Name == classgroup
                );
            var service = new ActivitiesService(context);
            var value = await service.DeleteActivityAsync(activityToDelete.Id, activityToDelete.Timestamp);
            var activity = context.Activities.FirstOrDefault(a => a.Id == activityToDelete.Id);
            Assert.Null(activity);
            Assert.True(value.IsRight);

        }
    }
}