using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Contracts.ViewModels;
using Microsoft.EntityFrameworkCore;
using Model;
using Persistence;

namespace Application {
    public class ScheduleService : IScheduleService {
        private readonly ApplicationDbContext _context;
        public ScheduleService(ApplicationDbContext context) {
            _context = context;
        }
        public ActivityEditViewModel GetActivity(int id) {
            var activity = GetActivities().FirstOrDefault(a => a.Id == id);
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
        public async Task CreateActivity(ActivityEditViewModel activity) {
            if (activity is null)
                throw new ArgumentException("Activity does not exists");

            if (!ValidateActivityForCreate(activity))
                throw new InvalidOperationException("One of values on this slot is occupied");

            ClassGroup classGroup = _context.ClassGroups.FirstOrDefault(c => c.Name == activity.ClassGroup)
                ?? throw new InvalidOperationException(activity.ClassGroup + " class group does not exist in database");

            Subject subject = _context.Subjects.FirstOrDefault(s => s.Name == activity.Subject)
                ?? throw new InvalidOperationException(activity.Subject + " subject does not exist in database");

            Teacher teacher = _context.Teachers.FirstOrDefault(t => t.Name == activity.Teacher)
                ?? throw new InvalidOperationException(activity.Teacher + " teacher does not exist in database");

            Slot slot = _context.Slots.FirstOrDefault(s => s.Index == activity.Slot)
                ?? throw new InvalidOperationException(activity.Slot + " slot does not exist in database");

            Room room = _context.Rooms.FirstOrDefault(r => r.Name == activity.Room)
                ?? throw new InvalidOperationException(activity.Teacher + " room does not exist in database");

            _context.Activities.Add(new Activity {
                ClassGroup = classGroup,
                Subject = subject,
                Teacher = teacher,
                Slot = slot,
                Room = room
            });
            await _context.SaveChangesAsync();
        }

        private bool ValidateActivityForCreate(ActivityEditViewModel activity) =>
            ValidateActivity(
                GetActivities(),
                activity
            );
        private bool ValidateActivity(IEnumerable<Activity> activities, ActivityEditViewModel activity) {
            return activities.Where(a =>
                a.Slot.Index == activity.Slot && (
                    a.Teacher.Name == activity.Teacher ||
                    a.Room.Name == activity.Room ||
                    a.ClassGroup.Name == activity.ClassGroup
                )
            ).Count() == 0;
        }
        public async Task EditActivity(int id, ActivityEditViewModel activity) {
            var activityToEdit = GetActivities().FirstOrDefault(a => a.Id == id);
            if (activityToEdit is null)
                throw new ArgumentException("Activity does not exist");

            if (!ValidateActivityForEdit(id, activity))
                throw new InvalidOperationException("One of values on this slot is occupied");

            if (activityToEdit.ClassGroup.Name != activity.ClassGroup)
                activityToEdit.ClassGroup = _context.ClassGroups.FirstOrDefault(c => c.Name == activity.ClassGroup)
                    ?? throw new InvalidOperationException(activity.ClassGroup + " class does not exist in database");

            if (activityToEdit.Subject.Name != activity.Subject)
                activityToEdit.Subject = _context.Subjects.FirstOrDefault(c => c.Name == activity.Subject)
                    ?? throw new InvalidOperationException(activity.Subject + " subject not exist in database");

            if (activityToEdit.Teacher.Name != activity.Teacher)
                activityToEdit.Teacher = _context.Teachers.FirstOrDefault(c => c.Name == activity.Teacher)
                    ?? throw new InvalidOperationException(activity.Teacher + " teacher does not exist in database");

            if (activityToEdit.Slot.Index != activity.Slot)
                activityToEdit.Slot = _context.Slots.FirstOrDefault(c => c.Index == activity.Slot)
                    ?? throw new InvalidOperationException(activity.Slot + " slot does not exist in database");

            if (activityToEdit.Room.Name != activity.Room)
                activityToEdit.Room = _context.Rooms.FirstOrDefault(c => c.Name == activity.Room)
                    ?? throw new InvalidOperationException(activity.Room + " room does not exist in database");

            _context.Activities.Update(activityToEdit);
            await _context.SaveChangesAsync();
        }
        private bool ValidateActivityForEdit(int id, ActivityEditViewModel activity) =>
            ValidateActivity(
                GetActivities().Where(a => a.Id != id),
                activity
            );
        public async Task DeleteActivity(int id) {
            var activity = _context.Activities.FirstOrDefault(a => a.Id == id);
            if (activity is null)
                throw new ArgumentException("Activity does not exist");
            _context.Remove(activity);
            await _context.SaveChangesAsync();
        }
    }
}
