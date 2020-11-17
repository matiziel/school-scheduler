using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebClient.Controllers {
    public class TeacherController : Controller {
        private readonly IScheduleService _scheduleService;
        private readonly IEditDataService _editDataService;
        public TeacherController(IScheduleService scheduleService, IEditDataService editDataService) {
            _scheduleService = scheduleService;
            _editDataService = editDataService;
        }
        public IActionResult Index(string name) {
            try {
                if (string.IsNullOrEmpty(name))
                    name = _editDataService.GetAllTeachers().FirstOrDefault();
                ViewBag.Description = name;
                ViewBag.Name = "Teacher";
                ViewBag.DropDownListElements = _editDataService.GetAllTeachers();
                return View("./Views/Schedule/Index.cshtml", _scheduleService.GetScheduleByTeacher(name));
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        public IActionResult Create(int slot, string helper) {
            try {
                ViewBag.ListOfRooms = _editDataService.GetFreeRoomsBySlot(slot);
                ViewBag.ListOfClasses = _editDataService.GetAllClasses();
                ViewBag.ListOfGroups = _editDataService.GetFreeGroupsBySlot(slot);
                ViewBag.Method = "Create";
                return View("./Views/Schedule/EditForTeacher.cshtml");
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }

        [HttpPost]
        public IActionResult Create(Activity activity) {
            try {
                if (activity is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request.");
                _scheduleService.CreateActivity(activity);
                return RedirectToAction("Index", new { name = activity.Teacher });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }

        public IActionResult Edit(int? id) {
            try {
                if (id is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request.");
                var activity = _scheduleService.GetActivity(id.Value);

                // ViewBag.ListOfRooms = _editDataService.GetFreeRoomsBySlot(activity.Slot, id);
                // ViewBag.ListOfClasses = _editDataService.GetAllClasses();
                // ViewBag.ListOfGroups = _editDataService.GetFreeGroupsBySlot(activity.Slot, id);
                ViewBag.Method = "Edit";
                return View("./Views/Schedule/EditForTeacher.cshtml", activity);
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
                return RedirectToAction("Index", new { name = activity.Teacher });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        [HttpPost]
        public IActionResult Delete(int? id, string teacher) {
            try {
                if (id is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");

                _scheduleService.DeleteActivity(id.Value);
                return RedirectToAction("Index", new { name = teacher });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
    }
}