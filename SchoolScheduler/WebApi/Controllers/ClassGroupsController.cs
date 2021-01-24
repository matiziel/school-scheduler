using Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class ClassGroupsController : GenericController<IClassGroupsService> {
        public ClassGroupsController(IClassGroupsService service) : base(service) { }
    }
}