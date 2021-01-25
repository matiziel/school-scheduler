using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Services;
using Contracts.DataTransferObjects.Dictionaries;
using Model;
using Persistence;
using Common;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application {
    public class SubjectsService : ISubjectsService {
        private readonly ApplicationDbContext _context;
        public SubjectsService(ApplicationDbContext context) =>
            _context = context;
        public async Task<DictionaryElementEditDTO> GetDictionaryElementAsync(int id) {
            var subject = await _context.Subjects.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new ArgumentException("Subject with id: " + id + " does not exist");
            return new DictionaryElementEditDTO() { Id = subject.Id, Name = subject.Name, Comment = subject.Comment, Timestamp = subject.Timestamp };

        }
        public async Task<IEnumerable<DictionaryReadDTO>> GetDictionaryAsync() {
            return await _context.Subjects
                        .Select(s => new DictionaryReadDTO() { Id = s.Id, Name = s.Name })
                        .AsNoTracking()
                        .OrderBy(c => c.Name)
                        .ToListAsync();
        }
        public async Task AddKey(DictionaryElementCreateDTO element) {
            if (!ValidateName(element.Name))
                throw new InvalidOperationException($"Subject with name: {element.Name} has already exist");

            await _context.Subjects.AddAsync(new Subject(element.Name, element.Comment));
            await _context.SaveChangesAsync();
        }
        private bool ValidateName(string name, int? id = null) {
            IQueryable<Subject> subjects = _context.Subjects.AsNoTracking();
            if (id != null)
                subjects = subjects.Where(d => d.Id != id);
            if (subjects.FirstOrDefault(d => d.Name == name) != null)
                return false;
            return true;
        }
        public async Task UpdateKey(DictionaryElementEditDTO element) {
            if (!ValidateName(element.Name, element.Id))
                throw new InvalidOperationException($"Subject with name: {element.Name} has already exist");
            var subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Id == element.Id)
                        ?? throw new ArgumentException("Subject with id: " + element.Id + " does not exist");

            subject.Update(element.Name, element.Comment);
            _context.Entry(subject).Property("Timestamp").OriginalValue = element.Timestamp;
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();

        }
        public async Task RemoveKey(int id, byte[] timestamp) {
            var subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Id == id)
                                   ?? throw new InvalidOperationException("Element has already been deleted");
            _context.Entry(subject).Property("Timestamp").OriginalValue = timestamp;
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
        }
        public IEnumerable<string> GetDictionaryBySlot(int slot, int? id = null) {
            return _context.Subjects.Select(s => s.Name).AsNoTracking().OrderBy(s => s).ToList();
            
        }
    }
}