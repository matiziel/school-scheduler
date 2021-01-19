using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Common;
using Persistence;
using Contracts.DataTransferObjects.Dictionaries;
using Microsoft.EntityFrameworkCore;


namespace WebClient.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class DictionariesController : ControllerBase {
        private readonly IDictionariesService _disctionariesService;
        public DictionariesController(IDictionariesService disctionariesService) =>
            _disctionariesService = disctionariesService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DictionaryIndexViewModel>>> Get(string type) {
            try {
                if (type is null)
                    type = Enum.GetNames(typeof(DataType)).FirstOrDefault();
                return Ok(await _disctionariesService.GetDictionaryAsync(GetValueFromString(type)));
            }
            catch (Exception) {
                return NotFound();
            }
        }
        // [HttpPost]
        // public IActionResult Create(string type) {
        //     try {
        //         if (type is null)
        //             return View("./Views/ErrorView.cshtml", "Value cannot be empty");
        //         ViewBag.Method = "Create";
        //         ViewBag.Description = type;
        //         return View("./Views/Dictionaries/Edit.cshtml", new DictionaryElementEditViewModel() {
        //         });
        //     }
        //     catch (Exception e) {
        //         return View("./Views/ErrorView.cshtml", e.Message);
        //     }
        // }
        private DataType GetValueFromString(string name) => (DataType)Enum.Parse(typeof(DataType), name);



    }
}