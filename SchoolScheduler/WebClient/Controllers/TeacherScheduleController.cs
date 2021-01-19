using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DataTransferObjects.Schedule;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;


namespace WebClient.Controllers {
    [Route("api/schedule/teacher")]
    public class TeacherController : GenericController<ITeacherService, ActivityByTeacherEditViewModel> {
        public TeacherController(ITeacherService teacherService) : base(teacherService) { }
    }
}