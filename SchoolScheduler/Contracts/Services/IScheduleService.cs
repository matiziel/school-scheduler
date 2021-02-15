using System.Threading.Tasks;
using Contracts.DataTransferObjects;
using Contracts.DataTransferObjects.Schedule;
using LanguageExt;

namespace Contracts.Services {
    public interface IScheduleService {
        Either<ErrorDTO, ScheduleDTO> GetScheduleByGroup(string classGroup);
        Either<ErrorDTO, ScheduleDTO>  GetScheduleByRoom(string room);
        Either<ErrorDTO, ScheduleDTO>  GetScheduleByTeacher(string teacher);
    }

}