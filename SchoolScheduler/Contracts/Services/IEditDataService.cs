using System.Collections.Generic;
using Contracts.ViewModels;
using Model;

namespace Contracts.Services {
    public interface IEditDataService {
        IEnumerable<string> GetAllGroups();
        IEnumerable<string> GetAllRooms();
        IEnumerable<string> GetAllTeachers();
        IEnumerable<string> GetAllClasses();

        List<string> GetDictionary(DataType type);
        void AddKey(string value, DataType type);
        void DeleteKey(string value, DataType type);
        IEnumerable<string> GetFreeGroupsBySlot(int slot, int? id = null);
        IEnumerable<string> GetFreeRoomsBySlot(int slot, int? id = null);
        IEnumerable<string> GetFreeTeachersBySlot(int slot, int? id = null);
    }
}