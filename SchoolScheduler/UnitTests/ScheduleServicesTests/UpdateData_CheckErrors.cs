using Xunit;
using Application;
using Persistence;
using Contracts.ViewModels.Schedule;
using System.Linq;
using Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace UnitTests.ScheduleServicesTests {
    public class UpdateData_CheckErrors {
        private IQueryable<Activity> GetActivities(ApplicationDbContext context) {
            return context.Activities
                .Include(a => a.Slot)
                .Include(a => a.Teacher)
                .Include(a => a.Room)
                .Include(a => a.ClassGroup)
                .Include(a => a.Subject);
        }

        [Fact]
        public async Task CreateActivityAsync_WhenActivityViewModelIsNull_CheckThrows() {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context, new DisctionariesService(context));
                await Assert.ThrowsAsync<ArgumentException>(
                    async () => await service.CreateActivityAsync(null));
            }
        }
        [Theory]
        [InlineData("nowak", "112", "mat", "1a", 0)]
        [InlineData("mazurek", "111", "phys", "2a", 0)]
        [InlineData("kowalski", "113", "eng", "3a", 0)]
        [InlineData("nowak", "111", "mat", "1a", 0)]
        [InlineData("kowalski", "111", "phys", "2a", 0)]
        [InlineData("kowalski", "113", "eng", "1a", 0)]
        public async Task CreateActivityAsync_WhenOneOfValuesIsOccupied_CheckThrowsInvalidOperation(
            string teacher, string room, string subject, string classGroup, int slot) {
            using (var context = PrepareData.GetDbContext()) {
                var activityViewModel = new ActivityViewModel() {
                    ClassGroup = classGroup,
                    Room = room,
                    Slot = slot,
                    Subject = subject,
                    Teacher = teacher
                };
                var service = new ScheduleService(context, new DisctionariesService(context));
                await Assert.ThrowsAsync<InvalidOperationException>(
                    async () => await service.CreateActivityAsync(activityViewModel));
            }
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
        public async Task CreateActivityAsync_WhenOneOfValuesIsNullOrDoesNotExist_CheckArgumentException(
    string teacher, string room, string subject, string classGroup, int slot) {
            using (var context = PrepareData.GetDbContext()) {
                var activityViewModel = new ActivityViewModel() {
                    ClassGroup = classGroup,
                    Room = room,
                    Slot = slot,
                    Subject = subject,
                    Teacher = teacher
                };
                var service = new ScheduleService(context, new DisctionariesService(context));
                await Assert.ThrowsAsync<ArgumentException>(
                    async () => await service.CreateActivityAsync(activityViewModel));
            }
        }
        [Theory]
        [InlineData(10000)]
        [InlineData(20000)]
        public async Task DeleteActivityAsync_CheckThrowsInNull(int id) {

            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context, new DisctionariesService(context));
                await Assert.ThrowsAsync<ArgumentException>(
                    async () => await service.DeleteActivityAsync(id, null));
            }
        }
    }
}