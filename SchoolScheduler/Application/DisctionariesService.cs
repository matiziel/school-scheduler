using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Contracts.Services;
using Contracts.ViewModels;
using Model;
using Persistence;
using Common;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application {
    public class DisctionariesService : IDisctionariesService {
        private readonly ApplicationDbContext _context;
        public DisctionariesService(ApplicationDbContext context) =>
            _context = context;

        public IEnumerable<string> GetAllClassGroups() =>
             _context.ClassGroups.Select(c => c.Name).ToList();
        public IEnumerable<string> GetAllRooms() =>
            _context.Rooms.Select(r => r.Name).ToList();
        public IEnumerable<string> GetAllSubjects() =>
            _context.Subjects.Select(s => s.Name).ToList();
        public IEnumerable<string> GetAllTeachers() =>
            _context.Teachers.Select(t => t.Name).ToList();

        public void AddKey(string value, DataType type) {
            // var dict = GetDictionary(type);
            // if (!dict.Contains(value)) {
            //     dict.Add(value);
            //     _context.SaveChanges();
            // }
        }
        public IEnumerable<string> GetDictionary(DataType type) {
            switch (type) {
                case DataType.ClassGroup:
                    return GetAllClassGroups();
                case DataType.Room:
                    return GetAllRooms();
                case DataType.Subject:
                    return GetAllSubjects();
                case DataType.Teacher:
                    return GetAllTeachers();

                default:
                    throw new ArgumentException("Type of dictionary does not exist");

            }
        }

        public void DeleteKey(string value, DataType type) {
            // var dict = GetDictionary(type);
            // if (dict.Contains(value)) {
            //     dict.Remove(value);
            //     RemoveFromActivities(value, type);
            // }
            // _context.SaveChanges();
        }
        private void RemoveFromActivities(string value, DataType type) {
            // Func<Activity, bool> lambda;

            // if (type == DataType.Group)
            //     lambda = a => a.Group == value;
            // else if (type == DataType.Class)
            //     lambda = a => a.Class == value;
            // else if (type == DataType.Teacher)
            //     lambda = a => a.Teacher == value;
            // else if (type == DataType.Room)
            //     lambda = a => a.Room == value;
            // else
            //     throw new ArgumentException("Type of dictionary does not exist");

            // var activitiesIdToRemove = _context.Schedule.Activities.Where(lambda).Select(a => a.Id);
            // _context.Schedule.Activities.RemoveAll(a => activitiesIdToRemove.Contains(a.Id));

            // _context.SaveChanges();
        }
        public IEnumerable<string> GetFreeClassGroupsBySlot(int slot, int? id = null) {
            var groups = GetAllClassGroups();
            var occupiedGroups = GetActivitiesBySlot(slot, id).Select(a => a.ClassGroup.Name);
            return groups.Where(g => !occupiedGroups.Contains(g));
        }
        public IEnumerable<string> GetFreeRoomsBySlot(int slot, int? id = null) {
            var rooms = GetAllRooms();
            var occupiedRooms = GetActivitiesBySlot(slot, id).Select(a => a.Room.Name);
            return rooms.Where(r => !occupiedRooms.Contains(r));
        }
        public IEnumerable<string> GetFreeTeachersBySlot(int slot, int? id = null) {
            var teachers = GetAllTeachers();
            var occupiedTeachers = GetActivitiesBySlot(slot, id).Select(a => a.Teacher.Name);
            return teachers.Where(t => !occupiedTeachers.Contains(t));
        }
        private IEnumerable<Activity> GetActivitiesBySlot(int slot, int? id) {
            var activities = _context.Activities
                .Include(a => a.Slot)
                .Include(a => a.ClassGroup)
                .Include(a => a.Room)
                .Include(a => a.Teacher);
            if (id is null)
                return activities.Where(a => a.Slot.Index == slot);
            else
                return activities.Where(a => a.Slot.Index == slot && a.Id != id.Value);
        }
    }
}