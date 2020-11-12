using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using Persistence;

namespace WebClient.Controllers {
    public class GroupController : Controller {
        private readonly IScheduleService _scheduleService;
        public GroupController(IScheduleService scheduleService) {
            _scheduleService = scheduleService;
        }
        public IActionResult Index(string group) {
            try {
                if (string.IsNullOrEmpty(group))
                    group = _scheduleService.GetAllGroups().FirstOrDefault();
                ViewBag.Description = group;
                ViewBag.Name = "Klasa";
                ViewBag.DropDownListElements = _scheduleService.GetAllGroups();
                return View("./Views/Schedule/Index.cshtml", _scheduleService.GetScheduleByGroup(group));
            }
            catch (Exception) {
                return BadRequest();
            }
        }
        public IActionResult Create(int slot, string helper) {
            try {
                ViewBag.ListOfRooms = _scheduleService.GetAllRooms();
                ViewBag.ListOfClasses = _scheduleService.GetAllClasses();
                ViewBag.ListOfTeachers = _scheduleService.GetAllTeachers();
                ViewBag.Method = "Create";
                return View("./Views/Schedule/EditForGroup.cshtml", new Activity {
                    Slot = slot,
                    Group = helper
                });
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Create(Activity activity) {
            try {
                if (activity is null)
                    return BadRequest();

                _scheduleService.CreateActivity(activity);
                return RedirectToAction("Index", new { group = activity.Group });
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        public IActionResult Edit(int? id) {
            try {
                if (id is null)
                    return NotFound();
                ViewBag.ListOfRooms = _scheduleService.GetAllRooms();
                ViewBag.ListOfClasses = _scheduleService.GetAllClasses();
                ViewBag.ListOfTeachers = _scheduleService.GetAllTeachers();
                ViewBag.Method = "Edit";
                return View("./Views/Schedule/EditForGroup.cshtml", _scheduleService.GetActivity(id.Value));
            }
            catch (Exception) {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Edit(int? id, Activity activity) {
            try {
                if (id is null || activity is null)
                    return NotFound();

                _scheduleService.EditActivity(id.Value, activity);
                return RedirectToAction("Index", new { group = activity.Group });
            }
            catch (Exception) {
                return BadRequest();
            }
        }
    }
}

