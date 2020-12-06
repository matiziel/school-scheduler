using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Contracts.ViewModels.Schedule;
using Microsoft.EntityFrameworkCore;

namespace WebClient.Controllers {
    public class TeacherController : Controller {
        private readonly IScheduleService _scheduleService;
        private readonly IDisctionariesService _disctionariesService;
        public TeacherController(IScheduleService scheduleService, IDisctionariesService disctionariesService) {
            _scheduleService = scheduleService;
            _disctionariesService = disctionariesService;
        }
        public IActionResult Index(string name) {
            try {
                if (string.IsNullOrEmpty(name))
                    name = _disctionariesService.GetAllTeachers().FirstOrDefault();
                ViewBag.Description = name;
                ViewBag.Name = "Teacher";
                ViewBag.DropDownListElements = _disctionariesService.GetAllTeachers();
                return View("./Views/Schedule/Index.cshtml", _scheduleService.GetScheduleByTeacher(name));
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        public IActionResult Create(int slot, string helper) {
            try {
                ViewBag.ListOfRooms = _disctionariesService.GetFreeRoomsBySlot(slot);
                ViewBag.ListOfClasses = _disctionariesService.GetAllSubjects();
                ViewBag.ListOfGroups = _disctionariesService.GetFreeClassGroupsBySlot(slot);
                ViewBag.Method = "Create";
                return View("./Views/Schedule/EditForTeacher.cshtml", new ActivityEditViewModel() {
                    Slot = slot,
                    Teacher = helper
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
                    return View("./Views/ErrorView.cshtml", "Error during http request.");
                if (ModelState.IsValid) {
                    await _scheduleService.CreateActivityAsync(activity);
                    return RedirectToAction("Index", new { name = activity.Teacher });
                }
                return RedirectToAction("Create", new { slot = activity.Slot, helper = activity.Teacher });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }

        public async Task<IActionResult> Edit(int? id) {
            try {
                if (id is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request.");
                var activity = await _scheduleService.GetActivityAsync(id.Value);

                ViewBag.ListOfRooms = _disctionariesService.GetFreeRoomsBySlot(activity.Slot, id);
                ViewBag.ListOfClasses = _disctionariesService.GetAllSubjects();
                ViewBag.ListOfGroups = _disctionariesService.GetFreeClassGroupsBySlot(activity.Slot, id);
                ViewBag.Method = "Edit";
                return View("./Views/Schedule/EditForTeacher.cshtml", activity);
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
                    return RedirectToAction("Index", new { name = activity.Teacher });
                }
                return RedirectToAction("Edit", new { id = id.Value });
            }
            catch (DbUpdateConcurrencyException) {
                return View("./Views/ErrorView.cshtml", "Someone has already update this activity");
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id, string teacher, byte[] timestamp) {
            try {
                if (id is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");

                await _scheduleService.DeleteActivityAsync(id.Value, timestamp);
                return RedirectToAction("Index", new { name = teacher });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
    }
}