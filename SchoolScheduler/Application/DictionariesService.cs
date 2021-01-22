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
    public class DictionariesService : IDictionariesService {
        private readonly ApplicationDbContext _context;
        public DictionariesService(ApplicationDbContext context) =>
            _context = context;

        public async Task<DictionaryElementEditDTO> GetDictionaryElementAsync(int id, DataType type) {
            switch (type) {
                case DataType.classGroups:
                    var classGroup = await _context.ClassGroups.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new ArgumentException("Class group with id: " + id + " does not exist");
                    return new DictionaryElementEditDTO() { Id = classGroup.Id, Name = classGroup.Name, Comment = classGroup.Comment, Timestamp = classGroup.Timestamp };
                case DataType.rooms:
                    var room = await _context.Rooms.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id)
                        ?? throw new ArgumentException("Room with id: " + id + " does not exist");
                    return new DictionaryElementEditDTO() { Id = room.Id, Name = room.Name, Comment = room.Comment, Timestamp = room.Timestamp };
                case DataType.subjects:
                    var subject = await _context.Subjects.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new ArgumentException("Class group with id: " + id + " does not exist");
                    return new DictionaryElementEditDTO() { Id = subject.Id, Name = subject.Name, Comment = subject.Comment, Timestamp = subject.Timestamp };
                case DataType.teachers:
                    var teacher = await _context.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id)
                        ?? throw new ArgumentException("Teacher with id: " + id + " does not exist");
                    return new DictionaryElementEditDTO() { Id = teacher.Id, Name = teacher.Name, Comment = teacher.Comment, Timestamp = teacher.Timestamp };
                default:
                    throw new ArgumentException("Type of dictionary does not exist");
            }
        }
        public async Task<IEnumerable<DictionaryReadDTO>> GetDictionaryAsync(DataType type) {
            switch (type) {
                case DataType.classGroups:
                    return await _context.ClassGroups
                        .Select(c => new DictionaryReadDTO() { Id = c.Id, Name = c.Name })
                        .AsNoTracking()
                        .OrderBy(c => c.Name)
                        .ToListAsync();
                case DataType.rooms:
                    return await _context.Rooms
                        .Select(r => new DictionaryReadDTO() { Id = r.Id, Name = r.Name })
                        .AsNoTracking()
                        .OrderBy(c => c.Name)
                        .ToListAsync();
                case DataType.subjects:
                    return await _context.Subjects
                        .Select(s => new DictionaryReadDTO() { Id = s.Id, Name = s.Name })
                        .AsNoTracking()
                        .OrderBy(c => c.Name)
                        .ToListAsync();
                case DataType.teachers:
                    return await _context.Teachers
                        .Select(t => new DictionaryReadDTO() { Id = t.Id, Name = t.Name })
                        .AsNoTracking()
                        .OrderBy(c => c.Name)
                        .ToListAsync();
                default:
                    throw new ArgumentException("Type of dictionary does not exist");
            }
        }

        public async Task AddKey(DictionaryElementCreateDTO element, DataType type) {
            if (!ValidateName(type, element.Name))
                throw new InvalidOperationException($"{type.ToString()} with name: {element.Name} has already exist");
            switch (type) {
                case DataType.classGroups:
                    await _context.ClassGroups.AddAsync(new ClassGroup(element.Name, element.Comment));
                    break;
                case DataType.rooms:
                    await _context.Rooms.AddAsync(new Room(element.Name, element.Comment));
                    break;
                case DataType.subjects:
                    await _context.Subjects.AddAsync(new Subject(element.Name, element.Comment));
                    break;
                case DataType.teachers:
                    await _context.Teachers.AddAsync(new Teacher(element.Name, element.Comment));
                    break;
                default:
                    throw new ArgumentException("Type of dictionary does not exist");
            }
            await _context.SaveChangesAsync();
        }
        private bool ValidateName(DataType type, string name, int? id = null) {
            switch (type) {
                case DataType.classGroups:
                    IQueryable<ClassGroup> classGroups = _context.ClassGroups.AsNoTracking();
                    if (id != null)
                        classGroups = classGroups.Where(d => d.Id != id);
                    if (classGroups.FirstOrDefault(d => d.Name == name) != null)
                        return false;
                    break;
                case DataType.rooms:
                    IQueryable<Room> rooms = _context.Rooms.AsNoTracking();
                    if (id != null)
                        rooms = rooms.Where(d => d.Id != id);
                    if (rooms.FirstOrDefault(r => r.Name == name) != null)
                        return false;
                    break;
                case DataType.subjects:
                    IQueryable<Subject> subjects = _context.Subjects.AsNoTracking();
                    if (id != null)
                        subjects = subjects.Where(d => d.Id != id);
                    if (subjects.FirstOrDefault(d => d.Name == name) != null)
                        return false;
                    break;
                case DataType.teachers:
                    IQueryable<Teacher> teachers = _context.Teachers.AsNoTracking();
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
        public async Task UpdateKey(DictionaryElementEditDTO element, DataType type) {
            if (!ValidateName(type, element.Name, element.Id))
                throw new InvalidOperationException($"{type.ToString()} with name: {element.Name} has already exist");
            switch (type) {
                case DataType.classGroups:
                    var classGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Id == element.Id)
                        ?? throw new ArgumentException("Class group with id: " + element.Id + " does not exist");

                    classGroup.Update(element.Name, element.Comment);
                    _context.Entry(classGroup).Property("Timestamp").OriginalValue = element.Timestamp;
                    _context.ClassGroups.Update(classGroup);
                    break;
                case DataType.rooms:
                    var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == element.Id)
                        ?? throw new ArgumentException("Room with id: " + element.Id + " does not exist");

                    room.Update(element.Name, element.Comment);
                    _context.Entry(room).Property("Timestamp").OriginalValue = element.Timestamp;
                    _context.Rooms.Update(room);
                    break;
                case DataType.subjects:
                    var subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Id == element.Id)
                        ?? throw new ArgumentException("Class group with id: " + element.Id + " does not exist");

                    subject.Update(element.Name, element.Comment);
                    _context.Entry(subject).Property("Timestamp").OriginalValue = element.Timestamp;
                    _context.Subjects.Update(subject);
                    break;
                case DataType.teachers:
                    var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == element.Id)
                        ?? throw new ArgumentException("Teacher with id: " + element.Id + " does not exist");

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
                case DataType.classGroups:
                    var classGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new InvalidOperationException("Element has already been deleted");
                    _context.Entry(classGroup).Property("Timestamp").OriginalValue = timestamp;
                    _context.ClassGroups.Remove(classGroup);
                    break;
                case DataType.rooms:
                    var room = await _context.Rooms.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new InvalidOperationException("Element has already been deleted");
                    _context.Entry(room).Property("Timestamp").OriginalValue = timestamp;
                    _context.Rooms.Remove(room);
                    break;
                case DataType.subjects:
                    var subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Id == id)
                        ?? throw new InvalidOperationException("Element has already been deleted");
                    _context.Entry(subject).Property("Timestamp").OriginalValue = timestamp;
                    _context.Subjects.Remove(subject);
                    break;
                case DataType.teachers:
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
        public IEnumerable<string> GetAllSubjects() =>
            _context.Subjects.Select(s => s.Name).AsNoTracking().OrderBy(s => s).ToList();

        public IEnumerable<string> GetFreeClassGroupsBySlot(int slot, int? id = null) {
            var groups = GetAllClassGroups();
            var occupiedGroups = GetActivitiesBySlot(slot, id).Select(a => a.ClassGroup.Name);
            return groups.Where(g => !occupiedGroups.Contains(g));
        }
        private IEnumerable<string> GetAllClassGroups() =>
            _context.ClassGroups.Select(c => c.Name).OrderBy(c => c).ToList();

        public IEnumerable<string> GetFreeRoomsBySlot(int slot, int? id = null) {
            var rooms = GetAllRooms();
            var occupiedRooms = GetActivitiesBySlot(slot, id).Select(a => a.Room.Name);
            return rooms.Where(r => !occupiedRooms.Contains(r));
        }
        private IEnumerable<string> GetAllRooms() =>
            _context.Rooms.Select(r => r.Name).OrderBy(r => r).ToList();

        public IEnumerable<string> GetFreeTeachersBySlot(int slot, int? id = null) {
            var teachers = GetAllTeachers();
            var occupiedTeachers = GetActivitiesBySlot(slot, id).Select(a => a.Teacher.Name);
            return teachers.Where(t => !occupiedTeachers.Contains(t));
        }
        private IEnumerable<string> GetAllTeachers() =>
            _context.Teachers.Select(t => t.Name).OrderBy(t => t).ToList();

        private IEnumerable<Activity> GetActivitiesBySlot(int slot, int? id) {
            var activities = _context.Activities
                .Include(a => a.Slot)
                .Include(a => a.ClassGroup)
                .Include(a => a.Room)
                .Include(a => a.Teacher)
                .AsNoTracking();
            if (id is null)
                return activities.Where(a => a.Slot.Index == slot);
            else
                return activities.Where(a => a.Slot.Index == slot && a.Id != id.Value);
        }
    }
}