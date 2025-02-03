using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;

namespace BmwFullProjectJquery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestController : ControllerBase
    {
        IConfiguration _configuration;
        public RestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet, Route("GetAll")]
        public async Task<IActionResult> GetAllCars()
        {
            await using var db = new SqlConnection(_configuration["db"]);
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

        [HttpGet, Route("GetBySeries")]
        public async Task<IActionResult> GetBySeries()
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                DynamicParameters
                var res = db.Query("pBmw;2", commandType: System.Data.CommandType.StoredProcedure);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
