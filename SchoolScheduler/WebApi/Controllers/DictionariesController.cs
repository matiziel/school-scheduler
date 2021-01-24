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
using Contracts.DataTransferObjects;

namespace WebApi.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class DictionariesController : ControllerBase {
        private readonly IDictionariesService _disctionariesService;
        public DictionariesController(IDictionariesService disctionariesService) =>
            _disctionariesService = disctionariesService;

        [HttpGet("all/{type}")]
        public async Task<ActionResult<IEnumerable<DictionaryReadDTO>>> Get(string type) {
            try {
                if (type is null)
                    type = Enum.GetNames(typeof(DataType)).FirstOrDefault();
                return Ok(await _disctionariesService.GetDictionaryAsync(GetValueFromString(type)));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
        [HttpGet("rooms/{slot}")]
        public ActionResult<IEnumerable<string>> GetRooms(int slot, [FromQuery] int? id) {
            try {
                return Ok(_disctionariesService.GetFreeRoomsBySlot(slot, id));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
        [HttpGet("classGroups/{slot}")]
        public ActionResult<IEnumerable<string>> GetClassGroups(int slot, [FromQuery] int? id) {
            try {
                return Ok(_disctionariesService.GetFreeClassGroupsBySlot(slot, id));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
        [HttpGet("teachers/{slot}")]
        public ActionResult<IEnumerable<string>> GetTeachers(int slot, [FromQuery] int? id) {
            try {
                return Ok(_disctionariesService.GetFreeTeachersBySlot(slot, id));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
        [HttpGet("subjects")]
        public ActionResult<IEnumerable<string>> GetSubjects() {
            try {
                return Ok(_disctionariesService.GetAllSubjects());
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
        [HttpGet]
        public async Task<ActionResult<DictionaryReadDTO>> Get([FromQuery] int id, [FromQuery] string type) {
            try {
                if (type is null)
                    type = Enum.GetNames(typeof(DataType)).FirstOrDefault();
                return Ok(await _disctionariesService.GetDictionaryElementAsync(id, GetValueFromString(type)));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
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
                return BadRequest(new ErrorDTO("Passed dictionary element is invalid"));
            }
            catch (InvalidOperationException e) {
                return BadRequest(new ErrorDTO(e.Message));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }

        }
        [HttpPut]
        public async Task<IActionResult> Edit([FromQuery] int id, [FromQuery] string type, [FromBody] DictionaryElementEditDTO element) {
            try {
                if (type is null || element is null)
                    return NotFound();
                if (ModelState.IsValid) {
                    await _disctionariesService.UpdateKey(element, GetValueFromString(type));
                    return Ok();
                }
                return BadRequest(new ErrorDTO("Passed dictionary element is invalid"));
            }
            catch (DbUpdateConcurrencyException) {
                return BadRequest(new ErrorDTO("Someone has already updated this element"));
            }
            catch (InvalidOperationException e) {
                return BadRequest(new ErrorDTO(e.Message));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string type, [FromBody] DictionaryElementDeleteDTO element) {
            try {
                if (type is null)
                    return NotFound();
                if (ModelState.IsValid) {
                    await _disctionariesService.RemoveKey(element.Id, element.Timestamp, GetValueFromString(type));
                    return Ok();
                }
                return BadRequest(new ErrorDTO("Passed dictionary element is invalid"));

            }
            catch (DbUpdateConcurrencyException) {
                return BadRequest(new ErrorDTO("Someone has already updated this element"));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
        private DataType GetValueFromString(string name) => (DataType)Enum.Parse(typeof(DataType), name);
    }
}