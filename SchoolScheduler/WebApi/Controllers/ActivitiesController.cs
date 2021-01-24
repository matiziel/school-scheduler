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
using System.Text;

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
                var x = await _activitiesService.GetActivity(id);
                return Ok(x);
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
                    return BadRequest(new ErrorDTO("Passed activity is invalid"));
                }
            }
            catch (InvalidOperationException e) {
                return BadRequest(new ErrorDTO(e.Message));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
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
                return BadRequest(new ErrorDTO("Passed activity is invalid"));
            }
            catch (DbUpdateConcurrencyException) {
                return BadRequest(new ErrorDTO("Someone has already updated this activity"));
            }
            catch (InvalidOperationException e) {
                return BadRequest(new ErrorDTO(e.Message));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
        [HttpDelete("{id:int}/{timestamp}")]
        public async Task<ActionResult> Delete(int? id, string timestamp) {
            try {
                if (id is null || timestamp is null)
                    NotFound();

                await _activitiesService.DeleteActivityAsync(id.Value, Convert.FromBase64String(timestamp));
                return Ok();
            }
            catch (DbUpdateConcurrencyException) {
                return BadRequest(new ErrorDTO("Someone has already updated this activity"));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
    }
}