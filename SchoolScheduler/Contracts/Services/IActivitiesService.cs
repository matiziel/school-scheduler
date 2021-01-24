using System.Threading.Tasks;
using Contracts.DataTransferObjects.Activities;

namespace Contracts.Services {
    public interface IActivitiesService {
        Task<ActivityEditDTO> GetActivity(int id);
        Task CreateActivityAsync(ActivityCreateDTO activity);
        Task EditActivityAsync(int id, ActivityEditDTO activity);
        Task DeleteActivityAsync(int id, byte[] timestamp);
    }

}