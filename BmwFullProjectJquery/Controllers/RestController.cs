using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using BmwFullProjectJquery.Model;
using BmwFullProjectJquery.DTO;

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

        [HttpGet, Route("GetBySeries/{name}")]
        public async Task<IActionResult> GetBySeries(string name)
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = await db.QueryAsync("pBmw;2", new { @seriesName = name }, commandType: System.Data.CommandType.StoredProcedure);
                if (!res.Any())
                {
                    return NotFound("No cars found for the given series.");
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("GetByModel/{name}")]
        public async Task<IActionResult> GetByModel(string name)
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = await db.QueryAsync("pBmw;3", new { @name = name }, commandType: System.Data.CommandType.StoredProcedure);
                if (!res.Any())
                {
                    return NotFound("No cars found for the given series.");
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("AddModel")]
        public async Task<IActionResult> AddModel(ModelTypeDTO bmw)
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = await db.ExecuteAsync("pBmw;4", new { @name = bmw.name, @seriesName = bmw.seriesName }, commandType: System.Data.CommandType.StoredProcedure);
                return Ok("Новая модель успешно добавлена");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("Update")]
        public async Task<IActionResult> UpdateModel(ModelTypeDTOUpdate bmw)
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = await db.ExecuteAsync("pBmw;6", new { @oldName = bmw.oldName, @newName = bmw.dto.name, @seriesName = bmw.dto.seriesName }, commandType: System.Data.CommandType.StoredProcedure);
                return Ok("Модель успешно обновлена");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
