using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Contracts.ViewModels.Schedule;
using Microsoft.EntityFrameworkCore;
using Model;
using Persistence;


namespace Application {
    public class ScheduleService : IScheduleService {
        private readonly ApplicationDbContext _context;
        private readonly IDisctionariesService _disctionariesService;
        public ScheduleService(ApplicationDbContext context, IDisctionariesService disctionariesService) {
            _context = context;
            _disctionariesService = disctionariesService;
        }
        public async Task<ActivityByGroupEditViewModel> GetActivityByGroupAsync(int id) {
            var activity = await GetActivities().FirstOrDefaultAsync(a => a.Id == id);
            return new ActivityByGroupEditViewModel() {
                Id = activity.Id,
                Slot = activity.Slot.Index,
                ClassGroup = activity.ClassGroup.Name,
                Room = activity.Room.Name,
                Subject = activity.Subject.Name,
                Teacher = activity.Teacher.Name,
                Timestamp = activity.Timestamp,
                ListOfRooms = _disctionariesService.GetFreeRoomsBySlot(activity.Slot.Index, id),
                ListOfClasses = _disctionariesService.GetAllSubjects(),
                ListOfTeachers = _disctionariesService.GetFreeTeachersBySlot(activity.Slot.Index, id),
            };
        }
        public ActivityByGroupEditViewModel GetEmptyActivityByGroup(int slot, string group) {
            return new ActivityByGroupEditViewModel() {
                Slot = slot,
                ClassGroup = group,
                ListOfTeachers = _disctionariesService.GetFreeTeachersBySlot(slot),
                ListOfClasses = _disctionariesService.GetAllSubjects(),
                ListOfRooms = _disctionariesService.GetFreeRoomsBySlot(slot)
            };
        }
        public async Task<ActivityByRoomEditViewModel> GetActivityByRoomAsync(int id) {
            var activity = await GetActivities().FirstOrDefaultAsync(a => a.Id == id);
            return new ActivityByRoomEditViewModel() {
                Id = activity.Id,
                Slot = activity.Slot.Index,
                ClassGroup = activity.ClassGroup.Name,
                Room = activity.Room.Name,
                Subject = activity.Subject.Name,
                Teacher = activity.Teacher.Name,
                Timestamp = activity.Timestamp,
                ListOfGroups = _disctionariesService.GetFreeClassGroupsBySlot(activity.Slot.Index, id),
                ListOfClasses = _disctionariesService.GetAllSubjects(),
                ListOfTeachers = _disctionariesService.GetFreeTeachersBySlot(activity.Slot.Index, id),
            };
        }
        public ActivityByRoomEditViewModel GetEmptyActivityByRoom(int slot, string room) {
            return new ActivityByRoomEditViewModel() {
                Slot = slot,
                Room = room,
                ListOfTeachers = _disctionariesService.GetFreeTeachersBySlot(slot),
                ListOfClasses = _disctionariesService.GetAllSubjects(),
                ListOfGroups = _disctionariesService.GetFreeClassGroupsBySlot(slot)
            };
        }
        public async Task<ActivityByTeacherEditViewModel> GetActivityByTeacherAsync(int id) {
            var activity = await GetActivities().FirstOrDefaultAsync(a => a.Id == id);
            return new ActivityByTeacherEditViewModel() {
                Id = activity.Id,
                Slot = activity.Slot.Index,
                ClassGroup = activity.ClassGroup.Name,
                Room = activity.Room.Name,
                Subject = activity.Subject.Name,
                Teacher = activity.Teacher.Name,
                Timestamp = activity.Timestamp,
                ListOfGroups = _disctionariesService.GetFreeClassGroupsBySlot(activity.Slot.Index, id),
                ListOfClasses = _disctionariesService.GetAllSubjects(),
                ListOfRooms = _disctionariesService.GetFreeRoomsBySlot(activity.Slot.Index, id),
            };
        }
        public ActivityByTeacherEditViewModel GetEmptyActivityByTeacher(int slot, string teacher) {
            return new ActivityByTeacherEditViewModel() {
                Slot = slot,
                Teacher = teacher,
                ListOfRooms = _disctionariesService.GetFreeRoomsBySlot(slot),
                ListOfClasses = _disctionariesService.GetAllSubjects(),
                ListOfGroups = _disctionariesService.GetFreeClassGroupsBySlot(slot)
            };
        }
        private IQueryable<Activity> GetActivities() {
            return _context.Activities
                .Include(a => a.Slot)
                .Include(a => a.Teacher)
                .Include(a => a.Room)
                .Include(a => a.ClassGroup)
                .Include(a => a.Subject);
        }
        public ScheduleViewModel GetScheduleByGroup(string classGroup) {
            var names = _disctionariesService.GetAllClassGroups();
            if (classGroup is null)
                classGroup = names.FirstOrDefault();

            var activitiesByGroup = GetActivities().Where(a => a.ClassGroup.Name == classGroup);
            var schedule = new ScheduleViewModel();
            foreach (var item in activitiesByGroup) {
                if (item.Slot.Id < schedule.Slots.Length) {
                    schedule.Slots[item.Slot.Index].Id = item.Id;
                    schedule.Slots[item.Slot.Index].Title = item.Room.Name + " " + item.Subject.Name;
                }
            }
            schedule.Names = names;
            schedule.Name = classGroup;
            return schedule;
        }

        public ScheduleViewModel GetScheduleByRoom(string room) {
            var names = _disctionariesService.GetAllRooms();
            if (room is null)
                room = names.FirstOrDefault();
            var activitiesByGroup = GetActivities().Where(a => a.Room.Name == room);
            var schedule = new ScheduleViewModel();
            foreach (var item in activitiesByGroup) {
                if (item.Slot.Id < schedule.Slots.Length) {
                    schedule.Slots[item.Slot.Index].Id = item.Id;
                    schedule.Slots[item.Slot.Index].Title = item.ClassGroup.Name;
                }
            }
            schedule.Names = names;
            schedule.Name = room;
            return schedule;
        }

        public ScheduleViewModel GetScheduleByTeacher(string teacher) {
            var names = _disctionariesService.GetAllTeachers();
            if (teacher is null)
                teacher = names.FirstOrDefault();
            var activitiesByGroup = GetActivities().Where(a => a.Teacher.Name == teacher);
            var schedule = new ScheduleViewModel();
            foreach (var item in activitiesByGroup) {
                if (item.Slot.Id < schedule.Slots.Length) {
                    schedule.Slots[item.Slot.Index].Id = item.Id;
                    schedule.Slots[item.Slot.Index].Title = item.Room.Name + " " + item.Subject.Name + " " + item.ClassGroup.Name;
                }
            }
            schedule.Names = names;
            schedule.Name = teacher;
            return schedule;
        }
        public async Task CreateActivityAsync(ActivityViewModel activity) {
            if (activity is null)
                throw new ArgumentException("Activity does not exists");

            if (!ValidateActivityForCreate(activity))
                throw new InvalidOperationException("One of values on this slot is occupied");

            var classGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Name == activity.ClassGroup)
                ?? throw new InvalidOperationException(activity.ClassGroup + " class group does not exist in database");

            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Name == activity.Subject)
                ?? throw new InvalidOperationException(activity.Subject + " subject does not exist in database");

            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Name == activity.Teacher)
                ?? throw new InvalidOperationException(activity.Teacher + " teacher does not exist in database");

            var slot = await _context.Slots.FirstOrDefaultAsync(s => s.Index == activity.Slot)
                ?? throw new InvalidOperationException(activity.Slot + " slot does not exist in database");

            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Name == activity.Room)
                ?? throw new InvalidOperationException(activity.Teacher + " room does not exist in database");

            await _context.Activities.AddAsync(new Activity {
                ClassGroup = classGroup,
                Subject = subject,
                Teacher = teacher,
                Slot = slot,
                Room = room
            });
            await _context.SaveChangesAsync();
        }

        private bool ValidateActivityForCreate(ActivityViewModel activity) {
            return ValidateActivity(
                GetActivities(),
                activity
            );
        }

        private bool ValidateActivity(IEnumerable<Activity> activities, ActivityViewModel activity) {
            return activities.Where(a =>
                a.Slot.Index == activity.Slot && (
                    a.Teacher.Name == activity.Teacher ||
                    a.Room.Name == activity.Room ||
                    a.ClassGroup.Name == activity.ClassGroup
                )
            ).Count() == 0;
        }
        public async Task EditActivityAsync(int id, ActivityViewModel activity) {
            var activityToEdit = GetActivities().FirstOrDefault(a => a.Id == id);
            if (activityToEdit is null)
                throw new ArgumentException("Activity does not exist");

            if (!ValidateActivityForEdit(id, activity))
                throw new InvalidOperationException("One of values on this slot is occupied");

            if (activityToEdit.ClassGroup.Name != activity.ClassGroup)
                activityToEdit.ClassGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Name == activity.ClassGroup)
                    ?? throw new InvalidOperationException(activity.ClassGroup + " class does not exist in database");

            if (activityToEdit.Subject.Name != activity.Subject)
                activityToEdit.Subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Name == activity.Subject)
                    ?? throw new InvalidOperationException(activity.Subject + " subject not exist in database");

            if (activityToEdit.Teacher.Name != activity.Teacher)
                activityToEdit.Teacher = await _context.Teachers.FirstOrDefaultAsync(c => c.Name == activity.Teacher)
                    ?? throw new InvalidOperationException(activity.Teacher + " teacher does not exist in database");

            if (activityToEdit.Slot.Index != activity.Slot)
                activityToEdit.Slot = await _context.Slots.FirstOrDefaultAsync(c => c.Index == activity.Slot)
                    ?? throw new InvalidOperationException(activity.Slot + " slot does not exist in database");

            if (activityToEdit.Room.Name != activity.Room)
                activityToEdit.Room = await _context.Rooms.FirstOrDefaultAsync(c => c.Name == activity.Room)
                    ?? throw new InvalidOperationException(activity.Room + " room does not exist in database");

            _context.Entry(activityToEdit).Property("Timestamp").OriginalValue = activity.Timestamp;
            _context.Activities.Update(activityToEdit);
            await _context.SaveChangesAsync();
        }
        private bool ValidateActivityForEdit(int id, ActivityViewModel activity) =>
            ValidateActivity(
                GetActivities().Where(a => a.Id != id),
                activity
            );
        public async Task DeleteActivityAsync(int id, byte[] timestamp) {
            var activity = _context.Activities.FirstOrDefault(a => a.Id == id);
            if (activity is null)
                throw new ArgumentException("Activity does not exist");
            _context.Remove(activity);
            await _context.SaveChangesAsync();
        }
    }
}
