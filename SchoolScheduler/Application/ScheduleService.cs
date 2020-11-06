using System;
using Contracts.Services;

namespace Application {
    public class ScheduleService : IScheduleService {
        private readonly DbContext _context;
        public ScheduleService(DbContext context) {
            _context = context;
        }
    }
}
