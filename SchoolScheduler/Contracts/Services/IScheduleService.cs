using System.Threading.Tasks;
using Contracts.DataTransferObjects.Schedule;

namespace Contracts.Services {
    public interface IScheduleService {
        ScheduleDTO GetScheduleByGroup(string classGroup);
        ScheduleDTO GetScheduleByRoom(string room);
        ScheduleDTO GetScheduleByTeacher(string teacher);
    }

}