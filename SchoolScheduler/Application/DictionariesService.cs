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
    public class DictionariesService : IDictionariesService {
        private readonly ApplicationDbContext _context;
        public DictionariesService(ApplicationDbContext context) =>
            _context = context;

        public async Task<DictionaryElementEditViewModel> GetDictionaryElementAsync(int id, DataType type) {
            switch (type) {
                case DataType.ClassGroup:
                    var classGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new ArgumentException("Class group with id: " + id + " does not exist");
                    return new DictionaryElementEditViewModel() { Id = classGroup.Id, Name = classGroup.Name, Comment = classGroup.Comment, Timestamp = classGroup.Timestamp };
                case DataType.Room:
                    var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id)
                        ?? throw new ArgumentException("Room with id: " + id + " does not exist");
                    return new DictionaryElementEditViewModel() { Id = room.Id, Name = room.Name, Comment = room.Comment, Timestamp = room.Timestamp };
                case DataType.Subject:
                    var subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new ArgumentException("Class group with id: " + id + " does not exist");
                    return new DictionaryElementEditViewModel() { Id = subject.Id, Name = subject.Name, Comment = subject.Comment, Timestamp = subject.Timestamp };
                case DataType.Teacher:
                    var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id)
                        ?? throw new ArgumentException("Teacher with id: " + id + " does not exist");
                    return new DictionaryElementEditViewModel() { Id = teacher.Id, Name = teacher.Name, Comment = teacher.Comment, Timestamp = teacher.Timestamp };
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
            if (!ValidateName(type, element.Name))
                throw new InvalidOperationException($"{type.ToString()} with name: {element.Name} has already exist");
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
        private bool ValidateName(DataType type, string name, int? id = null) {
            switch (type) {
                case DataType.ClassGroup:
                    IQueryable<ClassGroup> classGroups = _context.ClassGroups;
                    if (id != null)
                        classGroups = classGroups.Where(d => d.Id != id);
                    if (classGroups.FirstOrDefault(d => d.Name == name) != null)
                        return false;
                    break;
                case DataType.Room:
                    IQueryable<Room> rooms = _context.Rooms;
                    if (id != null)
                        rooms = rooms.Where(d => d.Id != id);
                    if (rooms.FirstOrDefault(r => r.Name == name) != null)
                        return false;
                    break;
                case DataType.Subject:
                    IQueryable<Subject> subjects = _context.Subjects;
                    if (id != null)
                        subjects = subjects.Where(d => d.Id != id);
                    if (subjects.FirstOrDefault(d => d.Name == name) != null)
                        return false;
                    break;
                case DataType.Teacher:
                    IQueryable<Teacher> teachers = _context.Teachers;
                    if (id != null)
                        teachers = teachers.Where(d => d.Id != id);
                    if (teachers.FirstOrDefault(d => d.Name == name) != null)
                        return false;
                    break;
                default:
                    return false;
            }
            return true;
        }
        public async Task UpdateKey(DictionaryElementEditViewModel element, DataType type) {
            if(!ValidateName(type, element.Name, element.Id.Value))
                throw new InvalidOperationException($"{type.ToString()} with name: {element.Name} has already exist");
            switch (type) {
                case DataType.ClassGroup:
                    var classGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Id == element.Id.Value)
                        ?? throw new ArgumentException("Class group with id: " + element.Id.Value + " does not exist");

                    classGroup.Update(element.Name, element.Comment);
                    _context.Entry(classGroup).Property("Timestamp").OriginalValue = element.Timestamp;
                    _context.ClassGroups.Update(classGroup);
                    break;
                case DataType.Room:
                    var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == element.Id.Value)
                        ?? throw new ArgumentException("Room with id: " + element.Id.Value + " does not exist");

                    room.Update(element.Name, element.Comment);
                    _context.Entry(room).Property("Timestamp").OriginalValue = element.Timestamp;
                    _context.Rooms.Update(room);
                    break;
                case DataType.Subject:
                    var subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Id == element.Id.Value)
                        ?? throw new ArgumentException("Class group with id: " + element.Id.Value + " does not exist");

                    subject.Update(element.Name, element.Comment);
                    _context.Entry(subject).Property("Timestamp").OriginalValue = element.Timestamp;
                    _context.Subjects.Update(subject);
                    break;
                case DataType.Teacher:
                    var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == element.Id.Value)
                        ?? throw new ArgumentException("Teacher with id: " + element.Id.Value + " does not exist");

                    teacher.Update(element.Name, element.Comment);
                    _context.Entry(teacher).Property("Timestamp").OriginalValue = element.Timestamp;
                    _context.Teachers.Update(teacher);
                    break;
                default:
                    throw new ArgumentException("Type of dictionary does not exist");
            }
            await _context.SaveChangesAsync();
        }
        public async Task RemoveKey(int id, byte[] timestamp, DataType type) {
            switch (type) {
                case DataType.ClassGroup:
                    var classGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new InvalidOperationException("Element has already been deleted");
                    _context.Entry(classGroup).Property("Timestamp").OriginalValue = timestamp;
                    _context.ClassGroups.Remove(classGroup);
                    break;
                case DataType.Room:
                    var room = await _context.Rooms.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new InvalidOperationException("Element has already been deleted");
                    _context.Entry(room).Property("Timestamp").OriginalValue = timestamp;
                    _context.Rooms.Remove(room);
                    break;
                case DataType.Subject:
                    var subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new InvalidOperationException("Element has already been deleted");
                    _context.Entry(subject).Property("Timestamp").OriginalValue = timestamp;
                    _context.Subjects.Remove(subject);
                    break;
                case DataType.Teacher:
                    var teacher = await _context.Teachers.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new InvalidOperationException("Element has already been deleted");
                    _context.Entry(teacher).Property("Timestamp").OriginalValue = timestamp;
                    _context.Teachers.Remove(teacher);
                    break;
                default:
                    throw new ArgumentException("Type of dictionary does not exist");
            }
            await _context.SaveChangesAsync();
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