using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using MyWebApi.Model;

namespace MyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
       public static readonly string conStr = "Server=LERA;Database=ado;Trusted_Connection=True;TrustServerCertificate=True;";
        [HttpGet, Route("GetStudents")]
        public ActionResult GetAllStudents()
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                var res = db.Query("pStudents", commandType: System.Data.CommandType.StoredProcedure);
                return Ok(res);
            }
        }
        [HttpPost, Route("InsertStudent")]
        public ActionResult InsertStudent(Students students)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@lastname", students.lastname);
                p.Add("@firstname", students.firstname);
                p.Add("@birthdate", students.birthdate);
                 p.Add("@Gender",   students.gender);
                db.Execute("pStudents;2", p, commandType: System.Data.CommandType.StoredProcedure);
                return Ok("Student added");
            }
        }
        [HttpPut, Route("Update")]
        public ActionResult UpdateStudent(Students students)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                DynamicParameters p = new DynamicParameters(students);
                db.Execute("pStudents;3", p, commandType: System.Data.CommandType.StoredProcedure);
                return Ok("Student updated");
            }
        }

        [HttpDelete, Route("Delete")]
        public ActionResult DeleteStudent(int id)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@id", id);
                db.Execute("pStudents;4", p, commandType: System.Data.CommandType.StoredProcedure);
                return Ok("Student deleted");
            }
        }

    }
}
