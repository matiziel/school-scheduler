using System.Threading.Tasks;
using Contracts.DataTransferObjects.Schedule;

namespace Contracts.Services {
    public interface IScheduleService {
        Task<ActivityEditDTO> GetActivity(int id);
        ScheduleDTO GetScheduleByGroup(string classGroup);
        ScheduleDTO GetScheduleByRoom(string room);

        ScheduleDTO GetScheduleByTeacher(string teacher);
        Task CreateActivityAsync(ActivityCreateDTO activity);
        Task EditActivityAsync(int id, ActivityEditDTO activity);
        Task DeleteActivityAsync(int id, ActivityDeleteDTO activityDTO);
    }

}