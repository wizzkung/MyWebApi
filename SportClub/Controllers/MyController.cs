using Azure.Core;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SportClub.Model;
using System.Data;

namespace SportClub.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class MyController : ControllerBase
    {
        IConfiguration _configuration;

        public MyController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet, Route("GetSchedule")]
        public async Task<IActionResult> GetSchedule()
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = db.Query("pClubSchedule", commandType: System.Data.CommandType.StoredProcedure);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost, Route("ChangeAbout")]
        public async Task<IActionResult> ChangeAboutClient([FromBody]About about)
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var p = new
                {
                    userId = about.id,
                    phoneNumber = about.phoneNum,
                    oldPass = about.old_pass, 
                    newPass = about.new_pass  
                };
            await db.ExecuteAsync("pClubChangeAboutClient", p, commandType: System.Data.CommandType.StoredProcedure);
                return Ok("Данные успешно обновлены");
            }
            catch (SqlException ex) when (ex.Message.Contains("Неверный текущий пароль"))
            {
                return BadRequest(new { error = "Неверный текущий пароль" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ошибка сервера: " + ex.Message });
            }

        }


        [HttpPost, Route("AddStuff")]
        public async Task<IActionResult> AddStuff(Stuff stuff)
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var p = new
                {
                    @firstname = stuff.first_name,
                    @lastname = stuff.last_name,
                    @dateBirth = stuff.dt,
                    @phoneNum = stuff.phoneNum,
                    @status = stuff.status,
                    @login = stuff.login,
                    @password = stuff.password,
                    @userType = stuff.userType,
                    @specializations = string.Join(",", stuff.specializations)
                };
             var res = await db.ExecuteAsync("AddClubStaff",p, commandType: System.Data.CommandType.StoredProcedure);
                return Ok("Сотрудник успешно добавлен");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost, Route("AddClient")]
        public async Task<IActionResult> AddClient(Clients clients)
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var p = new
                {
                    @firstname = clients.first_name,
                    @lastname = clients.last_name,
                    @dateBirth = clients.dt,
                    @phoneNum = clients.phoneNum,
                    @gender = clients.gender,
                    @login = clients.login,
                    @password = clients.password,
             
                };
                var res = await db.ExecuteAsync("AddClient", p, commandType: System.Data.CommandType.StoredProcedure);
                return Ok("Клиент успешно добавлен");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete, Route("DeleteClient/{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = await db.ExecuteAsync("pDeleteClient", new { @id = id }, commandType: System.Data.CommandType.StoredProcedure);
                return Ok("Клиент успешно удален");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete, Route("DeleteStuff/{id}")]
        public async Task<IActionResult> DeleteStuff(int id)
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = await db.ExecuteAsync("pDeleteStuff", new {@id = id },commandType: System.Data.CommandType.StoredProcedure);
                return Ok("Сотрудник успешно удален");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet, Route("GetClients")]
        public async Task<IActionResult> GetClients()
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = db.Query("pShowClients", commandType: System.Data.CommandType.StoredProcedure);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet, Route("GetStuff")]
        public async Task<IActionResult> GetStuff()
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = db.Query("pShowStuff", commandType: System.Data.CommandType.StoredProcedure);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost, Route("AddSchedule")]
        public async Task<IActionResult> AddSchedule(InsertSchedule schedule)
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var p = new
                {
                    @stuffId = schedule.stuff_id,
                    @specializationId = schedule.specialization_id,
                    @groupSchedule = schedule.group_schedule,
                    @individualSchedule = schedule.individual_schedule
                };

                var res = await db.ExecuteAsync("AddSchedule", p, commandType: System.Data.CommandType.StoredProcedure);

                return Ok("Расписание успешно добавлено");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet, Route("GetGyms")]
        public async Task<IActionResult> GetGyms()
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = db.Query("pShowGyms", commandType: System.Data.CommandType.StoredProcedure);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost, Route("AddGym")]
        public async Task<IActionResult> AddGym(Gym gym)
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var p = new
                {
                    @name = gym.name,
                    @address = gym.address
                 
                };

                var res = await db.ExecuteAsync("pAddGym", p, commandType: System.Data.CommandType.StoredProcedure);

                return Ok("Зал успешно добавлен");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("ShowGyms")]
        public async Task<IActionResult> ShowGyms()
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = db.Query("pShowGyms", commandType: System.Data.CommandType.StoredProcedure);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

            [HttpDelete, Route("DeleteGym/{id}")]
            public async Task<IActionResult> DeleteGym(int id)
            {
                await using var db = new SqlConnection(_configuration["db"]);
                try
                {
                    await db.OpenAsync();
                    var gymExists = await db.ExecuteScalarAsync<bool>(
              "SELECT 1 FROM Gyms WHERE id = @id",
              new { id });

                    if (!gymExists)
                    {
                        return NotFound(new { error = "Зал с указанным ID не найден" });
                    }

                    var rowsAffected = await db.ExecuteAsync(
                        "pDeleteGym",
                        new { id },
                        commandType: CommandType.StoredProcedure);

                
                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Зал успешно удален" });
                    }
                    else
                    {
                        return BadRequest(new { error = "Не удалось удалить зал" });
                    }
                }

                catch (Exception ex)
                {
                    return StatusCode(500, new { error = "Ошибка сервера", details = ex.Message });
                }

            }

        [HttpGet, Route("ChangeGymStatus/{id}")]
        public async Task<IActionResult> ChangeGymStatus(int id)
        {
            await using var db = new SqlConnection(_configuration["db"]);
            try
            {
                await db.OpenAsync();
                var res = await db.ExecuteAsync("pGymActiveStatus", new { @id = id }, commandType: System.Data.CommandType.StoredProcedure);
                return Ok($"Статус успешно изменен");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }






    }
}
