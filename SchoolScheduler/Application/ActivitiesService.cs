
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Contracts.DataTransferObjects.Activities;
using Microsoft.EntityFrameworkCore;
using Model;
using Persistence;
using LanguageExt;
using static LanguageExt.Prelude;
using Contracts.DataTransferObjects;

namespace Application {
    public class ActivitiesService : IActivitiesService {
        private readonly ApplicationDbContext _context;

        public ActivitiesService(ApplicationDbContext context) {
            _context = context;
        }
        public async Task<Either<ErrorDTO, ActivityEditDTO>> GetActivity(int id) {
            var activity = await GetActivities().AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            if (activity is null)
                return Left(new ErrorDTO("Activity does not exist"));
            return Right(new ActivityEditDTO() {
                Id = activity.Id,
                Slot = activity.Slot.Index,
                ClassGroup = activity.ClassGroup.Name,
                Room = activity.Room.Name,
                Subject = activity.Subject.Name,
                Teacher = activity.Teacher.Name,
                Timestamp = activity.Timestamp
            });
        }
        private IQueryable<Activity> GetActivities() {
            return _context.Activities
                .Include(a => a.Slot)
                .Include(a => a.Teacher)
                .Include(a => a.Room)
                .Include(a => a.ClassGroup)
                .Include(a => a.Subject);
        }
        public async Task<Either<ErrorDTO, Unit>> CreateActivityAsync(ActivityCreateDTO activity) {
            if (activity is null)
                return Left(new ErrorDTO("Activity does not exists"));

            if (!ValidateActivityForCreate(activity))
                return Left(new ErrorDTO("One of values on this slot is occupied"));

            var classGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Name == activity.ClassGroup);
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Name == activity.Subject);
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Name == activity.Teacher);
            var slot = await _context.Slots.FirstOrDefaultAsync(s => s.Index == activity.Slot);
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Name == activity.Room);

            var createdActivity = new Activity(room, classGroup, subject, slot, teacher);
            if (ValidateUpdatedActivity(createdActivity))
                return Left(new ErrorDTO("One of values is incorrect"));

            try {
                await _context.Activities.AddAsync(createdActivity);
                await _context.SaveChangesAsync();
                return Right(Unit.Default);
            }
            catch (Exception) {
                return Left(new ErrorDTO("Error while updating database"));
            }
        }
        private bool ValidateUpdatedActivity(Activity activity)
            => !(activity.Room is null ||
                activity.ClassGroup is null ||
                activity.Subject is null ||
                activity.Teacher is null ||
                activity.Slot is null);

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
        public async Task<Either<ErrorDTO, Unit>> EditActivityAsync(int id, ActivityEditDTO activity) {
            if (activity is null)
                return Left(new ErrorDTO("Activity does not exist"));

            var activityToEdit = GetActivities().FirstOrDefault(a => a.Id == id);
            if (activityToEdit is null)
                return Left(new ErrorDTO("Activity does not exist"));

            if (!ValidateActivityForEdit(id, activity))
                return Left(new ErrorDTO("One of values on this slot is occupied"));

            activityToEdit.ClassGroup = await _context.ClassGroups.FirstOrDefaultAsync(c => c.Name == activity.ClassGroup);
            activityToEdit.Subject = await _context.Subjects.FirstOrDefaultAsync(c => c.Name == activity.Subject);
            activityToEdit.Teacher = await _context.Teachers.FirstOrDefaultAsync(c => c.Name == activity.Teacher);
            activityToEdit.Room = await _context.Rooms.FirstOrDefaultAsync(c => c.Name == activity.Room);

            if (ValidateUpdatedActivity(activityToEdit))
                return Left(new ErrorDTO("One of values is incorrect"));

            try {
                _context.Entry(activityToEdit).Property("Timestamp").OriginalValue = activity.Timestamp;
                _context.Activities.Update(activityToEdit);
                await _context.SaveChangesAsync();
                return Right(Unit.Default);
            }
            catch (DbUpdateConcurrencyException) {
                return Left(new ErrorDTO("Someone has already updated this activity"));
            }
            catch (Exception) {
                return Left(new ErrorDTO("Error while updating database"));
            }
        }
        private bool ValidateActivityForEdit(int id, ActivityEditDTO activity) =>
            ValidateActivity(
                GetActivities().Where(a => a.Id != id),
                activity
            );

        public async Task<Either<ErrorDTO, Unit>> DeleteActivityAsync(int id, byte[] timestamp) {
            var activity = _context.Activities.FirstOrDefault(a => a.Id == id);
            if (activity is null)
                return Left(new ErrorDTO("Activity does not exist"));
            try {
                _context.Entry(activity).Property("Timestamp").OriginalValue = timestamp;
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
                return Right(Unit.Default);
            }
            catch (DbUpdateConcurrencyException) {
                return Left(new ErrorDTO("Someone has already updated this activity"));
            }
            catch (Exception) {
                return Left(new ErrorDTO("Error while updating database"));
            }

        }
    }
}
