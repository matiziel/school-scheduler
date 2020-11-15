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
        public EditDataService(DbContext context) => _context = context;

        public enum DataType { Group, Class, Teacher, Room }
        public IEnumerable<string> GetAllGroups() => _context.Schedule.Groups;
        public IEnumerable<string> GetAllRooms() => _context.Schedule.Rooms;
        public IEnumerable<string> GetAllTeachers() => _context.Schedule.Teachers;
        public IEnumerable<string> GetAllClasses() => _context.Schedule.Classes;

        public void AddKey(string value, DataType type) {
            if (type == DataType.Group) {
                if (!_context.Schedule.Groups.Contains(value))
                    _context.Schedule.Groups.Add(value);
            }
            else if (type == DataType.Class) {
                if (!_context.Schedule.Classes.Contains(value))
                    _context.Schedule.Classes.Add(value);
            }
            else if (type == DataType.Teacher) {
                if (!_context.Schedule.Teachers.Contains(value))
                    _context.Schedule.Teachers.Add(value);
            }
            else if (type == DataType.Room) {
                if (!_context.Schedule.Rooms.Contains(value))
                    _context.Schedule.Rooms.Add(value);
            }
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
            if (id is null)
                return _context.Schedule.Activities.Where(a => a.Slot == slot);
            else
                return _context.Schedule.Activities.Where(a => a.Slot == slot && a.Id != id.Value);
        }
    }
}