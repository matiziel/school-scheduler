using Xunit;
using Application;
using Persistence;
using Contracts.DataTransferObjects.Activities;
using System.Linq;
using Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace UnitTests.ScheduleServicesTests {
    public class UpdateData_CheckErrors {

        [Fact]
        public async Task CreateActivityAsync_WhenActivityDTOIsNull_CheckErrorOccured() {
            using var context = PrepareData.GetDbContext(); var service = new ActivitiesService(context);
            var value = await service.CreateActivityAsync(null);
            Assert.True(value.IsLeft);
        }
        [Theory]
        [InlineData("nowak", "112", "mat", "1a", 0)]
        [InlineData("mazurek", "111", "phys", "2a", 0)]
        [InlineData("kowalski", "113", "eng", "3a", 0)]
        [InlineData("nowak", "111", "mat", "1a", 0)]
        [InlineData("kowalski", "111", "phys", "2a", 0)]
        [InlineData("kowalski", "113", "eng", "1a", 0)]
        public async Task CreateActivityAsync_WhenOneOfValuesIsOccupied_CheckErrorOccured(
            string teacher, string room, string subject, string classGroup, int slot) {
            using var context = PrepareData.GetDbContext(); var service = new ActivitiesService(context);
            var value = await service.CreateActivityAsync(GetDTOToCreate(teacher, room, subject, classGroup, slot));
            Assert.True(value.IsLeft);
        }
        [Theory]
        [InlineData("nowak", "112", "mat", null, 22)]
        [InlineData("mazurek", "111", null, "2a", 23)]
        [InlineData("kowalski", null, "eng", "3a", 25)]
        [InlineData(null, "111", "mat", "1a", 26)]
        [InlineData("xxx", "111", "phys", "2a", 10)]
        [InlineData("kowalski", "xxx", "eng", "1a", 11)]
        [InlineData("kowalski", "111", "xxx", "2a", 12)]
        [InlineData("kowalski", "113", "eng", "xxx", 13)]
        [InlineData("kowalski", "111", "phys", "2a", 76)]
        public async Task CreateActivityAsync_WhenOneOfValuesIsNullOrDoesNotExist_CheckErrorOccured(
            string teacher, string room, string subject, string classGroup, int slot) {
            using var context = PrepareData.GetDbContext();
            var service = new ActivitiesService(context);
            var value = await service.CreateActivityAsync(GetDTOToCreate(teacher, room, subject, classGroup, slot));
            Assert.True(value.IsLeft);
        }
        private ActivityCreateDTO GetDTOToCreate(
            string teacher, string room, string subject, string classGroup, int slot) {

            return new ActivityCreateDTO() {
                ClassGroup = classGroup,
                Room = room,
                Slot = slot,
                Subject = subject,
                Teacher = teacher
            };
        }
        [Fact]
        public async Task EditActivityAsync_WhenActivityDTOIsNull_CheckErrorOccured() {
            using var context = PrepareData.GetDbContext(); var service = new ActivitiesService(context);
            var value = await service.EditActivityAsync(1, null);
            Assert.True(value.IsLeft);
        }
        [Theory]
        [InlineData("kowalski", "112", "eng", "3a")]
        [InlineData("nowak", "114", "phys", "1a")]
        [InlineData("mazurek", "111", "esp", "2a")]
        [InlineData("kowalski", "111", "esp", "1a")]
        public async Task EditActivityAsync_WhenOneOfValuesIsOccupied_CheckErrorOccured(
            string teacher, string room, string subject, string classGroup) {

            using var context = PrepareData.GetDbContext(); var activityDTO = GetDTOToEdit(teacher, room, subject, classGroup, context);
            var service = new ActivitiesService(context);
            var value = await service.EditActivityAsync(activityDTO.Id, activityDTO);
            Assert.True(value.IsLeft);
        }
        [Theory]
        [InlineData("asddas", "114", "esp", "4a")]
        [InlineData("clarkson", "fsfdsf", "esp", "5a")]
        [InlineData("may", "115", "sdfsdg", "6a")]
        [InlineData("hammond", "116", "esp", "sdfsg")]
        [InlineData(null, "117", "esp", "7a")]
        [InlineData("nowak", null, "esp", "6a")]
        [InlineData("clarkson", "112", null, "7a")]
        [InlineData("may", "117", "esp", null)]
        public async Task EditActivityAsync_WhenOneOfValuesIsIncorrect_CheckErrorOccured(
            string teacher, string room, string subject, string classGroup) {

            using var context = PrepareData.GetDbContext(); var activityDTO = GetDTOToEdit(teacher, room, subject, classGroup, context);
            var service = new ActivitiesService(context);
            var value = await service.EditActivityAsync(activityDTO.Id, activityDTO);
            Assert.True(value.IsLeft);
        }
        private ActivityEditDTO GetDTOToEdit(
            string teacher, string room, string subject, string classGroup, ApplicationDbContext context) {

            var activity = context.Activities.Include(a => a.Slot).Include(a => a.Room)
                    .FirstOrDefault(a => a.Slot.Index == 0 && a.Room.Name == "113");
            int id = activity.Id;
            int slot = activity.Slot.Index;
            return new ActivityEditDTO() {
                Id = id,
                ClassGroup = classGroup,
                Room = room,
                Slot = slot,
                Subject = subject,
                Teacher = teacher
            };
        }
        [Theory]
        [InlineData(10000)]
        [InlineData(20000)]
        public async Task DeleteActivityAsync_CheckErrorOccured(int id) {

            using var context = PrepareData.GetDbContext(); var service = new ActivitiesService(context);
            var value = await service.DeleteActivityAsync(id, null);
            Assert.True(value.IsLeft);
        }
    }
}