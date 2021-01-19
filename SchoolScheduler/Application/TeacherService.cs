using System.Threading.Tasks;
using Contracts.DataTransferObjects.Schedule;

namespace Contracts.Services {
    public class TeacherService : ITeacherService {
        private readonly IScheduleService _scheduleService;
        public TeacherService(IScheduleService scheduleService) =>
            _scheduleService = scheduleService;

        public async Task CreateActivityAsync(ActivityViewModel activity) =>
            await _scheduleService.CreateActivityAsync(activity);
        
        public async Task DeleteActivityAsync(int id, byte[] timestamp) =>
            await _scheduleService.DeleteActivityAsync(id, timestamp);

        public async Task EditActivityAsync(int id, ActivityViewModel activity) =>
            await _scheduleService.EditActivityAsync(id, activity);

        public async Task<ActivityByTeacherEditViewModel> GetActivityAsync(int id) =>
            await _scheduleService.GetActivityByTeacherAsync(id);

        public ActivityByTeacherEditViewModel GetEmptyActivity(int slot, string teacher) =>
            _scheduleService.GetEmptyActivityByTeacher(slot, teacher);

        public ScheduleViewModel GetSchedule(string teacher) =>
            _scheduleService.GetScheduleByTeacher(teacher);
    }
}