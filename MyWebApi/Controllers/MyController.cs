using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MyWebApi.Model;

namespace MyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyController : ControllerBase
    {
        [HttpGet, Route("Method_1")]
        public ActionResult Method_1()
        {
            return Ok("Hello Step");
        }


        [HttpGet, Route("Method_2/{name}/{id}")]
        public ActionResult Method_2(string name, string id)
        {
            return Ok($"Hello {name} - {id}");
        }


        [HttpGet, Route("Method_3/{a}/{b}/{c}")]
        public ActionResult Method_3(int a, int b, int c)
        {
            var v = a * b * c;
            return Ok(v);
        }


        [HttpPost, Route("Method_4")]
        public ActionResult Method_4(Book book)
        {
            return Ok($"{book.Author} - {book.Name} - {book.Content}");
        }

        [HttpGet, Route("Method_5/{a}/{b}/{c}")]
        public ActionResult Method_5(double a, double b, double c)
        {
            if (a == 0)
            {
                return BadRequest("A != 0");
            }

            double dis = b * b - 4 * a * c;
            if (dis > 0)
            {
                double x1 = (-b + Math.Sqrt(dis)) / (2 * a);
                double x2 = (-b - Math.Sqrt(dis)) / (2 * a);
                return Ok($"X1 - {x1}, X2 - {x2}");
            }

            if (dis == 0)
            {
                double x = -b / (2 * a);
                return Ok($"X - {x}");
            }
            else
            {
                return BadRequest("Dis < 0");
            }
        }



        [HttpGet, Route("Method_6/{fact}")]
        public ActionResult Method_6(int fact)
        {
            int h1 = fact;
            long h2 = 1;
            if (fact == 0)
            {
                return Ok($"Factorial = 1");
            }
            if (fact < 0)
            {
                return BadRequest("Ту мач");
            }
            else
            {
                for (int i = 1; i <= h1; i++)
                {
                    h2 *= i;
                }
                return Ok($"Factorial {h1} = {h2}");
            }

        }
        [HttpGet, Route("Method_7/{a}/{b}/{sym}")]
        public ActionResult Method_7(string sym, double a, double b)
        {
            double res = 0;
            switch (sym)
            {
                case "*":
                    res = a * b;
                    break;
                case "%":
                    if (b == 0)
                    {
                        return BadRequest("НОУ!");
                    }
                    res = a / b;
                    break;
                case "+":
                    res = a + b;
                    break;
                case "-":
                    res = a - b;
                    break;
                default:
                    return BadRequest("ТОК 4 ДЕЙСТВИЯ!");

            }
            return Ok(res);



        }
        public static string conStr = "Server=LERA;Database=ado;Trusted_Connection=True;TrustServerCertificate=True;";

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


        [HttpPut, Route("Method_9")]
        public ActionResult Method_9(Books book)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                DynamicParameters p = new DynamicParameters(book);
                db.Execute("pBooks;2", p, commandType: System.Data.CommandType.StoredProcedure);
                return Ok("Updated");

            }
        }

    }
}
