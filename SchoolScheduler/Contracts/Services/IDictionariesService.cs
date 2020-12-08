using System.Collections.Generic;
using Contracts.ViewModels.Dictionaries;
using Common;
using System.Threading.Tasks;

namespace Contracts.Services {
    public interface IDisctionariesService {
        Task<DictionaryElementEditViewModel> GetDictionaryElementAsync(int id, DataType type);
        Task<IEnumerable<DictionaryIndexViewModel>> GetDictionaryAsync(DataType type);
        IEnumerable<string> GetAllClassGroups();
        IEnumerable<string> GetAllRooms();
        IEnumerable<string> GetAllTeachers();
        IEnumerable<string> GetAllSubjects();
        Task AddKey(DictionaryElementEditViewModel element, DataType type);
        Task UpdateKey(DictionaryElementEditViewModel element, DataType type);
        Task RemoveKey(int id, byte[] timestamp, DataType type);
        IEnumerable<string> GetFreeClassGroupsBySlot(int slot, int? id = null);
        IEnumerable<string> GetFreeRoomsBySlot(int slot, int? id = null);
        IEnumerable<string> GetFreeTeachersBySlot(int slot, int? id = null);
    }
}