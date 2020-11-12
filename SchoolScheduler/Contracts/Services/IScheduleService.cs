using System.Collections.Generic;
using Contracts.ViewModels;
using Model;

namespace Contracts.Services {
    public interface IScheduleService {
        Activity GetActivity(int id);
        ScheduleViewModel GetScheduleByGroup(string group);
        ScheduleViewModel GetScheduleByRoom(string room);
        ScheduleViewModel GetScheduleByTeacher(string teacher);


        List<string> GetAllGroups();
        List<string> GetAllRooms();
        List<string> GetAllTeachers();
        List<string> GetAllClasses();
        void CreateActivity(Activity activity);
        void EditActivity(int id, Activity activity);
    }
}