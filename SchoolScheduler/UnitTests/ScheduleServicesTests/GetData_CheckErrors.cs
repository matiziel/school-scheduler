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

        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(3000)]
        public async Task GetActivityByGroupAsync_GiveIncorrectId_GetError(int id) {
            using var context = PrepareData.GetDbContext(); var service = new ActivitiesService(context);
            var test = await service.GetActivity(id);
            Assert.True(test.IsLeft);
        }

    }
}