using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Contracts.ViewModels.Schedule;
using Microsoft.EntityFrameworkCore;

namespace WebClient.Controllers {
    public class TeacherController : GenericController<ITeacherService, ActivityByTeacherEditViewModel> {
        public TeacherController(ITeacherService teacherService) : base(teacherService) { }
    }
}