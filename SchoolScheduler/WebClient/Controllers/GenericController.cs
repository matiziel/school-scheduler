using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using Contracts.ViewModels.Schedule;
using Microsoft.EntityFrameworkCore;

namespace WebClient.Controllers {
    public class GenericController<IService, ViewModel>
        : ControllerBase where IService : IBaseService<ViewModel> where ViewModel : ActivityViewModel {
        private readonly IService _service;
        public GenericController(IService scheduleService) {
            _service = scheduleService;
        }

        [HttpGet]
        public ActionResult<ScheduleViewModel> Get([FromQuery] string name) {
            try {
                return Ok(_service.GetSchedule(name));
            }
            catch (Exception) {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ActivityViewModel activity) {
            try {
                if (activity is null)
                    return NotFound();
                if (ModelState.IsValid) {
                    await _service.CreateActivityAsync(activity);
                    return Ok();
                }
                else {
                    return BadRequest();
                }
            }
            catch (Exception) {
                return BadRequest();
            }
        }
        [HttpPut]
        public async Task<ActionResult> Put([FromQuery] int? id,[FromBody] ActivityViewModel activity) {
            try {
                if (id is null || activity is null)
                    return NotFound();
                if (ModelState.IsValid) {
                    await _service.EditActivityAsync(id.Value, activity);
                    return Ok();
                }
                return BadRequest();
            }
            catch (DbUpdateConcurrencyException) {
                return BadRequest();
            }
            catch (Exception) {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<ActionResult> Delete([FromQuery] int? id, [FromBody] ActivityDeleteViewModel activity) {
            try {
                if (id is null || activity is null)
                    NotFound();
                await _service.DeleteActivityAsync(id.Value, activity.Timestamp);
                return Ok();
            }
            catch (DbUpdateConcurrencyException) {
                return BadRequest();
            }
            catch (Exception) {
                return BadRequest();
            }
        }
    }
}