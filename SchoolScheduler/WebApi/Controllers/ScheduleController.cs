using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using Contracts.DataTransferObjects.Schedule;
using Microsoft.EntityFrameworkCore;
using Contracts.DataTransferObjects;

namespace WebApi.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase {
        private readonly IScheduleService _scheduleService;
        public ScheduleController(IScheduleService scheduleService) {
            _scheduleService = scheduleService;
        }

        [HttpGet("rooms/{room}")]
        public ActionResult<ScheduleDTO> GetByRoom(string room)
        => _scheduleService.GetScheduleByRoom(room)
                .Match<ActionResult>(
                    Left: l => BadRequest(l),
                    Right: r => Ok(r)
                );

        [HttpGet("teachers/{teacher}")]
        public ActionResult<ScheduleDTO> GetByTeacher(string teacher)
            => _scheduleService.GetScheduleByTeacher(teacher)
                .Match<ActionResult>(
                    Left: l => BadRequest(l),
                    Right: r => Ok(r)
                );

        [HttpGet("classGroups/{group}")]
        public ActionResult<ScheduleDTO> GetByGroup(string group)
            => _scheduleService.GetScheduleByGroup(group)
                .Match<ActionResult>(
                    Left: l => BadRequest(l),
                    Right: r => Ok(r)
                );
    }
}