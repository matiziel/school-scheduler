using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace WebClient.Controllers {
    public class ClassController : Controller {
        private readonly IScheduleService _scheduleService;
        public ClassController(IScheduleService scheduleService) {
            _scheduleService = scheduleService;
        }
        public IActionResult Index() {
            return View("./Views/Schedule/Index.cshtml", _scheduleService.GetScheduleByGroup("1a"));
        }
    }
}