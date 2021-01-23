using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using Contracts.DataTransferObjects.Activities;
using Microsoft.EntityFrameworkCore;
using Contracts.DataTransferObjects;

namespace WebApi.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : ControllerBase {
        private readonly IActivitiesService _activitiesService;
        public ActivitiesController(IActivitiesService activitiesService) {
            _activitiesService = activitiesService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActivityEditDTO>> Get(int id) {
            try {
                return Ok(await _activitiesService.GetActivity(id));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ActivityCreateDTO activity) {
            try {
                if (activity is null)
                    return NotFound();
                if (ModelState.IsValid) {
                    await _activitiesService.CreateActivityAsync(activity);
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
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int? id, [FromBody] ActivityEditDTO activity) {
            try {
                if (activity is null)
                    return NotFound();
                if (ModelState.IsValid) {
                    await _activitiesService.EditActivityAsync(id.Value, activity);
                    return Ok();
                }
                return BadRequest();
            }
            catch (DbUpdateConcurrencyException) {
                return BadRequest();
            }
            catch (Exception e) {
                return NotFound();
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int? id, [FromBody] ActivityDeleteDTO activity) {
            try {
                if (id is null || activity is null)
                    NotFound();
                await _activitiesService.DeleteActivityAsync(id.Value, activity);
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