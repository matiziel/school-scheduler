using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Services;
using Contracts.ViewModels;
using Model;
using Persistence;

namespace Application {
    public class ScheduleService : IScheduleService {
        public void CreateActivity(Activity activity) {
            throw new NotImplementedException();
        }

        public void DeleteActivity(int id) {
            throw new NotImplementedException();
        }

        public void EditActivity(int id, Activity activity) {
            throw new NotImplementedException();
        }

        public Activity GetActivity(int id) {
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
