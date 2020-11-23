using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Contracts.Services;
using Contracts.ViewModels;
using Model;
using Persistence;
using Common;


namespace Application {
    public class DisctionariesService : IDisctionariesService {
        private readonly ApplicationDbContext _context;
        public DisctionariesService(ApplicationDbContext context) {
            _context = context;
        }
        public void AddKey(string value, DataType type) {
            throw new NotImplementedException();
        }


        public void DeleteKey(string value, DataType type) {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetAllSubjects() =>
            _context.Subjects.Select(c => c.Name);

        public IEnumerable<string> GetAllClassGroups() =>
            _context.ClassGroups.Select(c => c.Name);

        public IEnumerable<string> GetAllRooms() =>
            _context.Rooms.Select(r => r.Name);

        public IEnumerable<string> GetAllTeachers() =>
            _context.Teachers.Select(t => t.Name);

        public List<string> GetDictionary(DataType type) {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFreeGroupsBySlot(int slot, int? id = null) {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFreeRoomsBySlot(int slot, int? id = null) {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFreeTeachersBySlot(int slot, int? id = null) {
            throw new NotImplementedException();
        }

    }
}