using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebClient.Controllers {
    public class DictionariesController : Controller {
        private readonly IEditDataService _editDataService;
        public DictionariesController(IEditDataService editDataService) => _editDataService = editDataService;

        public IActionResult Index(string name) {
            return View("./Views/Dictionaries/Index.cshtml");
        }
    }
}