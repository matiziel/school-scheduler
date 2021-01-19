using Xunit;
using Application;
using Persistence;
using Contracts.DataTransferObjects.Schedule;
using System.Linq;
using Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Contracts.Services;

namespace UnitTests.ScheduleServicesTests {
    public class GetData_CheckCorrectPaths {
        
        private IQueryable<Activity> GetActivities(ApplicationDbContext context) {
            return context.Activities
                .Include(a => a.Slot)
                .Include(a => a.Teacher)
                .Include(a => a.Room)
                .Include(a => a.ClassGroup)
                .Include(a => a.Subject);
        }
        [Fact]
        public async Task GetActivityAsync_ReturnCorrectDTO() {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ActivitiesService(context);
                var activity = GetActivities(context).FirstOrDefault();
                var activityDTO = await service.GetActivity(activity.Id);
                Assert.Equal(activity.Id, activityDTO.Id);
                Assert.Equal(activity.Room.Name, activityDTO.Room);
                Assert.Equal(activity.Slot.Index, activityDTO.Slot);
                Assert.Equal(activity.Subject.Name, activityDTO.Subject);
                Assert.Equal(activity.Teacher.Name, activityDTO.Teacher);
                Assert.Equal(activity.ClassGroup.Name, activityDTO.ClassGroup);
            }
        }
        [Theory]
        [InlineData("1a", "111 mat", 0)]
        [InlineData("2a", "112 phys", 1)]
        public void GetScheduleByGroup_ReturnCorrectDTO(string group, string title, int index) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context);
                var scheduleDTO = service.GetScheduleByGroup(group);
                Assert.Equal(title, scheduleDTO.Slots[index].Title);
                Assert.Equal("Group", scheduleDTO.Type);
                Assert.Equal(group, scheduleDTO.Name);
            }
        }
        [Theory]
        [InlineData("111", "1a", 0)]
        [InlineData("112", "2a", 1)]
        public void GetScheduleByRoom_ReturnCorrectDTO(string room, string title, int index) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context);
                var scheduleDTO = service.GetScheduleByRoom(room);
                Assert.Equal(title, scheduleDTO.Slots[index].Title);
                Assert.Equal("Room", scheduleDTO.Type);
                Assert.Equal(room, scheduleDTO.Name);
            }
        }
        [Theory]
        [InlineData("kowalski", "111 mat 1a", 0)]
        [InlineData("nowak", "112 phys 2a", 1)]
        public void GetScheduleByTeacher_ReturnCorrectDTO(string teacher, string title, int index) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context);
                var scheduleDTO = service.GetScheduleByTeacher(teacher);
                Assert.Equal(title, scheduleDTO.Slots[index].Title);
                Assert.Equal("Teacher", scheduleDTO.Type);
                Assert.Equal(teacher, scheduleDTO.Name);
            }
        }
    }
}