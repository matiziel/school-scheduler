using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace WebClient.Controllers {
    public class ClassController : Controller {
        private readonly DbContext _context;
        public ClassController(DbContext context) {
            _context = context;
        }
        public IActionResult Index() {
            return View("./Views/Schedule/Index.cshtml", _context.Schedule.Activities);
        }
    }
}