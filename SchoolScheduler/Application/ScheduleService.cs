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
        public Activity GetActivity(int slot, string room) {
            return _context.Schedule.Activities.FirstOrDefault(a => a.Slot == slot && a.Room == room);
        }

        public ScheduleViewModel GetScheduleByGroup(string group) {
            var activitiesByClass = _context.Schedule.Activities.Where(a => a.Group == group);
            var schedule = new ScheduleViewModel();
            foreach (var item in activitiesByClass) {
                if (item.Slot < schedule.Slots.Length)
                    schedule.Slots[item.Slot] = item.Room + " " + item.Class;
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

        
    }
}
