using System.Collections.Generic;
using Contracts.DataTransferObjects.Dictionaries;
using Common;
using System.Threading.Tasks;
using LanguageExt;
using Contracts.DataTransferObjects;

namespace Contracts.Services {
    public interface IDictionariesService {
        Task<Either<ErrorDTO, DictionaryElementEditDTO>> GetDictionaryElementAsync(int id);
        Task<Either<ErrorDTO, IEnumerable<DictionaryReadDTO>>> GetDictionaryAsync();
        Task<Either<ErrorDTO, Unit>> AddKey(DictionaryElementCreateDTO element);
        Task<Either<ErrorDTO, Unit>> UpdateKey(DictionaryElementEditDTO element);
        Task<Either<ErrorDTO, Unit>> RemoveKey(int id, byte[] timestamp);
        Either<ErrorDTO, IEnumerable<string>> GetDictionaryBySlot(int slot, int? id = null);
    }
}