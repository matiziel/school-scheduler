using System.Collections.Generic;
using Contracts.ViewModels;
using Model;

namespace Contracts.Services {
    public interface IScheduleService {
        ActivityEditViewModel GetActivity(int id);
        ScheduleViewModel GetScheduleByGroup(string group);
        ScheduleViewModel GetScheduleByRoom(string room);
        ScheduleViewModel GetScheduleByTeacher(string teacher);

        void CreateActivity(ActivityEditViewModel activity);
        void EditActivity(int id, ActivityEditViewModel activity);
        void DeleteActivity(int id);
    }
}