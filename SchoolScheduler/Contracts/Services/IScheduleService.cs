using System.Collections.Generic;
using Contracts.ViewModels;
using Model;

namespace Contracts.Services {
    public interface IScheduleService {
        Activity GetActivity(int slot, string room);
        ScheduleViewModel GetScheduleByGroup(string group);
        ScheduleViewModel GetScheduleByRoom(string room);
        ScheduleViewModel GetScheduleByTeacher(string teacher);


        List<string> GetAllGroups();
    }
}