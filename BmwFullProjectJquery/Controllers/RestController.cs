using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using BmwFullProjectJquery.Model;
using BmwFullProjectJquery.DTO;
using OfficeOpenXml;
using System.Text;
using BmwFullProjectJquery.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using BmwFullProjectJquery.Model.DeepSeek;
using Microsoft.AspNetCore.Authorization;

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

        //[HttpGet("Auth"), Authorize(AuthenticationSchemes = "BasicAuthentication")]
        //public ActionResult Auth()
        //{
        //    // Если BasicAuthHandler вернёт успех, вы попадёте сюда
        //    return Ok("Вы успешно авторизовались на сайте!");
        //}

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
                return Ok($"Новая модель успешно добавлена - {res}");
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
                return Ok($"Модель успешно обновлена - {res}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("Delete/{modelName}/{seriesName}")]
        public async Task<IActionResult> DeleteModel([FromRoute] string modelName, [FromRoute] string seriesName)
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = await db.ExecuteAsync("pBmw;7", new { @modelName = modelName, @seriesName = seriesName }, commandType: System.Data.CommandType.StoredProcedure);
                return Ok($"Модель успешно  удалена - {res}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("excel")]
        public async Task<IActionResult> LoadExcel()
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = db.Query("pBmw", commandType: System.Data.CommandType.StoredProcedure);
                if (res == null || !res.Any())
                    return BadRequest();

                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Data");

                var columns = ((IDictionary<string, object>)res.First()).Keys.ToList();
                for (int i = 0; i < columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = columns[i];
                }

                int row = 2;
                foreach (dynamic item in res)
                {
                    var values = ((IDictionary<string, object>)item).Values;
                    for (int col = 0; col < values.Count; col++)
                    {
                        worksheet.Cells[row, col + 1].Value = values.ElementAt(col);
                    }
                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");

            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet, Route("csv")]
        public async Task<IActionResult> LoadCsv()
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = db.Query("pBmw", commandType: CommandType.StoredProcedure);

                if (res == null || !res.Any())
                    return BadRequest("Нет данных для выгрузки");

                var firstRow = (IDictionary<string, object>)res.First();
                var columns = firstRow.Keys.ToList();

                
                var sb = new StringBuilder();

                // Заголовок
                sb.AppendLine(string.Join(";", columns));

                // Данные
                foreach (dynamic row in res)
                {
                    var dict = (IDictionary<string, object>)row;
                    var line = string.Join(";", dict.Values.Select(v => v?.ToString().Replace(";", ",")));
                    sb.AppendLine(line);
                }

                var bytes = Encoding.UTF8.GetBytes(sb.ToString());
                return File(new MemoryStream(bytes), "text/csv", "Report.csv");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("deepSeek")]
        public async Task<IActionResult> GenerateModelDescription([FromBody] ModelRequest request)
        {
            try
            {
                using var deepSeek = new DeepSeekChatService(_configuration["DeepSeek:ApiKey"]);

                var messages = new List<ChatMessage>
            {
                new("system", "You are a BMW expert assistant. Provide detailed technical specifications in Markdown format."),
                new("user", $"Generate detailed description for BMW {request.ModelName} in Russian. Include: production years, engine options, technical innovations and historical significance.")
            };

                var response = await deepSeek.SendChatAsync(messages);

                if (response.error != null)
                    return BadRequest(new { error = response.error.message });

                return Ok(new
                {
                    description = response.choices?.FirstOrDefault()?.message.content
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }





}
