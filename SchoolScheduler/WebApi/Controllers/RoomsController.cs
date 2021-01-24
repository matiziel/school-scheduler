using Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : GenericController<IRoomsService> {
        public RoomsController(IRoomsService service) : base(service) { }
    }
}