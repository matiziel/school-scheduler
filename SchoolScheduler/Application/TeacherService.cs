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
    public class TeachersService : ITeachersService {

        private readonly ApplicationDbContext _context;
        public TeachersService(ApplicationDbContext context) =>
            _context = context;
        public async Task<DictionaryElementEditDTO> GetDictionaryElementAsync(int id) {
            var teacher = await _context.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id)
                        ?? throw new ArgumentException("Teacher with id: " + id + " does not exist");
            return new DictionaryElementEditDTO() { Id = teacher.Id, Name = teacher.Name, Comment = teacher.Comment, Timestamp = teacher.Timestamp };
        }
        public async Task<IEnumerable<DictionaryReadDTO>> GetDictionaryAsync() {
            return await _context.Teachers
                .Select(t => new DictionaryReadDTO() { Id = t.Id, Name = t.Name })
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
        public async Task AddKey(DictionaryElementCreateDTO element) {
            if (!ValidateName(element.Name))
                throw new InvalidOperationException($"Teacher with name: {element.Name} has already exist");
            await _context.Teachers.AddAsync(new Teacher(element.Name, element.Comment));
            await _context.SaveChangesAsync();
        }
        private bool ValidateName(string name, int? id = null) {
            IQueryable<Teacher> teachers = _context.Teachers.AsNoTracking();
            if (id != null)
                teachers = teachers.Where(d => d.Id != id);
            if (teachers.FirstOrDefault(d => d.Name == name) != null)
                return false;
            return true;
        }
        public async Task UpdateKey(DictionaryElementEditDTO element) {
            if (!ValidateName(element.Name, element.Id))
                throw new InvalidOperationException($"Teacher with name: {element.Name} has already exist");

            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == element.Id)
                ?? throw new ArgumentException("Teacher with id: " + element.Id + " does not exist");

            teacher.Update(element.Name, element.Comment);
            _context.Entry(teacher).Property("Timestamp").OriginalValue = element.Timestamp;
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveKey(int id, byte[] timestamp) {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new InvalidOperationException("Element has already been deleted");
            _context.Entry(teacher).Property("Timestamp").OriginalValue = timestamp;
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
        }
        public IEnumerable<string> GetDictionaryBySlot(int slot, int? id = null) {
            var teachers = _context.Teachers.Select(t => t.Name).OrderBy(t => t);
            var occupiedTeachers = Helper.GetActivitiesBySlot(_context, slot, id).Select(a => a.Teacher.Name);
            return teachers.Where(t => !occupiedTeachers.Contains(t)).ToList();
        }


    }
}