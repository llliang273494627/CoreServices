using System.Collections.Generic;
using BackServices.Api.Comm;
using Microsoft.AspNetCore.Mvc;

namespace BackServices.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApbController : ControllerBase
    {
        [HttpGet]
        [CustomRoute(ApiVersions.V1, "apbs")]
        public IEnumerable<string> Get()
        {
            return new string[] { "第一版的 apbs" };
        }

    }
}
