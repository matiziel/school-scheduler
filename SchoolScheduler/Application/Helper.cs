using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Services;
using Contracts.DataTransferObjects.Dictionaries;
using Model;
using Persistence;
using Common;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application {
    internal static class Helper {
        internal static IEnumerable<Activity> GetActivitiesBySlot(ApplicationDbContext context, int slot, int? id) {
            var activities = context.Activities
                .Include(a => a.Slot)
                .Include(a => a.ClassGroup)
                .Include(a => a.Room)
                .Include(a => a.Teacher)
                .AsNoTracking();
            if (id is null)
                return activities.Where(a => a.Slot.Index == slot);
            else
                return activities.Where(a => a.Slot.Index == slot && a.Id != id.Value);
        }
    }
}