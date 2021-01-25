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
    public class ClassGroupsService : GenericDictionaryService<ClassGroup>, IClassGroupsService {
        public ClassGroupsService(ApplicationDbContext context) : base(context) { }
        public IEnumerable<string> GetDictionaryBySlot(int slot, int? id = null) {
            var groups = _context.ClassGroups.Select(c => c.Name).OrderBy(c => c);
            var occupiedGroups = Helper.GetActivitiesBySlot(_context, slot, id).Select(a => a.ClassGroup.Name);
            return groups.Where(g => !occupiedGroups.Contains(g)).ToList();
        }
    }
}