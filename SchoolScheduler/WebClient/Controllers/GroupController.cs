using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using Contracts.ViewModels.Schedule;

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
                return View("./Views/Schedule/EditForGroup.cshtml", new ActivityEditViewModel() {
                    Slot = slot,
                    ClassGroup = helper
                });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ActivityEditViewModel activity) {
            try {
                if (activity is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");
                if (ModelState.IsValid) {
                    await _scheduleService.CreateActivityAsync(activity);
                    return RedirectToAction("Index", new { name = activity.ClassGroup });
                }
                return RedirectToAction("Create", new { slot = activity.Slot, helper = activity.ClassGroup });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }

        public async Task<IActionResult> Edit(int? id) {
            try {
                if (id is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");
                var activity = await _scheduleService.GetActivityAsync(id.Value);

                ViewBag.ListOfRooms = _disctionariesService.GetFreeRoomsBySlot(activity.Slot, id);
                ViewBag.ListOfClasses = _disctionariesService.GetAllSubjects();
                ViewBag.ListOfTeachers = _disctionariesService.GetFreeTeachersBySlot(activity.Slot, id);
                ViewBag.Method = "Edit";
                return View("./Views/Schedule/EditForGroup.cshtml", activity);
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, ActivityEditViewModel activity) {
            try {
                if (id is null || activity is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");
                if (ModelState.IsValid) {
                    await _scheduleService.EditActivityAsync(id.Value, activity);
                    return RedirectToAction("Index", new { name = activity.ClassGroup });
                }
                return RedirectToAction("Index", new { id = id.Value });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id, string group) {
            try {
                if (id is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");

                await _scheduleService.DeleteActivityAsync(id.Value);
                return RedirectToAction("Index", new { name = group });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
    }
}

