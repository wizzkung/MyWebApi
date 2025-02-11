using BmwFullProjectJquery.Model;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BmwFullProjectJquery.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTController : ControllerBase
    {
        IConfiguration config;

        public JWTController(IConfiguration configuration)
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
                    var res = sql.ExecuteScalar<bool>("CheckPass", new { @login = model.login, @pass = model.password }, commandType: System.Data.CommandType.StoredProcedure);
                    if (!res)
                    {
                        return Unauthorized(new ReturnStatus
                        {
                            status = StatusEnum.ERROR,
                            result = "error"
                        });
                    }
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new[] {
                          new Claim("myRole", "admin"),
                          new Claim("dateBirth", "2000-01-01")
                };

                    var token = new JwtSecurityToken(config["Jwt:Issuer"],
                        config["Jwt:Issuer"],
                        claims,
                        //null,
                        expires: DateTime.Now.AddMinutes(5),
                        signingCredentials: credentials);

                    var sToken = new JwtSecurityTokenHandler().WriteToken(token);

                    return Ok(new ReturnStatus
                    {
                        status = StatusEnum.OK,
                        result = sToken
                    });
                }
            }
            catch (Exception err)
            {
                return Ok(new ReturnStatus
                {
                    status = StatusEnum.ERROR,
                    result = "error",
                    error = err.Message
                });
            }
        }
    }
}
