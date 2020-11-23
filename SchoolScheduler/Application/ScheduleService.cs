using System;
using System.Collections.Generic;
using System.Linq;
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
        public async void CreateActivity(ActivityEditViewModel activity) {
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
                ?? throw new InvalidOperationException(activity.Teacher + " teacher does not exist in database");

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
                _context.Activities.Include(a => a.Teacher).Include(a => a.Room).Include(a => a.ClassGroup),
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
        public async void DeleteActivity(int id) {
            var activity = _context.Activities.FirstOrDefault(a => a.Id == id);
            if (activity is null)
                throw new InvalidOperationException("Activity does not exist");
            _context.Remove(activity);
            await _context.SaveChangesAsync();
        }

        public void EditActivity(int id, ActivityEditViewModel activity) {
            throw new NotImplementedException();
        }

        public ScheduleViewModel GetScheduleByGroup(string classGroup) {
            var activitiesByGroup = _context.Activities
                .Include(a => a.ClassGroup)
                .Include(a => a.Room)
                .Include(a => a.Subject)
                .Where(a => a.ClassGroup.Name == classGroup);
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
            throw new NotImplementedException();
        }

        public ScheduleViewModel GetScheduleByTeacher(string teacher) {
            throw new NotImplementedException();
        }
    }
}
