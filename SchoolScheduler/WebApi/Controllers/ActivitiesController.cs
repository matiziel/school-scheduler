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
using LanguageExt;

namespace WebApi.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : ControllerBase {
        private readonly IActivitiesService _activitiesService;
        public ActivitiesController(IActivitiesService activitiesService)
            => _activitiesService = activitiesService;


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActivityEditDTO>> Test(int id)
            => (await _activitiesService.GetActivity(id))
                .Match<ActionResult<ActivityEditDTO>>(
                    Left: l => NotFound(l),
                    Right: r => Ok(r)
                );

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ActivityCreateDTO activity)
            => (await _activitiesService.CreateActivityAsync(activity))
                .Match<ActionResult>(
                    Left: l => BadRequest(l),
                    Right: _ => Ok()
                );


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int? id, [FromBody] ActivityEditDTO activity)
            => (await _activitiesService.EditActivityAsync(id.Value, activity))
            .Match<ActionResult>(
                Left: l => BadRequest(l),
                Right: _ => Ok()
            );

        [HttpDelete("{id:int}/{timestamp}")]
        public async Task<ActionResult> Delete(int? id, string timestamp)
            => (await _activitiesService.DeleteActivityAsync(id.Value, Convert.FromBase64String(timestamp)))
                .Match<ActionResult>(
                    Left: l => NotFound(l),
                    Right: _ => Ok()
                );
    }
}