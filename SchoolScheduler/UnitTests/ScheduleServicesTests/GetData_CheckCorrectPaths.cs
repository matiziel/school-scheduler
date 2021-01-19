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
        public async Task GetActivityByGroupAsync_ReturnCorrectViewModel() {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context, new DictionariesService(context));
                var activity = GetActivities(context).FirstOrDefault();
                var activityViewModel = await service.GetActivityByGroupAsync(activity.Id);
                Assert.Equal(activity.Id, activityViewModel.Id);
                Assert.Equal(activity.Room.Name, activityViewModel.Room);
                Assert.Equal(activity.Slot.Index, activityViewModel.Slot);
                Assert.Equal(activity.Subject.Name, activityViewModel.Subject);
                Assert.Equal(activity.Teacher.Name, activityViewModel.Teacher);
                Assert.Equal(activity.ClassGroup.Name, activityViewModel.ClassGroup);
                Assert.Equal(activity.ClassGroup.Name, activityViewModel.ArgumentHelper);
                Assert.NotNull(activityViewModel.ListOfClasses);
                Assert.NotNull(activityViewModel.ListOfRooms);
                Assert.NotNull(activityViewModel.ListOfTeachers);
            }
        }
        [Fact]
        public async Task GetActivityByRoomAsync_ReturnCorrectViewModel() {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context, new DictionariesService(context));
                var activity = GetActivities(context).FirstOrDefault();
                var activityViewModel = await service.GetActivityByRoomAsync(activity.Id);
                Assert.Equal(activity.Id, activityViewModel.Id);
                Assert.Equal(activity.Room.Name, activityViewModel.Room);
                Assert.Equal(activity.Slot.Index, activityViewModel.Slot);
                Assert.Equal(activity.Subject.Name, activityViewModel.Subject);
                Assert.Equal(activity.Teacher.Name, activityViewModel.Teacher);
                Assert.Equal(activity.ClassGroup.Name, activityViewModel.ClassGroup);
                Assert.Equal(activity.Room.Name, activityViewModel.ArgumentHelper);
                Assert.NotNull(activityViewModel.ListOfClasses);
                Assert.NotNull(activityViewModel.ListOfGroups);
                Assert.NotNull(activityViewModel.ListOfTeachers);
            }
        }
        [Fact]
        public async Task GetActivityByTeacherAsync_ReturnCorrectViewModel() {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context, new DictionariesService(context));
                var activity = GetActivities(context).FirstOrDefault();
                var activityViewModel = await service.GetActivityByTeacherAsync(activity.Id);
                Assert.Equal(activity.Id, activityViewModel.Id);
                Assert.Equal(activity.Room.Name, activityViewModel.Room);
                Assert.Equal(activity.Slot.Index, activityViewModel.Slot);
                Assert.Equal(activity.Subject.Name, activityViewModel.Subject);
                Assert.Equal(activity.Teacher.Name, activityViewModel.Teacher);
                Assert.Equal(activity.ClassGroup.Name, activityViewModel.ClassGroup);
                Assert.Equal(activity.Teacher.Name, activityViewModel.ArgumentHelper);
                Assert.NotNull(activityViewModel.ListOfClasses);
                Assert.NotNull(activityViewModel.ListOfRooms);
                Assert.NotNull(activityViewModel.ListOfGroups);
            }
        }
        [Theory]
        [InlineData(2, "1a")]
        [InlineData(3, "3a")]
        [InlineData(4, "2a")]
        public void GetEmptyActivityByGroup_ReturnCorrectViewModel(int slot, string group) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context, new DictionariesService(context));
                var activityViewModel = service.GetEmptyActivityByGroup(slot, group);
                Assert.Equal(slot, activityViewModel.Slot);
                Assert.Equal(group, activityViewModel.ClassGroup);
                Assert.Equal(group, activityViewModel.ArgumentHelper);
                Assert.NotNull(activityViewModel.ListOfClasses);
                Assert.NotNull(activityViewModel.ListOfRooms);
                Assert.NotNull(activityViewModel.ListOfTeachers);
            }
        }
        [Theory]
        [InlineData(2, "111")]
        [InlineData(3, "112")]
        [InlineData(4, "113")]
        public void GetEmptyActivityByRoom_ReturnCorrectViewModel(int slot, string room) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context, new DictionariesService(context));
                var activityViewModel = service.GetEmptyActivityByRoom(slot, room);
                Assert.Equal(room, activityViewModel.Room);
                Assert.Equal(slot, activityViewModel.Slot);
                Assert.Equal(room, activityViewModel.ArgumentHelper);
                Assert.NotNull(activityViewModel.ListOfClasses);
                Assert.NotNull(activityViewModel.ListOfGroups);
                Assert.NotNull(activityViewModel.ListOfTeachers);
            }
        }
        [Theory]
        [InlineData(2, "kowalski")]
        [InlineData(3, "nowak")]
        [InlineData(4, "mazurek")]
        public void GetEmptyActivityByTeacher_ReturnCorrectViewModel(int slot, string teacher) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context, new DictionariesService(context));
                var activityViewModel = service.GetEmptyActivityByTeacher(slot, teacher);
                Assert.Equal(slot, activityViewModel.Slot);
                Assert.Equal(teacher, activityViewModel.Teacher);
                Assert.Equal(teacher, activityViewModel.ArgumentHelper);
                Assert.NotNull(activityViewModel.ListOfClasses);
                Assert.NotNull(activityViewModel.ListOfRooms);
                Assert.NotNull(activityViewModel.ListOfGroups);
            }
        }
        [Theory]
        [InlineData("1a", "111 mat", 0)]
        [InlineData("2a", "112 phys", 1)]
        public void GetScheduleByGroup_ReturnCorrectViewModel(string group, string title, int index) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context, new DictionariesService(context));
                var scheduleViewModel = service.GetScheduleByGroup(group);
                Assert.Equal(title, scheduleViewModel.Slots[index].Title);
                Assert.Equal("Group", scheduleViewModel.Type);
                Assert.NotNull(scheduleViewModel.Names);
                Assert.Equal(group, scheduleViewModel.Name);
            }
        }
        [Theory]
        [InlineData("111", "1a", 0)]
        [InlineData("112", "2a", 1)]
        public void GetScheduleByRoom_ReturnCorrectViewModel(string room, string title, int index) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context, new DictionariesService(context));
                var scheduleViewModel = service.GetScheduleByRoom(room);
                Assert.Equal(title, scheduleViewModel.Slots[index].Title);
                Assert.Equal("Room", scheduleViewModel.Type);
                Assert.NotNull(scheduleViewModel.Names);
                Assert.Equal(room, scheduleViewModel.Name);
            }
        }
        [Theory]
        [InlineData("kowalski", "111 mat 1a", 0)]
        [InlineData("nowak", "112 phys 2a", 1)]
        public void GetScheduleByTeacher_ReturnCorrectViewModel(string teacher, string title, int index) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new ScheduleService(context, new DictionariesService(context));
                var scheduleViewModel = service.GetScheduleByTeacher(teacher);
                Assert.Equal(title, scheduleViewModel.Slots[index].Title);
                Assert.Equal("Teacher", scheduleViewModel.Type);
                Assert.NotNull(scheduleViewModel.Names);
                Assert.Equal(teacher, scheduleViewModel.Name);
            }
        }
    }
}