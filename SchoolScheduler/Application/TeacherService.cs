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
    public class TeachersService : GenericDictionaryService<Teacher>, ITeachersService {
        public TeachersService(ApplicationDbContext context) : base(context) { }

        public Either<ErrorDTO, IEnumerable<string>> GetDictionaryBySlot(int slot, int? id = null) {
            try {
                var teachers = _context.Teachers.Select(t => t.Name).OrderBy(t => t);
                var occupiedTeachers = Helper.GetActivitiesBySlot(_context, slot, id).Select(a => a.Teacher.Name);
                return Right(teachers.Where(t => !occupiedTeachers.Contains(t)).ToList() as IEnumerable<string>);
            }
            catch (Exception) {
                return Left(new ErrorDTO("Error while getting teachers dictionary"));
            }
        }
    }
}