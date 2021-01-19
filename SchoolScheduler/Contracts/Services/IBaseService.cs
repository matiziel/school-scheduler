using System.Threading.Tasks;
using Contracts.DataTransferObjects.Schedule;

namespace Contracts.Services
{
    public interface IBaseService<ViewModel> where ViewModel : ActivityViewModel
    {
        Task<ViewModel> GetActivityAsync(int id);
        ViewModel GetEmptyActivity(int slot, string name);
        ScheduleViewModel GetSchedule(string name);
        Task CreateActivityAsync(ActivityViewModel activity);
        Task EditActivityAsync(int id, ActivityViewModel activity);
        Task DeleteActivityAsync(int id, byte[] timestamp);
    }
}