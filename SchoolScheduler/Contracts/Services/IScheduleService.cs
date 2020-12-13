using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.ViewModels.Schedule;
using Model;

namespace Contracts.Services {
    public interface IScheduleService {
        Task<ActivityByGroupEditViewModel> GetActivityByGroupAsync(int id);
        ActivityByGroupEditViewModel GetEmptyActivityByGroup(int slot, string group);
        
        Task<ActivityByRoomEditViewModel> GetActivityByRoomAsync(int id);
        ActivityByRoomEditViewModel GetEmptyActivityByRoom(int slot, string room);

        Task<ActivityByTeacherEditViewModel> GetActivityByTeacherAsync(int id);
        ActivityByTeacherEditViewModel GetEmptyActivityByTeacher(int slot, string teacher);

        ScheduleViewModel GetScheduleByGroup(string classGroup);
        ScheduleViewModel GetScheduleByRoom(string room);
        ScheduleViewModel GetScheduleByTeacher(string teacher);

        Task CreateActivityAsync(ActivityViewModel activity);
        Task EditActivityAsync(int id, ActivityViewModel activity);
        Task DeleteActivityAsync(int id, byte[] timestamp);
    }
}