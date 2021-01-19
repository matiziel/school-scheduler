using Xunit;
using Application;
using Persistence;
using Contracts.DataTransferObjects.Schedule;
using System.Linq;
using Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Contracts.Services;
using System;

namespace UnitTests.ScheduleServicesTests {
    public class GetData_CheckErrors {
        private IQueryable<Activity> GetActivities(ApplicationDbContext context) {
            return context.Activities
                .Include(a => a.Slot)
                .Include(a => a.Teacher)
                .Include(a => a.Room)
                .Include(a => a.ClassGroup)
                .Include(a => a.Subject);
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(3000)]
        public async Task GetActivityByGroupAsync_GiveIncorrectId_ThrowsArgumentException(int id) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context, new DictionariesService(context));
                await Assert.ThrowsAsync<ArgumentException>(
                    async () => await service.GetActivityByGroupAsync(id));

            }
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(3000)]
        public async Task GetActivityByRoomAsync_GiveIncorrectId_ThrowsArgumentException(int id) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context, new DictionariesService(context));
                await Assert.ThrowsAsync<ArgumentException>(
                    async () => await service.GetActivityByRoomAsync(id));

            }
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(3000)]
        public async Task GetActivityByTeacherAsync_GiveIncorrectId_ThrowsArgumentException(int id) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context, new DictionariesService(context));
                await Assert.ThrowsAsync<ArgumentException>(
                    async () => await service.GetActivityByTeacherAsync(id));

            }
        }

    }
}