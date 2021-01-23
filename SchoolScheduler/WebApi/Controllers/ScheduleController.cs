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
        public ActionResult<ScheduleDTO> GetByRoom(string room) {
            try {
                return Ok(_scheduleService.GetScheduleByRoom(room));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
        [HttpGet("teachers/{teacher}")]
        public ActionResult<ScheduleDTO> GetByTeacher(string teacher) {
            try {
                return Ok(_scheduleService.GetScheduleByTeacher(teacher));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
        [HttpGet("classGroups/{group}")]
        public ActionResult<ScheduleDTO> GetByGroup(string group) {
            try {
                return Ok(_scheduleService.GetScheduleByGroup(group));
            }
            catch (Exception e) {
                return NotFound(new ErrorDTO(e.Message));
            }
        }
    }
}