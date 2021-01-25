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
    public class RoomsService : GenericDictionaryService<Room>, IRoomsService {

        public RoomsService(ApplicationDbContext context) : base(context) { }

        public IEnumerable<string> GetDictionaryBySlot(int slot, int? id = null) {
            var rooms = _context.Rooms.Select(r => r.Name).OrderBy(r => r);
            var occupiedRooms = Helper.GetActivitiesBySlot(_context, slot, id).Select(a => a.Room.Name);
            return rooms.Where(r => !occupiedRooms.Contains(r)).ToList();
        }
    }
}