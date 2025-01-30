using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WindowsAuthentication.Controllers
{
    [Route("[controller]")]
    [ApiController, Authorize]
    public class MyController : ControllerBase
    {
        [HttpGet, Route("Test")]
        public ActionResult Test()
        {
            var username = User.Identity.Name.Split('\\')[1];
            return Ok(username);
        }

    }
}
