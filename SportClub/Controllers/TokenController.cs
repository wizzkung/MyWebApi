using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using SportClub.Model;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SportClub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        IConfiguration config;

        public TokenController(IConfiguration configuration)
        {
            config = configuration;
        }

        [HttpPost, Route("GetToken"), AllowAnonymous]
        public ActionResult GetToken(Users model)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(config["db"]))
                {
                    // Получаем данные пользователя
                    var user = sql.QueryFirstOrDefault<UserData>(
                        "pCheckUserLogin",
                        new
                        {
                            @Login = model.login,
                            @Password = model.password
                        },
                        commandType: CommandType.StoredProcedure);

                    if (user == null || !user.IsAuthenticated)
                    {
                        return Unauthorized(new AuthResponse
                        {
                            Status = "ERROR",
                            Message = "Authentication failed"
                        });
                    }

                    // Генерация токена (ваш существующий код)
                    var token = GenerateJwtToken(model.login, user.Status, user.UserType);

                    // Возвращаем ответ с дополнительными полями
                    return Ok(new AuthResponse
                    {
                        Status = "OK",
                        Token = token,
                        UserStatus = user.Status,
                        UserType = user.UserType
                    });
                }
            }
            catch (Exception err)
            {
                return StatusCode(500, new AuthResponse
                {
                    Status = "ERROR",
                    Message = err.Message
                });
            }
        }

        private string GenerateJwtToken(string login, string status, string userType)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, login),
        new Claim("status", status),
        new Claim("userType", userType),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
