using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebClient.Controllers {
    public class DictionariesController : Controller {
        private readonly IEditDataService _editDataService;
        public DictionariesController(IEditDataService editDataService) => _editDataService = editDataService;

        public IActionResult Index(string type) {
            try {
                if (type is null)
                    type = Enum.GetNames(typeof(DataType)).FirstOrDefault();
                ViewBag.Description = type;
                return View("./Views/Dictionaries/Index.cshtml", _editDataService.GetDictionary(GetValueFromString(type)));
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }

        }

        public IActionResult Create(string type, string value) {
            try {
                if (type is null || value is null)
                    return View("./Views/ErrorView.cshtml", "Value cannot be empty");

                _editDataService.AddKey(value, GetValueFromString(type));
                return RedirectToAction("Index", new { type = type });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }

        public IActionResult Delete(string type, string value) {
            try {
                if (type is null || value is null)
                    return View("./Views/ErrorView.cshtml", "Error during http request");

                _editDataService.DeleteKey(value, GetValueFromString(type));
                return RedirectToAction("Index", new { type = type });
            }
            catch (Exception e) {
                return View("./Views/ErrorView.cshtml", e.Message);
            }
        }
        private DataType GetValueFromString(string name) => (DataType)Enum.Parse(typeof(DataType), name);
    }
}