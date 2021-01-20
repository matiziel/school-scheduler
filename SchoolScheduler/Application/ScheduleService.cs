using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Contracts.DataTransferObjects.Schedule;
using Contracts.DataTransferObjects.Activities;

using Microsoft.EntityFrameworkCore;
using Model;
using Persistence;


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
        public ScheduleDTO GetScheduleByGroup(string classGroup) {
            var activitiesByGroup = GetActivities().Where(a => a.ClassGroup.Name == classGroup);
            var schedule = new ScheduleDTO();
            foreach (var item in activitiesByGroup) {
                if (item.Slot.Id < schedule.Slots.Length) {
                    schedule.Slots[item.Slot.Index].Id = item.Id;
                    schedule.Slots[item.Slot.Index].Title = item.GetTitleForGroups();
                }
            }
            schedule.Name = classGroup;
            schedule.Type = "Group";
            return schedule;
        }

        public ScheduleDTO GetScheduleByRoom(string room) {
            var activitiesByGroup = GetActivities().Where(a => a.Room.Name == room);
            var schedule = new ScheduleDTO();
            foreach (var item in activitiesByGroup) {
                if (item.Slot.Id < schedule.Slots.Length) {
                    schedule.Slots[item.Slot.Index].Id = item.Id;
                    schedule.Slots[item.Slot.Index].Title = item.GetTitleForRooms();
                }
            }
            schedule.Name = room;
            schedule.Type = "Room";
            return schedule;
        }

        public ScheduleDTO GetScheduleByTeacher(string teacher) {
            var activitiesByGroup = GetActivities().Where(a => a.Teacher.Name == teacher);
            var schedule = new ScheduleDTO();
            foreach (var item in activitiesByGroup) {
                if (item.Slot.Id < schedule.Slots.Length) {
                    schedule.Slots[item.Slot.Index].Id = item.Id;
                    schedule.Slots[item.Slot.Index].Title = item.GetTitleForTeachers();
                }
            }
            schedule.Name = teacher;
            schedule.Type = "Teacher";
            return schedule;
        }
    }
}
