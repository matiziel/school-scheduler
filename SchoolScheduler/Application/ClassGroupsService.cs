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
    public class ClassGroupsService : IClassGroupsService {
        private readonly ApplicationDbContext _context;
        public ClassGroupsService(ApplicationDbContext context) =>
            _context = context;
        public async Task<DictionaryElementEditDTO> GetDictionaryElementAsync(int id) {
            var classGroup = await _context.ClassGroups.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new ArgumentException("Class group with id: " + id + " does not exist");
            return new DictionaryElementEditDTO() { Id = classGroup.Id, Name = classGroup.Name, Comment = classGroup.Comment, Timestamp = classGroup.Timestamp };

        }
        public async Task<IEnumerable<DictionaryReadDTO>> GetDictionaryAsync() {
            return await _context.ClassGroups
                .Select(c => new DictionaryReadDTO() { Id = c.Id, Name = c.Name })
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
        public async Task AddKey(DictionaryElementCreateDTO element) {
            if (!ValidateName(element.Name))
                throw new InvalidOperationException($"Class group with name: {element.Name} has already exist");
            await _context.ClassGroups.AddAsync(new ClassGroup(element.Name, element.Comment));
            await _context.SaveChangesAsync();
        }
        private bool ValidateName(string name, int? id = null) {
            IQueryable<ClassGroup> classGroups = _context.ClassGroups.AsNoTracking();
            if (id != null)
                classGroups = classGroups.Where(d => d.Id != id);
            if (classGroups.FirstOrDefault(d => d.Name == name) != null)
                return false;
            return true;
        }
        public async Task UpdateKey(DictionaryElementEditDTO element) {
            if (!ValidateName(element.Name, element.Id))
                throw new InvalidOperationException($"Class group with name: {element.Name} has already exist");
            var classGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Id == element.Id)
                       ?? throw new ArgumentException("Class group with id: " + element.Id + " does not exist");

            classGroup.Update(element.Name, element.Comment);
            _context.Entry(classGroup).Property("Timestamp").OriginalValue = element.Timestamp;
            _context.ClassGroups.Update(classGroup);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveKey(int id, byte[] timestamp) {
            var classGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Id == id)
                         ?? throw new InvalidOperationException("Element has already been deleted");
            _context.Entry(classGroup).Property("Timestamp").OriginalValue = timestamp;
            _context.ClassGroups.Remove(classGroup);
            await _context.SaveChangesAsync();
        }
        public IEnumerable<string> GetDictionaryBySlot(int slot, int? id = null) {
            var groups = _context.ClassGroups.Select(c => c.Name).OrderBy(c => c);
            var occupiedGroups = Helper.GetActivitiesBySlot(_context, slot, id).Select(a => a.ClassGroup.Name);
            return groups.Where(g => !occupiedGroups.Contains(g)).ToList();

        }
    }
}