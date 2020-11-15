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
                ViewBag.Name = "Nauczyciel";
                ViewBag.DropDownListElements = _editDataService.GetAllTeachers();
                return View("./Views/Schedule/Index.cshtml", _scheduleService.GetScheduleByTeacher(name));
            }
            catch (Exception) {
                return BadRequest();
            }
        }
        public IActionResult Create(int slot, string helper) {
            try {
                ViewBag.ListOfRooms = _editDataService.GetAllRooms();
                ViewBag.ListOfClasses = _editDataService.GetAllClasses();
                ViewBag.ListOfGroups = _editDataService.GetAllGroups();
                ViewBag.Method = "Create";
                return View("./Views/Schedule/EditForTeacher.cshtml", new Activity {
                    Slot = slot,
                    Teacher = helper
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
                return RedirectToAction("Index", new { name = activity.Teacher });
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        public IActionResult Edit(int? id) {
            try {
                if (id is null)
                    return NotFound();
                ViewBag.ListOfRooms = _editDataService.GetAllRooms();
                ViewBag.ListOfClasses = _editDataService.GetAllClasses();
                ViewBag.ListOfGroups = _editDataService.GetAllGroups();
                ViewBag.Method = "Edit";
                return View("./Views/Schedule/EditForTeacher.cshtml", _scheduleService.GetActivity(id.Value));
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
                return RedirectToAction("Index", new { name = activity.Teacher });
            }
            catch (Exception) {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Delete(int? id, string teacher) {
            try {
                if (id is null)
                    return NotFound();

                _scheduleService.DeleteActivity(id.Value);
                return RedirectToAction("Index", new { name = teacher });
            }
            catch (Exception) {
                return BadRequest();
            }
        }
    }
}