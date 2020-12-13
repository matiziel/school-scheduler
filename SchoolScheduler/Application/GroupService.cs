using System.Threading.Tasks;
using Contracts.ViewModels.Schedule;

namespace Contracts.Services {
    public class GroupService : IGroupService {
        private readonly IScheduleService _scheduleService;
        public GroupService(IScheduleService scheduleService) =>
            _scheduleService = scheduleService;
        public async Task CreateActivityAsync(ActivityViewModel activity) =>
            await _scheduleService.CreateActivityAsync(activity);

        public async Task DeleteActivityAsync(int id, byte[] timestamp) =>
            await _scheduleService.DeleteActivityAsync(id, timestamp);

        public async Task EditActivityAsync(int id, ActivityViewModel activity) =>
            await _scheduleService.EditActivityAsync(id, activity);

        public async Task<ActivityByGroupEditViewModel> GetActivityAsync(int id) =>
            await _scheduleService.GetActivityByGroupAsync(id);

        public ActivityByGroupEditViewModel GetEmptyActivity(int slot, string group) =>
            _scheduleService.GetEmptyActivityByGroup(slot, group);

        public ScheduleViewModel GetSchedule(string classGroup) =>
            _scheduleService.GetScheduleByGroup(classGroup);
    }
}