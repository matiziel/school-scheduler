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
using LanguageExt;
using static LanguageExt.Prelude;
using Contracts.DataTransferObjects;

namespace Application {
    public class ClassGroupsService : GenericDictionaryService<ClassGroup>, IClassGroupsService {
        public ClassGroupsService(ApplicationDbContext context) : base(context) { }
        public Either<ErrorDTO, IEnumerable<string>> GetDictionaryBySlot(int slot, int? id = null) {
            try {
                var groups = _context.ClassGroups.Select(c => c.Name).OrderBy(c => c);
                var occupiedGroups = Helper.GetActivitiesBySlot(_context, slot, id).Select(a => a.ClassGroup.Name);
                return Right(groups.Where(g => !occupiedGroups.Contains(g)).ToList() as IEnumerable<string>);
            }
            catch (Exception) {
                return Left(new ErrorDTO("Error while getting class groups dictionary"));
            }

        }
    }
}