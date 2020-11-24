using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using Contracts.ViewModels.Schedule;

namespace WebClient.Controllers {
    public class RoomController : Controller {
        private readonly IScheduleService _scheduleService;
        private readonly IDisctionariesService _disctionariesService;
        public RoomController(IScheduleService scheduleService, IDisctionariesService disctionariesService) {
            _scheduleService = scheduleService;
            _disctionariesService = disctionariesService;
        }
        public IActionResult Index(string name) {
            try {
                if (string.IsNullOrEmpty(name))
                    name = _disctionariesService.GetAllRooms().FirstOrDefault();
                ViewBag.Description = name;
                ViewBag.Name = "Room";
                ViewBag.DropDownListElements = _disctionariesService.GetAllRooms();
                return View("./Views/Schedule/Index.cshtml", _scheduleService.GetScheduleByRoom(name));
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        public IActionResult Create(int slot, string helper) {
            try {
                ViewBag.ListOfGroups = _disctionariesService.GetFreeClassGroupsBySlot(slot);
                ViewBag.ListOfClasses = _disctionariesService.GetAllSubjects();
                ViewBag.ListOfTeachers = _disctionariesService.GetFreeTeachersBySlot(slot);
                return View("./Views/Schedule/EditForRoom.cshtml");
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

                await _scheduleService.CreateActivity(activity);
                return RedirectToAction("Index", new { name = activity.Room });
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

                ViewBag.ListOfGroups = _disctionariesService.GetFreeClassGroupsBySlot(activity.Slot, id);
                ViewBag.ListOfClasses = _disctionariesService.GetAllSubjects();
                ViewBag.ListOfTeachers = _disctionariesService.GetFreeTeachersBySlot(activity.Slot, id);
                return View("./Views/Schedule/EditForRoom.cshtml", activity);
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

                await _scheduleService.EditActivity(id.Value, activity);
                return RedirectToAction("Index", new { name = activity.Room });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id, string room) {
            try {
                if (id is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");

                await _scheduleService.DeleteActivity(id.Value);
                return RedirectToAction("Index", new { name = room });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
    }
}

