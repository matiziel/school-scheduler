using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebClient.Controllers {
    public class GroupController : Controller {
        private readonly IScheduleService _scheduleService;
        private readonly IEditDataService _editDataService;
        public GroupController(IScheduleService scheduleService, IEditDataService editDataService) {
            _scheduleService = scheduleService;
            _editDataService = editDataService;
        }
        public IActionResult Index(string name) {
            try {
                if (string.IsNullOrEmpty(name))
                    name = _editDataService.GetAllGroups().FirstOrDefault();
                ViewBag.Description = name;
                ViewBag.Name = "Klasa";
                ViewBag.DropDownListElements = _editDataService.GetAllGroups();
                return View("./Views/Schedule/Index.cshtml", _scheduleService.GetScheduleByGroup(name));
            }
            catch (Exception) {
                return BadRequest();
            }
        }
        public IActionResult Create(int slot, string helper) {
            try {
                ViewBag.ListOfRooms = _editDataService.GetFreeRoomsBySlot(slot);
                ViewBag.ListOfClasses = _editDataService.GetAllClasses();
                ViewBag.ListOfTeachers = _editDataService.GetFreeTeachersBySlot(slot);
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
                return RedirectToAction("Index", new { name = activity.Group });
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        public IActionResult Edit(int? id) {
            try {
                if (id is null)
                    return NotFound();
                var activity = _scheduleService.GetActivity(id.Value);

                ViewBag.ListOfRooms = _editDataService.GetFreeRoomsBySlot(activity.Slot, id);
                ViewBag.ListOfClasses = _editDataService.GetAllClasses();
                ViewBag.ListOfTeachers = _editDataService.GetFreeTeachersBySlot(activity.Slot, id);
                ViewBag.Method = "Edit";
                return View("./Views/Schedule/EditForGroup.cshtml", activity);
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
                return RedirectToAction("Index", new { name = activity.Group });
            }
            catch (Exception) {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Delete(int? id, string group) {
            try {
                if (id is null)
                    return NotFound();

                _scheduleService.DeleteActivity(id.Value);
                return RedirectToAction("Index", new { name = group });
            }
            catch (Exception) {
                return BadRequest();
            }
        }
    }
}

