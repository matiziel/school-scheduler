using System.Threading.Tasks;
using Contracts.DataTransferObjects;
using Contracts.DataTransferObjects.Activities;
using LanguageExt;

namespace Contracts.Services {
    public interface IActivitiesService {
        Task<Either<ErrorDTO, ActivityEditDTO>> GetActivity(int id);
        Task<Either<ErrorDTO, Unit>> CreateActivityAsync(ActivityCreateDTO activity);
        Task<Either<ErrorDTO, Unit>> EditActivityAsync(int id, ActivityEditDTO activity);
        Task<Either<ErrorDTO, Unit>> DeleteActivityAsync(int id, byte[] timestamp);
    }
}