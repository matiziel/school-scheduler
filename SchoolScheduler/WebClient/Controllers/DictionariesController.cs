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
            if (type is null)
                type = Enum.GetNames(typeof(DataType)).FirstOrDefault();
            ViewBag.Description = type;
            return View("./Views/Dictionaries/Index.cshtml", _editDataService.GetDictionary(GetValueFromString(type)));
        }

        public IActionResult Create(string type, string value) {
            try {
                if (type is null || value is null)
                    return NotFound();

                _editDataService.AddKey(value, GetValueFromString(type));
                return RedirectToAction("Index", new { type = type });
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        public IActionResult Delete(string type, string value) {
            try {
                if (type is null || value is null)
                    return NotFound();

                _editDataService.DeleteKey(value, GetValueFromString(type));
                return RedirectToAction("Index", new { type = type });
            }
            catch (Exception) {
                return BadRequest();
            }
        }
        private DataType GetValueFromString(string name) => (DataType)Enum.Parse(typeof(DataType), name);
    }
}