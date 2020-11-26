using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Services;
using Contracts.ViewModels.Dictionaries;
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

        public async Task<DictionaryElementEditViewModel> GetDictionaryElement(int id, DataType type) {
            switch (type) {
                case DataType.ClassGroup:
                    var classGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new ArgumentException("Class group with id: " + id + " does not exist");
                    return new DictionaryElementEditViewModel() { Id = classGroup.Id, Name = classGroup.Name, Comment = classGroup.Comment };
                case DataType.Room:
                    var room = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new ArgumentException("Class group with id: " + id + " does not exist");
                    return new DictionaryElementEditViewModel() { Id = room.Id, Name = room.Name, Comment = room.Comment };
                case DataType.Subject:
                    var subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new ArgumentException("Class group with id: " + id + " does not exist");
                    return new DictionaryElementEditViewModel() { Id = subject.Id, Name = subject.Name, Comment = subject.Comment };
                case DataType.Teacher:
                    var teacher = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new ArgumentException("Class group with id: " + id + " does not exist");
                    return new DictionaryElementEditViewModel() { Id = teacher.Id, Name = teacher.Name, Comment = teacher.Comment };
                default:
                    throw new ArgumentException("Type of dictionary does not exist");
            }
        }

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
        public async Task<IEnumerable<DictionaryIndexViewModel>> GetDictionary(DataType type) {
            switch (type) {
                case DataType.ClassGroup:
                    return await _context.ClassGroups
                        .Select(c => new DictionaryIndexViewModel() { Id = c.Id, Name = c.Name }).ToListAsync();
                case DataType.Room:
                    return await _context.Rooms
                        .Select(r => new DictionaryIndexViewModel() { Id = r.Id, Name = r.Name }).ToListAsync();
                case DataType.Subject:
                    return await _context.Subjects
                        .Select(s => new DictionaryIndexViewModel() { Id = s.Id, Name = s.Name }).ToListAsync();
                case DataType.Teacher:
                    return await _context.Teachers
                        .Select(t => new DictionaryIndexViewModel() { Id = t.Id, Name = t.Name }).ToListAsync();
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