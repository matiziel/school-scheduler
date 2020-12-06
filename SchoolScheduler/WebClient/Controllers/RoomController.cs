using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using Contracts.ViewModels.Schedule;
using Microsoft.EntityFrameworkCore;

namespace WebClient.Controllers {
    public class RoomController : Controller {
        private readonly IScheduleService _scheduleService;
        public RoomController(IScheduleService scheduleService) {
            _scheduleService = scheduleService;
        }
        public IActionResult Index(string name) {
            try {
                ViewBag.Name = "Room";
                return View("./Views/Schedule/Index.cshtml", _scheduleService.GetScheduleByRoom(name));
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        public IActionResult Create(int slot, string helper) {
            try {
                ViewBag.Method = "Create";
                return View(_scheduleService.GetEmptyActivityByRoom(slot, helper));
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ActivityViewModel activity) {
            try {
                if (activity is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");
                if (ModelState.IsValid) {
                    await _scheduleService.CreateActivityAsync(activity);
                    return RedirectToAction("Index", new { name = activity.Room });
                }
                return RedirectToAction("Create", new { slot = activity.Slot, helper = activity.Room });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }

        public async Task<IActionResult> Edit(int? id) {
            try {
                if (id is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");
                var activity = await _scheduleService.GetActivityByRoomAsync(id.Value);
                ViewBag.Method = "Edit";
                return View(activity);
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, ActivityViewModel activity) {
            try {
                if (id is null || activity is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");
                if (ModelState.IsValid) {
                    await _scheduleService.EditActivityAsync(id.Value, activity);
                    return RedirectToAction("Index", new { name = activity.Room });
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
        public async Task<IActionResult> Delete(int? id, string room, byte[] timestamp) {
            try {
                if (id is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");

                await _scheduleService.DeleteActivityAsync(id.Value, timestamp);
                return RedirectToAction("Index", new { name = room });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
    }
}

