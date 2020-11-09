using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GXVCU.Api.Comm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GXVCU.Api.Controllers.v1
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
