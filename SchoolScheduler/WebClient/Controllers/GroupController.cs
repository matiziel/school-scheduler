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

    public class GroupController : GenericController<IGroupService, ActivityByGroupEditViewModel> {
        public GroupController(IGroupService groupService) : base(groupService) { }
    }
}

