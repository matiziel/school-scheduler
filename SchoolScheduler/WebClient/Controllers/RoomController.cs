using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebClient.Controllers {
    public class RoomController : Controller {
        private readonly IScheduleService _scheduleService;
        private readonly IEditDataService _editDataService;
        public RoomController(IScheduleService scheduleService, IEditDataService editDataService) {
            _scheduleService = scheduleService;
            _editDataService = editDataService;
        }
        public IActionResult Index(string name) {
            try {
                if (string.IsNullOrEmpty(name))
                    name = _editDataService.GetAllRooms().FirstOrDefault();
                ViewBag.Description = name;
                ViewBag.Name = "Room";
                ViewBag.DropDownListElements = _editDataService.GetAllRooms();
                return View("./Views/Schedule/Index.cshtml", _scheduleService.GetScheduleByRoom(name));
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        public IActionResult Create(int slot, string helper) {
            try {
                ViewBag.ListOfGroups = _editDataService.GetFreeGroupsBySlot(slot);
                ViewBag.ListOfClasses = _editDataService.GetAllClasses();
                ViewBag.ListOfTeachers = _editDataService.GetFreeTeachersBySlot(slot);
                ViewBag.Method = "Create";
                return View("./Views/Schedule/EditForRoom.cshtml");
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }

        [HttpPost]
        public IActionResult Create(Activity activity) {
            try {
                if (activity is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");

                _scheduleService.CreateActivity(activity);
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

                // ViewBag.ListOfGroups = _editDataService.GetFreeGroupsBySlot(activity.Slot, id);
                // ViewBag.ListOfClasses = _editDataService.GetAllClasses();
                // ViewBag.ListOfTeachers = _editDataService.GetFreeTeachersBySlot(activity.Slot, id);
                ViewBag.Method = "Edit";
                return View("./Views/Schedule/EditForRoom.cshtml", activity);
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        [HttpPost]
        public IActionResult Edit(int? id, Activity activity) {
            try {
                if (id is null || activity is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");

                _scheduleService.EditActivity(id.Value, activity);
                return RedirectToAction("Index", new { name = activity.Room });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        [HttpPost]
        public IActionResult Delete(int? id, string room) {
            try {
                if (id is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");

                _scheduleService.DeleteActivity(id.Value);
                return RedirectToAction("Index", new { name = room });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
    }
}

