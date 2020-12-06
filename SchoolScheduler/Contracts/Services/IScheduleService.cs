using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.ViewModels.Schedule;
using Model;

namespace Contracts.Services {
    public interface IScheduleService {
        Task<ActivityEditViewModel> GetActivityAsync(int id);
        ScheduleViewModel GetScheduleByGroup(string classGroup);
        ScheduleViewModel GetScheduleByRoom(string room);
        ScheduleViewModel GetScheduleByTeacher(string teacher);

        Task CreateActivityAsync(ActivityEditViewModel activity);
        Task EditActivityAsync(int id, ActivityEditViewModel activity);
        Task DeleteActivityAsync(int id, byte[] timestamp);
    }
}