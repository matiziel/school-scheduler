
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using Contracts.ViewModels.Schedule;
using Microsoft.EntityFrameworkCore;

namespace WebClient.Controllers {

    [Route("api/schedule/group")]
    public class GroupScheduleController : GenericController<IGroupService, ActivityByGroupEditViewModel> {
        public GroupScheduleController(IGroupService groupService) : base(groupService) { }
    }
}