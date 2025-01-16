using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using MyWebApi.Model;

namespace MyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        string conStr = "Server=LERA;Database=ado;Trusted_Connection=True;TrustServerCertificate=True;";
        [HttpPost, Route("Method_8")]
        public ActionResult ReturnJson()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection db = new SqlConnection(conStr))
            {
                try
                {
                    var res = db.Query<Books>("pBooks",
                     commandType: System.Data.CommandType.StoredProcedure);

                    return Ok(res);
                }
                catch (Exception ex)
                {
                    return BadRequest(new
                    {
                        Message = "An error occurred while processing the request.",
                        Details = ex.Message
                    });
                }
            }
        }
    }
}
