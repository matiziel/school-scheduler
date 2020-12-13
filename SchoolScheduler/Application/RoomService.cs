using System.Threading.Tasks;
using Contracts.Services;
using Contracts.ViewModels.Schedule;

namespace Application {
    public class RoomService : IRoomService {
        private readonly IScheduleService _scheduleService;
        public RoomService(IScheduleService scheduleService) =>
            _scheduleService = scheduleService;
            
        public async Task CreateActivityAsync(ActivityViewModel activity) =>
            await _scheduleService.CreateActivityAsync(activity);

        public async Task DeleteActivityAsync(int id, byte[] timestamp) =>
            await _scheduleService.DeleteActivityAsync(id, timestamp);

        public async Task EditActivityAsync(int id, ActivityViewModel activity) =>
            await _scheduleService.EditActivityAsync(id, activity);

        public async Task<ActivityByRoomEditViewModel> GetActivityAsync(int id) =>
            await _scheduleService.GetActivityByRoomAsync(id);

        public ActivityByRoomEditViewModel GetEmptyActivity(int slot, string room) =>
            _scheduleService.GetEmptyActivityByRoom(slot, room);

        public ScheduleViewModel GetSchedule(string room) =>
            _scheduleService.GetScheduleByRoom(room);
        
    }
}