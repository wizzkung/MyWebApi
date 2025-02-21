using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MyReact.DTO;

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

        [HttpPost, Route("AddModel")]
        public async Task<IActionResult> AddModel(ModelTypeDTO model)
        {
            await using var db = new SqlConnection(configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = await db.ExecuteAsync("pBmw;4", new { @name = model.name, @seriesName = model.seriesName }, commandType: System.Data.CommandType.StoredProcedure);
                return Ok($"Новая модель успешно добавлена - {res}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
