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
        public IEnumerable<string> GetAllGroups() {
            return _context.Schedule.Groups;
        }
        public IEnumerable<string> GetAllRooms() {
            return _context.Schedule.Rooms;
        }
        public IEnumerable<string> GetAllTeachers() {
            return _context.Schedule.Teachers;
        }
        public IEnumerable<string> GetAllClasses() {
            return _context.Schedule.Classes;
        }

        public IEnumerable<string> GetFreeGroupsBySlot(int slot, int? id = null) {
            var groups = GetAllGroups();
            var occupiedGroups = GetActivitiesBySlot(slot, id).Select(a => a.Group);
            return groups.Where(g => !occupiedGroups.Contains(g));
        }
        public IEnumerable<string> GetFreeRoomsBySlot(int slot, int? id = null) {
            var rooms = GetAllRooms();
            var occupiedRooms = GetActivitiesBySlot(slot, id).Select(a => a.Room);
            return rooms.Where(r => !occupiedRooms.Contains(r));
        }
        public IEnumerable<string> GetFreeTeachersBySlot(int slot, int? id = null) {
            var teachers = GetAllTeachers();
            var occupiedTeachers = GetActivitiesBySlot(slot, id).Select(a => a.Teacher);
            return teachers.Where(t => !occupiedTeachers.Contains(t));
        }
        private IEnumerable<Activity> GetActivitiesBySlot(int slot, int? id) {
            if(id is null)
                return _context.Schedule.Activities.Where(a => a.Slot == slot);
            else
                return _context.Schedule.Activities.Where(a => a.Slot == slot && a.Id != id.Value);
        }
    }
}