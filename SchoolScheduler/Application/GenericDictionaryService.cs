using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DataTransferObjects;
using Contracts.DataTransferObjects.Dictionaries;
using Contracts.Services;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Model;
using Persistence;
using static LanguageExt.Prelude;


namespace Application {
    public class GenericDictionaryService<DicitonaryType> where DicitonaryType : DictionaryElementBase, new() {
        protected readonly ApplicationDbContext _context;
        public GenericDictionaryService(ApplicationDbContext context) =>
            _context = context;
        public async Task<Either<ErrorDTO, DictionaryElementEditDTO>> GetDictionaryElementAsync(int id) {
            try {
                var element = await _context.Set<DicitonaryType>().AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                if (element is null)
                    return Left(new ErrorDTO($"Dictionary element does not exist"));
                return Right(new DictionaryElementEditDTO() {
                    Id = element.Id, Name = element.Name, Comment = element.Comment, Timestamp = element.Timestamp
                });
            }
            catch (Exception) {
                return Left(new ErrorDTO("Error while getting dictionary element"));
            }
        }
        public async Task<Either<ErrorDTO, IEnumerable<DictionaryReadDTO>>> GetDictionaryAsync() {
            try {
                return Right(await _context.Set<DicitonaryType>()
                    .Select(c => new DictionaryReadDTO() { Id = c.Id, Name = c.Name })
                    .AsNoTracking()
                    .OrderBy(c => c.Name)
                    .ToListAsync() as IEnumerable<DictionaryReadDTO>);
            }
            catch (Exception) {
                return Left(new ErrorDTO("Error while getting dictionary"));
            }
        }
        public async Task<Either<ErrorDTO, Unit>> AddKey(DictionaryElementCreateDTO element) {
            try {
                if (!ValidateName(element.Name))
                    return Left(new ErrorDTO($"Dictionary element with name: {element.Name} has already exist"));
                await _context.Set<DicitonaryType>().AddAsync(new DicitonaryType() { Name = element.Name, Comment = element.Comment });
                await _context.SaveChangesAsync();
                return Right(Unit.Default);
            }
            catch (Exception) {
                return Left(new ErrorDTO("Error while getting dictionary element"));
            }
        }
        private bool ValidateName(string name, int? id = null) {
            IQueryable<DicitonaryType> elements = _context.Set<DicitonaryType>().AsNoTracking();
            if (id != null)
                elements = elements.Where(d => d.Id != id);
            if (elements.FirstOrDefault(d => d.Name == name) != null)
                return false;
            return true;
        }
        public async Task<Either<ErrorDTO, Unit>> UpdateKey(DictionaryElementEditDTO element) {
            try {
                if (!ValidateName(element.Name, element.Id))
                    return Left(new ErrorDTO($"Dictionary element with name: {element.Name} has already exist"));

                var dictionaryElement = await _context.Set<DicitonaryType>().FirstOrDefaultAsync(c => c.Id == element.Id);

                if (dictionaryElement is null)
                    return Left(new ErrorDTO("Dictionary element with id: " + element.Id + " does not exist"));

                dictionaryElement.Update(element.Name, element.Comment);
                _context.Entry(dictionaryElement).Property("Timestamp").OriginalValue = element.Timestamp;
                _context.Update<DicitonaryType>(dictionaryElement);
                await _context.SaveChangesAsync();
                return Right(Unit.Default);
            }
            catch (DbUpdateConcurrencyException) {
                return Left(new ErrorDTO("Someone has already updated this element"));
            }
            catch (Exception e) {
                return Left(new ErrorDTO(e.Message));
            }

        }
        public async Task<Either<ErrorDTO, Unit>> RemoveKey(int id, byte[] timestamp) {
            try {
                var dictionaryElement = await _context.Set<DicitonaryType>().FirstOrDefaultAsync(c => c.Id == id);
                if(dictionaryElement is null)
                     return Left(new ErrorDTO("Element has already been deleted"));
                     
                _context.Entry(dictionaryElement).Property("Timestamp").OriginalValue = timestamp;
                _context.Remove<DicitonaryType>(dictionaryElement);
                await _context.SaveChangesAsync();
                return Right(Unit.Default);
            }
            catch (DbUpdateConcurrencyException) {
                return Left(new ErrorDTO("Someone has already updated this element"));
            }
            catch (Exception e) {
                return Left(new ErrorDTO(e.Message));
            }
        }
    }
}
