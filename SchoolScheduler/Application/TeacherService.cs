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
    public class TeachersService : GenericDictionaryService<Teacher>, ITeachersService {
        public TeachersService(ApplicationDbContext context) : base(context) { }

        public IEnumerable<string> GetDictionaryBySlot(int slot, int? id = null) {
            var teachers = _context.Teachers.Select(t => t.Name).OrderBy(t => t);
            var occupiedTeachers = Helper.GetActivitiesBySlot(_context, slot, id).Select(a => a.Teacher.Name);
            return teachers.Where(t => !occupiedTeachers.Contains(t)).ToList();
        }
    }
}