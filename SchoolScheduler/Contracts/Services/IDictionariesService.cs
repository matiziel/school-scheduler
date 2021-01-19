using System.Collections.Generic;
using Contracts.DataTransferObjects.Dictionaries;
using Common;
using System.Threading.Tasks;

namespace Contracts.Services {
    public interface IDictionariesService {
        Task<DictionaryElementEditDTO> GetDictionaryElementAsync(int id, DataType type);
        Task<IEnumerable<DictionaryReadDTO>> GetDictionaryAsync(DataType type);
        IEnumerable<string> GetAllClassGroups();
        IEnumerable<string> GetAllRooms();
        IEnumerable<string> GetAllTeachers();
        IEnumerable<string> GetAllSubjects();
        Task AddKey(DictionaryElementCreateDTO element, DataType type);
        Task UpdateKey(DictionaryElementEditDTO element, DataType type);
        Task RemoveKey(int id, byte[] timestamp, DataType type);
        IEnumerable<string> GetFreeClassGroupsBySlot(int slot, int? id = null);
        IEnumerable<string> GetFreeRoomsBySlot(int slot, int? id = null);
        IEnumerable<string> GetFreeTeachersBySlot(int slot, int? id = null);
    }
}