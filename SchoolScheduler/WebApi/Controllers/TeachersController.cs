using Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class TeachersController : GenericController<ITeachersService> {
        public TeachersController(ITeachersService service) : base(service) { }
    }
}