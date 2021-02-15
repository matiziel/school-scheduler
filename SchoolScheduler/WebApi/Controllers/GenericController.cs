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
        public async Task<ActionResult<IEnumerable<DictionaryReadDTO>>> Get()
            => (await _service.GetDictionaryAsync())
                .Match<ActionResult<IEnumerable<DictionaryReadDTO>>>(
                    Left: l => NotFound(l),
                    Right: r => Ok(r)
                );

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
        public async Task<ActionResult<DictionaryReadDTO>> Get(int id)
            => (await _service.GetDictionaryElementAsync(id))
                .Match<ActionResult<DictionaryReadDTO>>(
                    Left: l => NotFound(l),
                    Right: r => Ok(r)
                );

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] DictionaryElementCreateDTO element)
        => (await _service.AddKey(element))
                .Match<ActionResult>(
                    Left: l => NotFound(l),
                    Right: r => Ok(r)
                );

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int? id, [FromBody] DictionaryElementEditDTO element)
            => (await _service.UpdateKey(element))
                .Match<ActionResult>(
                    Left: l => NotFound(l),
                    Right: r => Ok(r)
                );

        [HttpDelete("{id:int}/{timestamp}")]
        public async Task<IActionResult> Delete(int? id, string timestamp)
            => (await _service.RemoveKey(id.Value, Convert.FromBase64String(timestamp)))
                .Match<ActionResult>(
                    Left: l => NotFound(l),
                    Right: r => Ok(r)
                );
    }
}