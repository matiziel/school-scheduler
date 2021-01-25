using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DataTransferObjects.Dictionaries;
using Contracts.Services;
using Microsoft.EntityFrameworkCore;
using Model;
using Persistence;

namespace Application {
    public class GenericDictionaryService<DicitonaryType> where DicitonaryType : DictionaryElementBase, new() {
        protected readonly ApplicationDbContext _context;
        public GenericDictionaryService(ApplicationDbContext context) =>
            _context = context;
        public async Task<DictionaryElementEditDTO> GetDictionaryElementAsync(int id) {
            var element = await _context.Set<DicitonaryType>().AsNoTracking().FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new ArgumentException("Dictionary element with id: " + id + " does not exist");
            return new DictionaryElementEditDTO() { Id = element.Id, Name = element.Name, Comment = element.Comment, Timestamp = element.Timestamp };

        }
        public async Task<IEnumerable<DictionaryReadDTO>> GetDictionaryAsync() {
            return await _context.Set<DicitonaryType>()
                .Select(c => new DictionaryReadDTO() { Id = c.Id, Name = c.Name })
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
        public async Task AddKey(DictionaryElementCreateDTO element) {
            if (!ValidateName(element.Name))
                throw new InvalidOperationException($"Dictionary element with name: {element.Name} has already exist");
            await _context.Set<DicitonaryType>().AddAsync(new DicitonaryType() { Name = element.Name, Comment = element.Comment });
            await _context.SaveChangesAsync();
        }
        private bool ValidateName(string name, int? id = null) {
            IQueryable<DicitonaryType> elements = _context.Set<DicitonaryType>().AsNoTracking();
            if (id != null)
                elements = elements.Where(d => d.Id != id);
            if (elements.FirstOrDefault(d => d.Name == name) != null)
                return false;
            return true;
        }
        public async Task UpdateKey(DictionaryElementEditDTO element) {
            if (!ValidateName(element.Name, element.Id))
                throw new InvalidOperationException($"Dictionary element with name: {element.Name} has already exist");
            var dictionaryElement = await _context.Set<DicitonaryType>().FirstOrDefaultAsync(c => c.Id == element.Id)
                ?? throw new ArgumentException("Dictionary element with id: " + element.Id + " does not exist");

            dictionaryElement.Update(element.Name, element.Comment);
            _context.Entry(dictionaryElement).Property("Timestamp").OriginalValue = element.Timestamp;
            _context.Update<DicitonaryType>(dictionaryElement);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveKey(int id, byte[] timestamp) {
            var dictionaryElement = await _context.Set<DicitonaryType>().FirstOrDefaultAsync(c => c.Id == id)
                         ?? throw new InvalidOperationException("Element has already been deleted");
            _context.Entry(dictionaryElement).Property("Timestamp").OriginalValue = timestamp;
            _context.Remove<DicitonaryType>(dictionaryElement);
            await _context.SaveChangesAsync();
        }
    }
}
