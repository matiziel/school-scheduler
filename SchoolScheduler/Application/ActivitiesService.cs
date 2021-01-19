
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Contracts.DataTransferObjects.Schedule;
using Microsoft.EntityFrameworkCore;
using Model;
using Persistence;


namespace Application {
    public class ActivitiesService : IActivitiesService {
        private readonly ApplicationDbContext _context;

        public ActivitiesService(ApplicationDbContext context) {
            _context = context;
        }
        public async Task<ActivityEditDTO> GetActivity(int id) {
            var activity = await GetActivities().FirstOrDefaultAsync(a => a.Id == id);
            if (activity is null)
                throw new ArgumentException("Activity does not exist");
            return new ActivityEditDTO() {
                Id = activity.Id,
                Slot = activity.Slot.Index,
                ClassGroup = activity.ClassGroup.Name,
                Room = activity.Room.Name,
                Subject = activity.Subject.Name,
                Teacher = activity.Teacher.Name,
                Timestamp = activity.Timestamp
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
        public async Task CreateActivityAsync(ActivityCreateDTO activity) {
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

        private bool ValidateActivityForCreate(ActivityCreateDTO activity) {
            return ValidateActivity(
                GetActivities(),
                activity
            );
        }

        private bool ValidateActivity(IEnumerable<Activity> activities, ActivityBaseDTO activity) {
            return activities.Where(a =>
                a.Slot.Index == activity.Slot && (
                    a.Teacher.Name == activity.Teacher ||
                    a.Room.Name == activity.Room ||
                    a.ClassGroup.Name == activity.ClassGroup
                )
            ).Count() == 0;
        }
        public async Task EditActivityAsync(int id, ActivityEditDTO activity) {
            if (activity is null)
                throw new ArgumentException("Activity does not exist");

            var activityToEdit = GetActivities().FirstOrDefault(a => a.Id == id);
            if (activityToEdit is null)
                throw new ArgumentException("Activity does not exist");

            //TODO think about throwing exception while slot in dto is different from value from db 
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
        private bool ValidateActivityForEdit(int id, ActivityEditDTO activity) =>
            ValidateActivity(
                GetActivities().Where(a => a.Id != id),
                activity
            );
        public async Task DeleteActivityAsync(int id, ActivityDeleteDTO activityDTO) {
            var activity = _context.Activities.FirstOrDefault(a => a.Id == id);
            if (activity is null)
                throw new ArgumentException("Activity does not exist");
            _context.Entry(activity).Property("Timestamp").OriginalValue = activityDTO.Timestamp;
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
        }
    }
}
