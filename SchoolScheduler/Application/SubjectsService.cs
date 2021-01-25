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
    public class SubjectsService : GenericDictionaryService<Subject>, ISubjectsService {
        public SubjectsService(ApplicationDbContext context) : base(context) { }

        public IEnumerable<string> GetDictionaryBySlot(int slot, int? id = null) {
            return _context.Subjects.Select(s => s.Name).AsNoTracking().OrderBy(s => s).ToList();    
        }
    }
}