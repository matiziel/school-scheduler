using Contracts.ViewModels;

namespace Contracts.Services {
    public interface IScheduleService {
        ScheduleViewModel GetScheduleByGroup(string group);
        ScheduleViewModel GetScheduleByRoom(string room);
        ScheduleViewModel GetScheduleByTeacher(string teacher);
    }
}