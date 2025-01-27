using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;


namespace MyWebApiBasicAuth.Auth
{
    public class BasicAuth : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        IConfiguration _configuration;
        public BasicAuth(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            _configuration = configuration; 
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("No section Authorization");
            try
            {
                // "user1;password"
                var value = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credValue = Convert.FromBase64String(value.Parameter);
                var credArray = Encoding.UTF8.GetString(credValue).Split(':');
                var login = credArray[0];
                var psw = credArray[1];

                /*
                    проверка в БД            
                */
                bool isValid = await Check(login, psw);


                //string cred = "user1;1234";
                if (isValid)
                {

                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Name, login),
                    new Claim("psw", psw)
                };


                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
                else
                {
                    return AuthenticateResult.Fail("Invalid login or password.");
                }
            }
            catch (Exception err)
            {
                return AuthenticateResult.Fail(err.Message);
            }
        }


        private async Task<bool> Check(string login, string pass)
        {
            using (SqlConnection db = new SqlConnection(_configuration["db"]))
            {
                var param = new DynamicParameters();
                param.Add("@login", login);
                param.Add("@pass", pass);

                var res = await db.ExecuteScalarAsync<bool>("pCheckPass", param, commandType: System.Data.CommandType.StoredProcedure);
                return res;
            }
        }
    }
}
