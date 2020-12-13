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
        private readonly IDictionariesService _disctionariesService;
        public ScheduleService(ApplicationDbContext context, IDictionariesService disctionariesService) {
            _context = context;
            _disctionariesService = disctionariesService;
        }
        public async Task<ActivityByGroupEditViewModel> GetActivityByGroupAsync(int id) {
            var activity = await GetActivities().FirstOrDefaultAsync(a => a.Id == id);
            if(activity is null)
                throw new ArgumentException("Activity does not exist");
            return new ActivityByGroupEditViewModel() {
                Id = activity.Id,
                Slot = activity.Slot.Index,
                ClassGroup = activity.ClassGroup.Name,
                Room = activity.Room.Name,
                Subject = activity.Subject.Name,
                Teacher = activity.Teacher.Name,
                Timestamp = activity.Timestamp,
                ArgumentHelper = activity.ClassGroup.Name,
                ListOfRooms = _disctionariesService.GetFreeRoomsBySlot(activity.Slot.Index, id),
                ListOfClasses = _disctionariesService.GetAllSubjects(),
                ListOfTeachers = _disctionariesService.GetFreeTeachersBySlot(activity.Slot.Index, id),
            };
        }
        public ActivityByGroupEditViewModel GetEmptyActivityByGroup(int slot, string group) {
            return new ActivityByGroupEditViewModel() {
                Slot = slot,
                ClassGroup = group,
                ArgumentHelper = group,
                ListOfTeachers = _disctionariesService.GetFreeTeachersBySlot(slot),
                ListOfClasses = _disctionariesService.GetAllSubjects(),
                ListOfRooms = _disctionariesService.GetFreeRoomsBySlot(slot)
            };
        }
        public async Task<ActivityByRoomEditViewModel> GetActivityByRoomAsync(int id) {
            var activity = await GetActivities().FirstOrDefaultAsync(a => a.Id == id);
            if (activity is null)
                throw new ArgumentException("Activity does not exist");
            return new ActivityByRoomEditViewModel() {
                Id = activity.Id,
                Slot = activity.Slot.Index,
                ClassGroup = activity.ClassGroup.Name,
                Room = activity.Room.Name,
                Subject = activity.Subject.Name,
                Teacher = activity.Teacher.Name,
                Timestamp = activity.Timestamp,
                ArgumentHelper = activity.Room.Name,
                ListOfGroups = _disctionariesService.GetFreeClassGroupsBySlot(activity.Slot.Index, id),
                ListOfClasses = _disctionariesService.GetAllSubjects(),
                ListOfTeachers = _disctionariesService.GetFreeTeachersBySlot(activity.Slot.Index, id),
            };
        }
        public ActivityByRoomEditViewModel GetEmptyActivityByRoom(int slot, string room) {
            return new ActivityByRoomEditViewModel() {
                Slot = slot,
                Room = room,
                ArgumentHelper = room,
                ListOfTeachers = _disctionariesService.GetFreeTeachersBySlot(slot),
                ListOfClasses = _disctionariesService.GetAllSubjects(),
                ListOfGroups = _disctionariesService.GetFreeClassGroupsBySlot(slot)
            };
        }
        public async Task<ActivityByTeacherEditViewModel> GetActivityByTeacherAsync(int id) {
            var activity = await GetActivities().FirstOrDefaultAsync(a => a.Id == id);
            if (activity is null)
                throw new ArgumentException("Activity does not exist");
            return new ActivityByTeacherEditViewModel() {
                Id = activity.Id,
                Slot = activity.Slot.Index,
                ClassGroup = activity.ClassGroup.Name,
                Room = activity.Room.Name,
                Subject = activity.Subject.Name,
                Teacher = activity.Teacher.Name,
                Timestamp = activity.Timestamp,
                ArgumentHelper = activity.Teacher.Name,
                ListOfGroups = _disctionariesService.GetFreeClassGroupsBySlot(activity.Slot.Index, id),
                ListOfClasses = _disctionariesService.GetAllSubjects(),
                ListOfRooms = _disctionariesService.GetFreeRoomsBySlot(activity.Slot.Index, id),
            };
        }
        public ActivityByTeacherEditViewModel GetEmptyActivityByTeacher(int slot, string teacher) {
            return new ActivityByTeacherEditViewModel() {
                Slot = slot,
                Teacher = teacher,
                ArgumentHelper = teacher,
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
                    schedule.Slots[item.Slot.Index].Title = item.GetTitleForGroups();
                }
            }
            schedule.Names = names;
            schedule.Name = classGroup;
            schedule.Type = "Group";
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
                    schedule.Slots[item.Slot.Index].Title = item.GetTitleForRooms();
                }
            }
            schedule.Names = names;
            schedule.Name = room;
            schedule.Type = "Room";
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
                    schedule.Slots[item.Slot.Index].Title = item.GetTitleForTeachers();
                }
            }
            schedule.Names = names;
            schedule.Name = teacher;
            schedule.Type = "Teacher";
            return schedule;
        }
        public async Task CreateActivityAsync(ActivityViewModel activity) {
            if (activity is null)
                throw new ArgumentException("Activity does not exists");

            if (!ValidateActivityForCreate(activity))
                throw new InvalidOperationException("One of values on this slot is occupied");

            var classGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Name == activity.ClassGroup);
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Name == activity.Subject);
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Name == activity.Teacher);
            var slot = await _context.Slots.FirstOrDefaultAsync(s => s.Index == activity.Slot);
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Name == activity.Room);

            await _context.Activities.AddAsync(new Activity(room, classGroup, subject, slot, teacher));
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
            if (activity is null)
                throw new ArgumentException("Activity does not exist");

            var activityToEdit = GetActivities().FirstOrDefault(a => a.Id == id);
            if (activityToEdit is null)
                throw new ArgumentException("Activity does not exist");

            //TODO think about throwing exception while slot in viewmodel is different from value from db 
            if (!ValidateActivityForEdit(id, activity))
                throw new InvalidOperationException("One of values on this slot is occupied");

            if (activityToEdit.ClassGroup.Name != activity.ClassGroup)
                activityToEdit.ClassGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Name == activity.ClassGroup)
                    ?? throw new ArgumentException(activity.ClassGroup + " class does not exist in database");

            if (activityToEdit.Subject.Name != activity.Subject)
                activityToEdit.Subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Name == activity.Subject)
                    ?? throw new ArgumentException(activity.Subject + " subject not exist in database");

            if (activityToEdit.Teacher.Name != activity.Teacher)
                activityToEdit.Teacher = await _context.Teachers.FirstOrDefaultAsync(c => c.Name == activity.Teacher)
                    ?? throw new ArgumentException(activity.Teacher + " teacher does not exist in database");

            if (activityToEdit.Room.Name != activity.Room)
                activityToEdit.Room = await _context.Rooms.FirstOrDefaultAsync(c => c.Name == activity.Room)
                    ?? throw new ArgumentException(activity.Room + " room does not exist in database");

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
            _context.Entry(activity).Property("Timestamp").OriginalValue = timestamp;
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
        }
    }
}
