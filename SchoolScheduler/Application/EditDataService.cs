using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Contracts.Services;
using Contracts.ViewModels;
using Model;
using Persistence;


namespace Application {
    public class EditDataService : IEditDataService {

        private readonly DbContext _context;
        public EditDataService(DbContext context) => _context = context;

        public IEnumerable<string> GetAllGroups() => _context.Schedule.Groups;
        public IEnumerable<string> GetAllRooms() => _context.Schedule.Rooms;
        public IEnumerable<string> GetAllTeachers() => _context.Schedule.Teachers;
        public IEnumerable<string> GetAllClasses() => _context.Schedule.Classes;

        public void AddKey(string value, DataType type) {
            var dict = GetDictionary(type);
            if (!dict.Contains(value)) {
                dict.Add(value);
                _context.SaveChanges();
            }
        }
        public List<string> GetDictionary(DataType type) {
            if (type == DataType.Group)
                return _context.Schedule.Groups;
            else if (type == DataType.Class)
                return _context.Schedule.Classes;
            else if (type == DataType.Teacher)
                return _context.Schedule.Teachers;
            else if (type == DataType.Room)
                return _context.Schedule.Rooms;

            throw new ArgumentException("Type of dictionary does not exist");
        }
        public void DeleteKey(string value, DataType type) {
            var dict = GetDictionary(type);
            if (dict.Contains(value)) {
                dict.Remove(value);
                RemoveFromActivities(value, type);
            }
            _context.SaveChanges();
        }
        private void RemoveFromActivities(string value, DataType type) {
            Func<Activity, bool> lambda;

            if (type == DataType.Group)
                lambda = a => a.Group == value;
            else if (type == DataType.Class)
                lambda = a => a.Class == value;
            else if (type == DataType.Teacher)
                lambda = a => a.Teacher == value;
            else if (type == DataType.Room)
                lambda = a => a.Room == value;
            else
                throw new ArgumentException("Type of dictionary does not exist");

            var activitiesIdToRemove = _context.Schedule.Activities.Where(lambda).Select(a => a.Id);
            _context.Schedule.Activities.RemoveAll(a => activitiesIdToRemove.Contains(a.Id));

            _context.SaveChanges();
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