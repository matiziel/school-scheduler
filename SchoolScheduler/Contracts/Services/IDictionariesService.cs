using System.Collections.Generic;
using Contracts.DataTransferObjects.Dictionaries;
using Common;
using System.Threading.Tasks;

namespace Contracts.Services {
    public interface IDictionariesService {
        Task<DictionaryElementEditDTO> GetDictionaryElementAsync(int id);
        Task<IEnumerable<DictionaryReadDTO>> GetDictionaryAsync();
        Task AddKey(DictionaryElementCreateDTO element);
        Task UpdateKey(DictionaryElementEditDTO element);
        Task RemoveKey(int id, byte[] timestamp);
        IEnumerable<string> GetDictionaryBySlot(int slot, int? id = null);
    }
}