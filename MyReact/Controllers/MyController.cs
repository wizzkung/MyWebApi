using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace MyReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyController : ControllerBase
    {
        IConfiguration configuration;
        public MyController(IConfiguration configuration) {
        this.configuration = configuration;
        }
        [HttpGet, Route("GetAll")]
        public async Task<IActionResult> GetAllCars()
        {
            await using var db = new SqlConnection(configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = db.Query("pBmw", commandType: System.Data.CommandType.StoredProcedure);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
