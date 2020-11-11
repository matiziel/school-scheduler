using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace WebClient.Controllers {
    public class ClassController : Controller {
        private readonly IScheduleService _scheduleService;
        public ClassController(IScheduleService scheduleService) {
            _scheduleService = scheduleService;
        }
        public IActionResult Index(string group) {
            if (string.IsNullOrEmpty(group))
                group = _scheduleService.GetAllGroups().FirstOrDefault();
            ViewBag.Description = group;
            ViewBag.Name = "Klasa";
            ViewBag.DropDownListElements = _scheduleService.GetAllGroups();
            return View("./Views/Schedule/Index.cshtml", _scheduleService.GetScheduleByGroup(group));
        }
        public IActionResult Edit(int? slot, string room) {
            if (slot is null || string.IsNullOrEmpty(room))
                return NotFound();
            return View("./Views/Schedule/Edit.cshtml", _scheduleService.GetActivity(slot.Value, room));
        }
    }
}

