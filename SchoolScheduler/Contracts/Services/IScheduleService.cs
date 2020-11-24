using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.ViewModels.Schedule;
using Model;

namespace Contracts.Services {
    public interface IScheduleService {
        ActivityEditViewModel GetActivity(int id);
        ScheduleViewModel GetScheduleByGroup(string classGroup);
        ScheduleViewModel GetScheduleByRoom(string room);
        ScheduleViewModel GetScheduleByTeacher(string teacher);

        Task CreateActivity(ActivityEditViewModel activity);
        Task EditActivity(int id, ActivityEditViewModel activity);
        Task DeleteActivity(int id);
    }
}