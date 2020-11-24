using System.Collections.Generic;
using Contracts.ViewModels;
using Common;
using System.Threading.Tasks;

namespace Contracts.Services {
    public interface IDisctionariesService {
        IEnumerable<string> GetAllClassGroups();
        IEnumerable<string> GetAllRooms();
        IEnumerable<string> GetAllTeachers();
        IEnumerable<string> GetAllSubjects();

        IEnumerable<string> GetDictionary(DataType type);
        IEnumerable<string> GetFreeClassGroupsBySlot(int slot, int? id = null);
        IEnumerable<string> GetFreeRoomsBySlot(int slot, int? id = null);
        IEnumerable<string> GetFreeTeachersBySlot(int slot, int? id = null);
    }
}