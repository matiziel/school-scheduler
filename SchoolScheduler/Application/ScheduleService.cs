using System.Linq;
using Contracts.Services;
using Contracts.DataTransferObjects.Schedule;
using Microsoft.EntityFrameworkCore;
using Model;
using Persistence;
using LanguageExt;
using static LanguageExt.Prelude;
using Contracts.DataTransferObjects;
using System;

namespace Application {
    public class ScheduleService : IScheduleService {
        private readonly ApplicationDbContext _context;

        public ScheduleService(ApplicationDbContext context) {
            _context = context;
        }
        private IQueryable<Activity> GetActivities() {
            return _context.Activities
                .Include(a => a.Slot)
                .Include(a => a.Teacher)
                .Include(a => a.Room)
                .Include(a => a.ClassGroup)
                .Include(a => a.Subject)
                .AsNoTracking();
        }
        public Either<ErrorDTO, ScheduleDTO> GetScheduleByGroup(string classGroup) {
            try {
                var activitiesByGroup = GetActivities().Where(a => a.ClassGroup.Name == classGroup);
                var schedule = new ScheduleDTO();
                foreach (var item in activitiesByGroup) {
                    if (item.Slot.Index < schedule.Slots.Length) {
                        schedule.Slots[item.Slot.Index].Id = item.Id;
                        schedule.Slots[item.Slot.Index].Title = item.GetTitleForGroups();
                    }
                }
                schedule.Name = classGroup;
                schedule.Type = "classGroups";
                return Right(schedule);
            }
            catch (Exception e) {
                return Left(new ErrorDTO(e.Message));
            }
        }
        public Either<ErrorDTO, ScheduleDTO> GetScheduleByRoom(string room) {
            try {
                var activitiesByGroup = GetActivities().Where(a => a.Room.Name == room);
                var schedule = new ScheduleDTO();
                foreach (var item in activitiesByGroup) {
                    if (item.Slot.Index < schedule.Slots.Length) {
                        schedule.Slots[item.Slot.Index].Id = item.Id;
                        schedule.Slots[item.Slot.Index].Title = item.GetTitleForRooms();
                    }
                }
                schedule.Name = room;
                schedule.Type = "rooms";
                return Right(schedule);
            }
            catch (Exception e) {
                return Left(new ErrorDTO(e.Message));
            }
        }
        public Either<ErrorDTO, ScheduleDTO> GetScheduleByTeacher(string teacher) {
            try {
                var activitiesByGroup = GetActivities().Where(a => a.Teacher.Name == teacher);
                var schedule = new ScheduleDTO();
                foreach (var item in activitiesByGroup) {
                    if (item.Slot.Index < schedule.Slots.Length) {
                        schedule.Slots[item.Slot.Index].Id = item.Id;
                        schedule.Slots[item.Slot.Index].Title = item.GetTitleForTeachers();
                    }
                }
                schedule.Name = teacher;
                schedule.Type = "teachers";
                return Right(schedule);
            }
            catch (Exception e) {
                return Left(new ErrorDTO(e.Message));
            }
        }
    }
}
