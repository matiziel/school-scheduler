using System.Collections.Generic;
using Contracts.ViewModels;
using Common;

namespace Contracts.Services {
    public interface IDisctionariesService {
        IEnumerable<string> GetAllClassGroups();
        IEnumerable<string> GetAllRooms();
        IEnumerable<string> GetAllTeachers();
        IEnumerable<string> GetAllSubjects();

        List<string> GetDictionary(DataType type);
        IEnumerable<string> GetFreeGroupsBySlot(int slot, int? id = null);
        IEnumerable<string> GetFreeRoomsBySlot(int slot, int? id = null);
        IEnumerable<string> GetFreeTeachersBySlot(int slot, int? id = null);
    }
}