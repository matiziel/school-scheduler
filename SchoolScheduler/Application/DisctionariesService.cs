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

        public async Task<DictionaryElementEditViewModel> GetDictionaryElementAsync(int id, DataType type) {
            switch (type) {
                case DataType.ClassGroup:
                    var classGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new ArgumentException("Class group with id: " + id + " does not exist");
                    return new DictionaryElementEditViewModel() { Id = classGroup.Id, Name = classGroup.Name, Comment = classGroup.Comment };
                case DataType.Room:
                    var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id)
                        ?? throw new ArgumentException("Room with id: " + id + " does not exist");
                    return new DictionaryElementEditViewModel() { Id = room.Id, Name = room.Name, Comment = room.Comment };
                case DataType.Subject:
                    var subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new ArgumentException("Class group with id: " + id + " does not exist");
                    return new DictionaryElementEditViewModel() { Id = subject.Id, Name = subject.Name, Comment = subject.Comment };
                case DataType.Teacher:
                    var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id)
                        ?? throw new ArgumentException("Teacher with id: " + id + " does not exist");
                    return new DictionaryElementEditViewModel() { Id = teacher.Id, Name = teacher.Name, Comment = teacher.Comment };
                default:
                    throw new ArgumentException("Type of dictionary does not exist");
            }
        }
        public async Task<IEnumerable<DictionaryIndexViewModel>> GetDictionaryAsync(DataType type) {
            switch (type) {
                case DataType.ClassGroup:
                    return await _context.ClassGroups
                        .Select(c => new DictionaryIndexViewModel() { Id = c.Id, Name = c.Name })
                        .OrderBy(c => c.Name)
                        .ToListAsync();
                case DataType.Room:
                    return await _context.Rooms
                        .Select(r => new DictionaryIndexViewModel() { Id = r.Id, Name = r.Name })
                        .OrderBy(c => c.Name)
                        .ToListAsync();
                case DataType.Subject:
                    return await _context.Subjects
                        .Select(s => new DictionaryIndexViewModel() { Id = s.Id, Name = s.Name })
                        .OrderBy(c => c.Name)
                        .ToListAsync();
                case DataType.Teacher:
                    return await _context.Teachers
                        .Select(t => new DictionaryIndexViewModel() { Id = t.Id, Name = t.Name })
                        .OrderBy(c => c.Name)
                        .ToListAsync();
                default:
                    throw new ArgumentException("Type of dictionary does not exist");
            }
        }

        public IEnumerable<string> GetAllClassGroups() =>
             _context.ClassGroups.Select(c => c.Name).OrderBy(c => c).ToList();
        public IEnumerable<string> GetAllRooms() =>
            _context.Rooms.Select(r => r.Name).OrderBy(r => r).ToList();
        public IEnumerable<string> GetAllSubjects() =>
            _context.Subjects.Select(s => s.Name).OrderBy(s => s).ToList();
        public IEnumerable<string> GetAllTeachers() =>
            _context.Teachers.Select(t => t.Name).OrderBy(t => t).ToList();

        public async Task AddKey(DictionaryElementEditViewModel element, DataType type) {
            switch (type) {
                case DataType.ClassGroup:
                    await _context.ClassGroups.AddAsync(new ClassGroup(element.Name, element.Comment));
                    break;
                case DataType.Room:
                    await _context.Rooms.AddAsync(new Room(element.Name, element.Comment));
                    break;
                case DataType.Subject:
                    await _context.Subjects.AddAsync(new Subject(element.Name, element.Comment));
                    break;
                case DataType.Teacher:
                    await _context.Teachers.AddAsync(new Teacher(element.Name, element.Comment));
                    break;
                default:
                    throw new ArgumentException("Type of dictionary does not exist");
            }
            await _context.SaveChangesAsync();

        }
        public async Task UpdateKey(DictionaryElementEditViewModel element, DataType type) {
            switch (type) {
                case DataType.ClassGroup:
                    var classGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Id == element.Id.Value)
                        ?? throw new ArgumentException("Class group with id: " + element.Id.Value + " does not exist");
                    classGroup.Update(element.Name, element.Comment);
                    _context.ClassGroups.Update(classGroup);
                    break;
                case DataType.Room:
                    var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == element.Id.Value)
                        ?? throw new ArgumentException("Room with id: " + element.Id.Value + " does not exist");
                    room.Update(element.Name, element.Comment);
                    _context.Rooms.Update(room);
                    break;
                case DataType.Subject:
                    var subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Id == element.Id.Value)
                        ?? throw new ArgumentException("Class group with id: " + element.Id.Value + " does not exist");
                    subject.Update(element.Name, element.Comment);
                    _context.Subjects.Update(subject);
                    break;
                case DataType.Teacher:
                    var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == element.Id.Value)
                        ?? throw new ArgumentException("Teacher with id: " + element.Id.Value + " does not exist");
                    teacher.Update(element.Name, element.Comment);
                    _context.Teachers.Update(teacher);
                    break;
                default:
                    throw new ArgumentException("Type of dictionary does not exist");
            }
            await _context.SaveChangesAsync();
        }
        public async Task RemoveKey(int id, DataType type) {
            RemoveFromActivities(id, type);
            switch (type) {
                case DataType.ClassGroup:
                    var classGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Id == id);
                    _context.ClassGroups.Remove(classGroup);
                    break;
                case DataType.Room:
                    var room = await _context.Rooms.FirstOrDefaultAsync(c => c.Id == id);
                    _context.Rooms.Remove(room);
                    break;
                case DataType.Subject:
                    var subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Id == id);
                    _context.Subjects.Remove(subject);
                    break;
                case DataType.Teacher:
                    var teacher = await _context.Teachers.FirstOrDefaultAsync(c => c.Id == id);
                    _context.Teachers.Remove(teacher);
                    break;
                default:
                    throw new ArgumentException("Type of dictionary does not exist");
            }
            await _context.SaveChangesAsync();
        }
        private void RemoveFromActivities(int id, DataType type) {
            IEnumerable<Activity> activities;
            switch (type) {
                case DataType.ClassGroup:
                    activities = _context.Activities.Include(a => a.ClassGroup).Where(a => a.ClassGroup.Id == id);
                    break;
                case DataType.Room:
                    activities = _context.Activities.Include(a => a.Room).Where(a => a.Room.Id == id);
                    break;
                case DataType.Subject:
                    activities = _context.Activities.Include(a => a.Subject).Where(a => a.Subject.Id == id);
                    break;
                case DataType.Teacher:
                    activities = _context.Activities.Include(a => a.Teacher).Where(a => a.Teacher.Id == id);
                    break;
                default:
                    throw new ArgumentException("Type of dictionary does not exist");
            }
            _context.Activities.RemoveRange(activities);
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