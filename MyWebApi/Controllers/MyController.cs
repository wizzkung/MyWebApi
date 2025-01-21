using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MyWebApi.Model;
using System.Text;

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


        //[HttpPost, Route("Method_4")]
        //public ActionResult Method_4(Book book)
        //{
        //    return Ok($"{book.Author} - {book.Name} - {book.Content}");
        //}

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

        [HttpPost, Route("Method_10")]
        public ActionResult Method_10(AuthorMain model)
        {
                StringBuilder sb = new StringBuilder();
            foreach(var item in model.Books)
            {
                sb.AppendLine($"{item.Name} - {model.Author}");
            }    

            return Ok(sb.ToString());
        }



        [HttpGet, Route("Method_11")]
        public int Method_11(string id)
        {
            return
            int.Parse(id) * int.Parse(id);
        }

        [HttpPost, Route("Method_12")]
        public ActionResult Method_12(string[] id)
        {
            return Ok(id.Length);
        }


        [HttpGet, Route("Method_13")]
        public Books Method_13(string Author, string BookName)
        {
            return (new Books { Name = BookName, Author = Author });
        }

        [HttpGet, Route("Method_14")]
        public ActionResult Method_14(string id)
        {
            if(id == "1")
            return Ok();
            else if (id == "2")
                return NotFound("Not found");
            else if (id == "3")
                return BadRequest("Wrong params");
            else if (id == "4")
                return Unauthorized("No auth");
            else if (id == "5")
                return NoContent();
            else
                return Ok();


        }
        [HttpGet, Route("ReturnFile")]
        public ActionResult ReturnFiles(string name)
        {
            var environment = HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment)) as IWebHostEnvironment;
            if (environment == null)
            {
                return StatusCode(500, "Неправильно указаны данные");
            }
           string path = environment.ContentRootPath;
            string filePath = Path.Combine(path, name);
            if (System.IO.File.Exists(filePath))
            {

                string contentType = GetMimeType(filePath);

                return PhysicalFile(filePath, contentType, name);
            }
            else
            {
               
                return NotFound(new { Message = $"Файл '{name}' не найден" });
            }
        }

        private string GetMimeType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension switch
            {
                ".txt" => "text/plain",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream",
            };
        }
    } 
}
