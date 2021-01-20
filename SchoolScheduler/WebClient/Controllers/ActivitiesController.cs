using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using Contracts.DataTransferObjects.Activities;
using Microsoft.EntityFrameworkCore;

namespace WebClient.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : ControllerBase {
        private readonly IActivitiesService _activitiesService;
        public ActivitiesController(IActivitiesService activitiesService) {
            _activitiesService = activitiesService;
        }

        [HttpGet]
        public ActionResult<ActivityEditDTO> Get([FromQuery] int id) {
            try {
                return Ok(_activitiesService.GetActivity(id));
            }
            catch (Exception) {
                return NotFound();
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
        [HttpPut]
        public async Task<ActionResult> Put([FromQuery] int? id, [FromBody] ActivityEditDTO activity) {
            try {
                if (id is null || activity is null)
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
            catch (Exception) {
                return NotFound();
            }
        }
        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] int? id, [FromBody] ActivityDeleteDTO activity) {
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