using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using Contracts.DataTransferObjects.Schedule;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase {
        private readonly IScheduleService _scheduleService;
        public ScheduleController(IScheduleService scheduleService) {
            _scheduleService = scheduleService;
        }

        [HttpGet("rooms/{room}")]
        public ActionResult<ScheduleDTO> GetByRoom(string room) {
            try {
                return Ok(_scheduleService.GetScheduleByRoom(room));
            }
            catch (Exception) {
                return NotFound();
            }
        }
        [HttpGet("teachers/{teacher}")]
        public ActionResult<ScheduleDTO> GetByTeacher(string teacher) {
            try {
                return Ok(_scheduleService.GetScheduleByTeacher(teacher));
            }
            catch (Exception) {
                return NotFound();
            }
        }
        [HttpGet("classGroups/{group}")]
        public ActionResult<ScheduleDTO> GetByGroup(string group) {
            try {
                return Ok(_scheduleService.GetScheduleByGroup(group));
            }
            catch (Exception) {
                return NotFound();
            }
        }
    }
}