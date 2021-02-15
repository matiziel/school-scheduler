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
using static LanguageExt.Prelude;
using LanguageExt;
using Contracts.DataTransferObjects;

namespace Application {
    public class RoomsService : GenericDictionaryService<Room>, IRoomsService {

        public RoomsService(ApplicationDbContext context) : base(context) { }

        public Either<ErrorDTO, IEnumerable<string>> GetDictionaryBySlot(int slot, int? id = null) {
            try {
                var rooms = _context.Rooms.Select(r => r.Name).OrderBy(r => r);
                var occupiedRooms = Helper.GetActivitiesBySlot(_context, slot, id).Select(a => a.Room.Name);
                return Right(rooms.Where(r => !occupiedRooms.Contains(r)) as IEnumerable<string>);
            }
            catch(Exception) {
                return Left(new ErrorDTO("Error while getting rooms dictionary"));
            } 
        }
    }
}