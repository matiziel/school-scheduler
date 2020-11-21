using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using Contracts.ViewModels;

namespace WebClient.Controllers {
    public class GroupController : Controller {
        private readonly IScheduleService _scheduleService;
        private readonly IDisctionariesService _disctionariesService;
        public GroupController(IScheduleService scheduleService, IDisctionariesService editDataService) {
            _scheduleService = scheduleService;
            _disctionariesService = editDataService;
        }
        public IActionResult Index(string name) {
            try {
                if (string.IsNullOrEmpty(name))
                    name = _disctionariesService.GetAllClassGroups().FirstOrDefault();
                ViewBag.Description = name;
                ViewBag.Name = "Group";
                ViewBag.DropDownListElements = _disctionariesService.GetAllClassGroups();
                return View("./Views/Schedule/Index.cshtml", _scheduleService.GetScheduleByGroup(name));
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        public IActionResult Create(int slot, string helper) {
            try {
                ViewBag.ListOfRooms = _disctionariesService.GetFreeRoomsBySlot(slot);
                ViewBag.ListOfClasses = _disctionariesService.GetAllSubjects();
                ViewBag.ListOfTeachers = _disctionariesService.GetFreeTeachersBySlot(slot);
                ViewBag.Method = "Create";
                return View("./Views/Schedule/EditForGroup.cshtml");
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }

        [HttpPost]
        public IActionResult Create(ActivityEditViewModel activity) {
            try {
                if (activity is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");

                _scheduleService.CreateActivity(activity);
                return RedirectToAction("Index");
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }

        public IActionResult Edit(int? id) {
            try {
                if (id is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");
                var activity = _scheduleService.GetActivity(id.Value);

                // ViewBag.ListOfRooms = _disctionariesService.GetFreeRoomsBySlot(activity.Slot, id);
                // ViewBag.ListOfClasses = _disctionariesService.GetAllSubjects();
                // ViewBag.ListOfTeachers = _disctionariesService.GetFreeTeachersBySlot(activity.Slot, id);
                ViewBag.Method = "Edit";
                return View("./Views/Schedule/EditForGroup.cshtml", activity);
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        [HttpPost]
        public IActionResult Edit(int? id, ActivityEditViewModel activity) {
            try {
                if (id is null || activity is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");

                _scheduleService.EditActivity(id.Value, activity);
                return RedirectToAction("Index");
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        [HttpPost]
        public IActionResult Delete(int? id, string group) {
            try {
                if (id is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");

                _scheduleService.DeleteActivity(id.Value);
                return RedirectToAction("Index", new { name = group });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
    }
}

