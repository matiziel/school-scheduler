using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Services;
using Contracts.ViewModels;
using Model;
using Persistence;


namespace Application {
    public class EditDataService : IEditDataService {
        private readonly DbContext _context;
        public EditDataService(DbContext context) {
            _context = context;
        }
        public List<string> GetAllGroups() {
            return _context.Schedule.Groups;
        }
        public List<string> GetAllRooms() {
            return _context.Schedule.Rooms;
        }
        public List<string> GetAllTeachers() {
            return _context.Schedule.Teachers;
        }
        public List<string> GetAllClasses() {
            return _context.Schedule.Classes;
        }
    }
}