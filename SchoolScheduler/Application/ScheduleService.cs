using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Services;
using Contracts.ViewModels;
using Model;
using Persistence;

namespace Application {
    public class ScheduleService : IScheduleService {
        private readonly DbContext _context;
        public ScheduleService(DbContext context) => _context = context;
        
        public Activity GetActivity(int id) => _context.Schedule.Activities.FirstOrDefault(a => a.Id == id);

        public ScheduleViewModel GetScheduleByGroup(string group) {
            var activitiesByGroup = _context.Schedule.Activities.Where(a => a.Group == group);
            var schedule = new ScheduleViewModel();
            foreach (var item in activitiesByGroup) {
                if (item.Slot < schedule.Slots.Length) {
                    schedule.Slots[item.Slot].Id = item.Id;
                    schedule.Slots[item.Slot].Title = item.Room + " " + item.Class;
                }
            }
            return schedule;
        }

        public ScheduleViewModel GetScheduleByRoom(string room) {
            var activitiesByRoom = _context.Schedule.Activities.Where(a => a.Room == room);
            var schedule = new ScheduleViewModel();
            foreach (var item in activitiesByRoom) {
                if (item.Slot < schedule.Slots.Length) {
                    schedule.Slots[item.Slot].Id = item.Id;
                    schedule.Slots[item.Slot].Title = item.Group;
                }
            }
            return schedule;
        }

        public ScheduleViewModel GetScheduleByTeacher(string teacher) {
            var activitiesByTeacher = _context.Schedule.Activities.Where(a => a.Teacher == teacher);
            var schedule = new ScheduleViewModel();
            foreach (var item in activitiesByTeacher) {
                if (item.Slot < schedule.Slots.Length) {
                    schedule.Slots[item.Slot].Id = item.Id;
                    schedule.Slots[item.Slot].Title = item.Room + " " + item.Class + " " + item.Group;
                }
            }
            return schedule;
        }

        public void CreateActivity(Activity activity) {
            if (activity is null)
                throw new ArgumentException();

            if (!ValidateActivityForCreate(activity))
                throw new InvalidOperationException();

            activity.Id = GetFreeId();
            _context.Schedule.Activities.Add(activity);
            _context.SaveChanges();
        }
        private bool ValidateActivityForCreate(Activity activity) => 
            ValidateActivity(_context.Schedule.Activities, activity);
        private int GetFreeId() => _context.Schedule.Activities.Select(a => a.Id).Max() + 1;

        public void EditActivity(int id, Activity activity) {
            var temp = _context.Schedule.Activities.FirstOrDefault(a => a.Id == id);
            if (temp is null || activity is null)
                throw new ArgumentException();

            if (!ValidateActivityForEdit(id, activity))
                throw new InvalidOperationException();

            temp.Class = activity.Class;
            temp.Group = activity.Group;
            temp.Room = activity.Room;
            temp.Teacher = activity.Teacher;
            _context.SaveChanges();
        }
        private bool ValidateActivityForEdit(int id, Activity activity) =>
            ValidateActivity(_context.Schedule.Activities.Where(a => a.Id != id), activity);

        private bool ValidateActivity(IEnumerable<Activity> activities, Activity activity) {
            return activities.Where(a =>
                a.Slot == activity.Slot && (
                    a.Teacher == activity.Teacher ||
                    a.Room == activity.Room ||
                    a.Group == activity.Group
                )
            ).Count() == 0;
        }

        public void DeleteActivity(int id) {
            var activity = _context.Schedule.Activities.FirstOrDefault(a => a.Id == id);
            if (activity is null)
                throw new InvalidOperationException();
            _context.Schedule.Activities.Remove(activity);
            _context.SaveChanges();
        }
    }
}
