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
    public class GenericController<IService, ViewModel> 
        : Controller where IService : IBaseService<ViewModel> where ViewModel : ActivityViewModel   {
        private readonly IService _service;
        public GenericController(IService scheduleService) {
            _service = scheduleService;
        }
        public IActionResult Index(string name) {
            try {
                return View("./Views/Schedule/Index.cshtml", _service.GetSchedule(name));
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        public IActionResult Create(int slot, string helper) {
            try {
                ViewBag.Method = "Create";
                return View(_service.GetEmptyActivity(slot, helper));
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
                    await _service.CreateActivityAsync(activity);
                    return RedirectToAction("Index", new { name = activity.ArgumentHelper });
                }
                return RedirectToAction("Create", new { slot = activity.Slot, helper = activity.ArgumentHelper });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }

        public async Task<IActionResult> Edit(int? id) {
            try {
                if (id is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");
                var activity = await _service.GetActivityAsync(id.Value);
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
                    await _service.EditActivityAsync(id.Value, activity);
                    return RedirectToAction("Index", new { name = activity.ArgumentHelper });
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
        public async Task<IActionResult> Delete(int? id, string name, byte[] timestamp) {
            try {
                if (id is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");
                await _service.DeleteActivityAsync(id.Value, timestamp);
                return RedirectToAction("Index", new { name = name });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
    }
}

