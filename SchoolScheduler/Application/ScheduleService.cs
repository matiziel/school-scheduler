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
        public ActivityEditViewModel GetActivity(int id) {
            var activity = GetActivities().FirstOrDefault(a => a.Id == id);
            return new ActivityEditViewModel() {
                Id = activity.Id,
                Slot = activity.Slot.Id,
                ClassGroup = activity.ClassGroup.Name,
                Room =  activity.Room.Name,
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
        public ScheduleService(ApplicationDbContext context) {
            _context = context;
        }
        public async void CreateActivity(ActivityEditViewModel activity) {
            if (activity is null)
                throw new ArgumentException("Activity does not exists");

            if (!ValidateActivityForCreate(activity))
                throw new InvalidOperationException("One of values on this slot is occupied");


            _context.Activities.Add(new Activity {

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
                a.Slot.Id == activity.Slot && (
                    a.Teacher.Name == activity.Teacher||
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

        public ScheduleViewModel GetScheduleByGroup(string group) {
            throw new NotImplementedException();
        }

        public ScheduleViewModel GetScheduleByRoom(string room) {
            throw new NotImplementedException();
        }

        public ScheduleViewModel GetScheduleByTeacher(string teacher) {
            throw new NotImplementedException();
        }
    }
}
