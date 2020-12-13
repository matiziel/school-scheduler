using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Common;
using Persistence;
using Contracts.ViewModels.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace WebClient.Controllers {
    public class DictionariesController : Controller {
        private readonly IDictionariesService _disctionariesService;
        public DictionariesController(IDictionariesService disctionariesService) => _disctionariesService = disctionariesService;
        public async Task<IActionResult> Index(string type) {
            try {
                if (type is null)
                    type = Enum.GetNames(typeof(DataType)).FirstOrDefault();
                ViewBag.Description = type;
                return View("./Views/Dictionaries/Index.cshtml", await _disctionariesService.GetDictionaryAsync(GetValueFromString(type)));
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        public IActionResult Create(string type) {
            try {
                if (type is null)
                    return View("./Views/ErrorView.cshtml", "Value cannot be empty");
                ViewBag.Method = "Create";
                ViewBag.Description = type;
                return View("./Views/Dictionaries/Edit.cshtml", new DictionaryElementEditViewModel() {
                });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(string type, DictionaryElementEditViewModel element) {
            try {
                if (type is null || element is null)
                    return View("./Views/ErrorView.cshtml", "Value cannot be empty");
                if (ModelState.IsValid) {
                    await _disctionariesService.AddKey(element, GetValueFromString(type));
                    return RedirectToAction("Index", new { type = type });
                }
                return RedirectToAction("Create", new { type = type });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }

        }
        public async Task<IActionResult> Edit(int? id, string type) {
            try {
                if (id is null || type is null)
                    return View("./Views/ErrorView.cshtml", "Value or id cannot be empty");
                ViewBag.Method = "Edit";
                ViewBag.Description = type;
                return View(await _disctionariesService.GetDictionaryElementAsync(id.Value, GetValueFromString(type)));
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string type, DictionaryElementEditViewModel element) {
            try {
                if (type is null || element is null)
                    return View("./Views/ErrorView.cshtml", "Value cannot be empty");
                if (ModelState.IsValid) {
                    await _disctionariesService.UpdateKey(element, GetValueFromString(type));
                    return RedirectToAction("Index", new { type = type });
                }
                return RedirectToAction("Edit", new { id = id, type = type });
            }
            catch (DbUpdateConcurrencyException) {
                return View("./Views/ErrorView.cshtml", "Someone has already update this element");
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }

        public async Task<IActionResult> Delete(int? id, string type) {
            try {
                if (id is null || type is null)
                    return View("./Views/ErrorView.cshtml", "Value or id cannot be empty");
                ViewBag.Description = type;
                var element = await _disctionariesService.GetDictionaryElementAsync(id.Value, GetValueFromString(type));
                return View("./Views/Dictionaries/Delete.cshtml", new DictionaryElementDeleteViewModel() { Id = element.Id.Value, Timestamp = element.Timestamp, Name = element.Name });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string type, DictionaryElementDeleteViewModel element) {
            try {
                if (type is null)
                    return View("./Views/ErrorView.cshtml", "Value cannot be empty");

                await _disctionariesService.RemoveKey(element.Id, element.Timestamp, GetValueFromString(type));
                return RedirectToAction("Index", new { type = type });
            }
            catch (DbUpdateConcurrencyException) {
                return View("./Views/ErrorView.cshtml", "Someone has already update this element");
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        private DataType GetValueFromString(string name) => (DataType)Enum.Parse(typeof(DataType), name);
    }
}