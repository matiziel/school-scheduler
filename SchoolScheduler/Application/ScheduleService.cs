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
        public ScheduleService(DbContext context) {
            _context = context;
        }
        public Activity GetActivity(int id) {
            return _context.Schedule.Activities.FirstOrDefault(a => a.Id == id);
        }

        public ScheduleViewModel GetScheduleByGroup(string group) {
            var activitiesByClass = _context.Schedule.Activities.Where(a => a.Group == group);
            var schedule = new ScheduleViewModel();
            foreach (var item in activitiesByClass) {
                if (item.Slot < schedule.Slots.Length) {
                    schedule.Slots[item.Slot].Id = item.Id;
                    schedule.Slots[item.Slot].Title = item.Room + " " + item.Class;
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

        public List<string> GetAllGroups() {
            return _context.Schedule.Groups;
        }

        public List<string> GetAllRooms() {
            return _context.Schedule.Rooms;
        }

        public List<string> GetAllTeachers() {
            return _context.Schedule.Teachers;
        }

        public List<string> GetAllClasses() {
            return _context.Schedule.Classes;
        }

        public void CreateActivity(Activity activity) {
            activity.Id = GetFreeId();
            _context.Schedule.Activities.Add(activity);
            _context.SaveChanges();
        }
        private int GetFreeId() {
            return _context.Schedule.Activities.Select(a => a.Id).Max() + 1;
        }
        public void EditActivity(int id, Activity activity) {
            var temp = _context.Schedule.Activities.FirstOrDefault(a => a.Id == id);
            if (temp is null)
                throw new ArgumentException();
            temp.Class = activity.Class;
            temp.Group = activity.Group;
            temp.Room = activity.Room;
            temp.Teacher = activity.Teacher;
            _context.SaveChanges();
        }
        private bool ValidateActivity(Activity activity) {
            var activities = _context.Schedule.Activities.Where(a => a.Slot == activity.Slot);

            return activities.Where(a =>
                a.Teacher == activity.Teacher ||
                a.Room == activity.Room ||
                a.Group == activity.Group
            ).Count() > 0;
        }
    }
}
