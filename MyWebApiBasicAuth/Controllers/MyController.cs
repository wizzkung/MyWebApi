using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApiBasicAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MyController : ControllerBase
    {
        [HttpGet, Route("Method_1")]
        public ActionResult Method_1()
        {
            return Ok("Hello Step");
        }

    }
}
