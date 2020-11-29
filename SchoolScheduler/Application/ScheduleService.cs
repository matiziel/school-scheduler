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
        public ScheduleService(ApplicationDbContext context) {
            _context = context;
        }
        public async Task<ActivityEditViewModel> GetActivityAsync(int id) {
            var activity = await GetActivities().FirstOrDefaultAsync(a => a.Id == id);
            return new ActivityEditViewModel() {
                Id = activity.Id,
                Slot = activity.Slot.Index,
                ClassGroup = activity.ClassGroup.Name,
                Room = activity.Room.Name,
                Subject = activity.Subject.Name,
                Teacher = activity.Teacher.Name
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
            var activitiesByGroup = GetActivities().Where(a => a.ClassGroup.Name == classGroup);
            var schedule = new ScheduleViewModel();
            foreach (var item in activitiesByGroup) {
                if (item.Slot.Id < schedule.Slots.Length) {
                    schedule.Slots[item.Slot.Index].Id = item.Id;
                    schedule.Slots[item.Slot.Index].Title = item.Room.Name + " " + item.Subject.Name;
                }
            }
            return schedule;
        }

        public ScheduleViewModel GetScheduleByRoom(string room) {
            var activitiesByGroup = GetActivities().Where(a => a.Room.Name == room);
            var schedule = new ScheduleViewModel();
            foreach (var item in activitiesByGroup) {
                if (item.Slot.Id < schedule.Slots.Length) {
                    schedule.Slots[item.Slot.Index].Id = item.Id;
                    schedule.Slots[item.Slot.Index].Title = item.ClassGroup.Name;
                }
            }
            return schedule;
        }

        public ScheduleViewModel GetScheduleByTeacher(string teacher) {
            var activitiesByGroup = GetActivities().Where(a => a.Teacher.Name == teacher);
            var schedule = new ScheduleViewModel();
            foreach (var item in activitiesByGroup) {
                if (item.Slot.Id < schedule.Slots.Length) {
                    schedule.Slots[item.Slot.Index].Id = item.Id;
                    schedule.Slots[item.Slot.Index].Title = item.Room.Name + " " + item.Subject.Name + " " + item.ClassGroup.Name;
                }
            }
            return schedule;
        }
        public async Task CreateActivityAsync(ActivityEditViewModel activity) {
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

        private bool ValidateActivityForCreate(ActivityEditViewModel activity) {
            return ValidateActivity(
                GetActivities(),
                activity
            );
        }

        private bool ValidateActivity(IEnumerable<Activity> activities, ActivityEditViewModel activity) {
            return activities.Where(a =>
                a.Slot.Index == activity.Slot && (
                    a.Teacher.Name == activity.Teacher ||
                    a.Room.Name == activity.Room ||
                    a.ClassGroup.Name == activity.ClassGroup
                )
            ).Count() == 0;
        }
        public async Task EditActivityAsync(int id, ActivityEditViewModel activity) {
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

            _context.Activities.Update(activityToEdit);
            await _context.SaveChangesAsync();
        }
        private bool ValidateActivityForEdit(int id, ActivityEditViewModel activity) =>
            ValidateActivity(
                GetActivities().Where(a => a.Id != id),
                activity
            );
        public async Task DeleteActivityAsync(int id) {
            var activity = _context.Activities.FirstOrDefault(a => a.Id == id);
            if (activity is null)
                throw new ArgumentException("Activity does not exist");
            _context.Remove(activity);
            await _context.SaveChangesAsync();
        }
    }
}
