using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using Microsoft.EntityFrameworkCore;
using Contracts.DataTransferObjects.Schedule;

namespace WebClient.Controllers {
    [Route("api/schedule/room")]
    public class RoomScheduleController : GenericController<IRoomService, ActivityByRoomEditViewModel> {
        public RoomScheduleController(IRoomService roomService) : base(roomService) { }
    }
}
