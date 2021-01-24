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
    public class RoomsService : IRoomsService {
        private readonly ApplicationDbContext _context;
        public RoomsService(ApplicationDbContext context) =>
            _context = context;
        public async Task<DictionaryElementEditDTO> GetDictionaryElementAsync(int id) {
            var room = await _context.Rooms.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new ArgumentException("Room with id: " + id + " does not exist");
            return new DictionaryElementEditDTO() { Id = room.Id, Name = room.Name, Comment = room.Comment, Timestamp = room.Timestamp };
        }
        public async Task<IEnumerable<DictionaryReadDTO>> GetDictionaryAsync() {
            return await _context.Rooms
                .Select(r => new DictionaryReadDTO() { Id = r.Id, Name = r.Name })
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
        public async Task AddKey(DictionaryElementCreateDTO element) {
            if (!ValidateName(element.Name))
                throw new InvalidOperationException($"Room with name: {element.Name} has already exist");
            await _context.Rooms.AddAsync(new Room(element.Name, element.Comment));
            await _context.SaveChangesAsync();

        }
        private bool ValidateName(string name, int? id = null) {
            IQueryable<Room> rooms = _context.Rooms.AsNoTracking();
            if (id != null)
                rooms = rooms.Where(d => d.Id != id);
            if (rooms.FirstOrDefault(r => r.Name == name) != null)
                return false;
            return true;
        }
        public async Task UpdateKey(DictionaryElementEditDTO element) {
            if (!ValidateName(element.Name, element.Id))
                throw new InvalidOperationException($"Room with name: {element.Name} has already exist");
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == element.Id)
                ?? throw new ArgumentException("Room with id: " + element.Id + " does not exist");

            room.Update(element.Name, element.Comment);
            _context.Entry(room).Property("Timestamp").OriginalValue = element.Timestamp;
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveKey(int id, byte[] timestamp) {
            var room = await _context.Rooms.FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new InvalidOperationException("Element has already been deleted");
            _context.Entry(room).Property("Timestamp").OriginalValue = timestamp;
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

        }
        public IEnumerable<string> GetDictionaryBySlot(int slot, int? id = null) {
            var rooms = _context.Rooms.Select(r => r.Name).OrderBy(r => r);
            var occupiedRooms = Helper.GetActivitiesBySlot(_context, slot, id).Select(a => a.Room.Name);
            return rooms.Where(r => !occupiedRooms.Contains(r)).ToList();
        }
    }
}