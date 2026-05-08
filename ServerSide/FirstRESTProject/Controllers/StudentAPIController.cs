using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FirstRESTProject.Model;
using FirstRESTProject.DataSemulation;
using System.Collections.Generic;

namespace FirstRESTProject.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Students")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllStudents")]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            return Ok(StudentDataSemulation.StudentsList);
        }

        [HttpGet("Passed", Name = "GetPassedStudents")]
        public ActionResult<IEnumerable<Student>> GetPassedStudents()
        {
            var passedStudents = StudentDataSemulation.StudentsList.Where(student => student.Grage >= 50);
            return Ok(passedStudents);
        }




    }
}
