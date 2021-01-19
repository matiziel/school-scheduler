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
        public async Task<ActionResult<IEnumerable<DictionaryReadDTO>>> Get([FromQuery] string type) {
            try {
                if (type is null)
                    type = Enum.GetNames(typeof(DataType)).FirstOrDefault();
                return Ok(await _disctionariesService.GetDictionaryAsync(GetValueFromString(type)));
            }
            catch (Exception) {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<ActionResult<DictionaryReadDTO>> Get([FromQuery] int id, [FromQuery] string type) {
            try {
                if (type is null)
                    type = Enum.GetNames(typeof(DataType)).FirstOrDefault();
                return Ok(await _disctionariesService.GetDictionaryElementAsync(id, GetValueFromString(type)));
            }
            catch (Exception) {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromQuery] string type, [FromBody] DictionaryElementCreateDTO element) {
            try {
                if (type is null || element is null)
                    return NotFound();
                if (ModelState.IsValid) {
                    await _disctionariesService.AddKey(element, GetValueFromString(type));
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception) {
                return BadRequest();
            }

        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromQuery] int id,[FromQuery] string type, [FromBody] DictionaryElementEditDTO element) {
            try {
                if (type is null || element is null)
                    return NotFound();
                if (ModelState.IsValid) {
                    await _disctionariesService.UpdateKey(element, GetValueFromString(type));
                    return RedirectToAction("Index", new { type = type });
                }
                return RedirectToAction("Edit", new { id = id, type = type });
            }
            catch (DbUpdateConcurrencyException) {
                return BadRequest();
            }
            catch (Exception) {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromQuery] string type,[FromBody] DictionaryElementDeleteDTO element) {
            try {
                if (type is null)
                    return NotFound();

                await _disctionariesService.RemoveKey(element.Id, element.Timestamp, GetValueFromString(type));
                return Ok();
            }
            catch (DbUpdateConcurrencyException) {
                return BadRequest();
            }
            catch (Exception) {
                return NotFound();
            }
        }
        private DataType GetValueFromString(string name) => (DataType)Enum.Parse(typeof(DataType), name);
    }
}