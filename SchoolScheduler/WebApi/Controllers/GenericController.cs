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
    public class GenericController<IService> : ControllerBase
    where IService : IDictionariesService {
        private readonly IService _service;
        public GenericController(IService service) =>
            _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DictionaryReadDTO>>> Get() {
            try {
                return Ok(await _service.GetDictionaryAsync());
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
        [HttpGet("slot/{slot:int}")]
        public ActionResult<IEnumerable<string>> GetBySlot(int slot, [FromQuery] int? id) {
            try {
                return Ok(_service.GetDictionaryBySlot(slot, id));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DictionaryReadDTO>> Get(int id) {
            try {
                return Ok(await _service.GetDictionaryElementAsync(id));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] DictionaryElementCreateDTO element) {
            try {
                if (element is null)
                    return NotFound();
                if (ModelState.IsValid) {
                    await _service.AddKey(element);
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
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int? id, [FromBody] DictionaryElementEditDTO element) {
            try {
                if (id is null || element is null)
                    return NotFound();
                if (ModelState.IsValid) {
                    await _service.UpdateKey(element);
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
        [HttpDelete("{id:int}/{timestamp}")]
        public async Task<IActionResult> Delete(int? id, string timestamp) {
            try {
                if (id is null || timestamp is null)
                    return NotFound();

                await _service.RemoveKey(id.Value, Convert.FromBase64String(timestamp));
                return Ok();
            }
            catch (DbUpdateConcurrencyException) {
                return BadRequest(new ErrorDTO("Someone has already updated this element"));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
    }
}