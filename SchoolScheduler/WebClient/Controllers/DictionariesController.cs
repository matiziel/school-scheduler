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

namespace WebClient.Controllers {
    public class DictionariesController : Controller {
        private readonly IDisctionariesService _disctionariesService;
        public DictionariesController(IDisctionariesService disctionariesService) => _disctionariesService = disctionariesService;
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

                await _disctionariesService.AddKey(element, GetValueFromString(type));
                return RedirectToAction("Index", new { type = type });
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
                return View("./Views/Dictionaries/Edit.cshtml", await _disctionariesService.GetDictionaryElementAsync(id.Value, GetValueFromString(type)));
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

                await _disctionariesService.UpdateKey(element, GetValueFromString(type));
                return RedirectToAction("Index", new { type = type });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }

        public async Task<IActionResult> Delete(int? id, string type) {
            try {
                if (id is null || type is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");

                await _disctionariesService.RemoveKey(id.Value, GetValueFromString(type));
                return RedirectToAction("Index", new { type = type });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        private DataType GetValueFromString(string name) => (DataType)Enum.Parse(typeof(DataType), name);
    }
}