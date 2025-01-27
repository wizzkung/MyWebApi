using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Model;
using System.Numerics;
using Complex = MyWebApi.Model.Complex;

namespace MyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplexController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        IConfiguration _configuration;
        public ComplexController(IWebHostEnvironment web, IConfiguration configuration)
        {
            _webHostEnvironment = web;
            _configuration = configuration;
        }

        [HttpPost, Route("Method_1")]
        public IActionResult Post([FromForm] Complex complex)
        {
            try
            {
                var excel = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", complex.file.FileName);
                Directory.CreateDirectory(Path.GetDirectoryName(excel)!);
                using (var stream = new FileStream(excel, FileMode.Create))
                {
                    complex.file.CopyToAsync(stream);
                }

                var array = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "array.txt");
                System.IO.File.WriteAllLines(array, complex.array.Select(x => x.ToString()));

                var dateTimesFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "dateTimes.txt");
                System.IO.File.WriteAllLinesAsync(dateTimesFilePath, complex.dateTimes.Select(dt => dt.ToString("yyyy-MM-dd")));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
