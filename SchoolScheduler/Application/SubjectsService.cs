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
    public class SubjectsService : GenericDictionaryService<Subject>, ISubjectsService {
        public SubjectsService(ApplicationDbContext context) : base(context) { }

        public Either<ErrorDTO, IEnumerable<string>> GetDictionaryBySlot(int slot, int? id = null) {
            try {
                return Right(_context.Subjects.Select(s => s.Name).AsNoTracking().OrderBy(s => s).ToList() as IEnumerable<string>);
            }
            catch(Exception) {
                return Left(new ErrorDTO("Error while getting subjects dictionary"));
            }     
        }
    }
}