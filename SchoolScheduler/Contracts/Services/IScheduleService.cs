using System.Collections.Generic;
using Contracts.ViewModels;
using Model;

namespace Contracts.Services {
    public interface IScheduleService {
        Activity GetActivity(int id);
        ScheduleViewModel GetScheduleByGroup(string group);
        ScheduleViewModel GetScheduleByRoom(string room);
        ScheduleViewModel GetScheduleByTeacher(string teacher);

        void CreateActivity(Activity activity);
        void EditActivity(int id, Activity activity);
        void DeleteActivity(int id);
    }
}