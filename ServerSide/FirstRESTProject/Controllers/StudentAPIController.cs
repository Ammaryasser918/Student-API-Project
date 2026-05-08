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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            if (StudentDataSemulation.StudentsList.Count == 0)
            {
                return NotFound("No Students Found");
            }
            return Ok(StudentDataSemulation.StudentsList);
        }

        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Student>> GetPassedStudents()
        {
            if ((StudentDataSemulation.StudentsList.Count == 0) ||
                (StudentDataSemulation.StudentsList.Where(student => student.Grade >= 50).ToList().Count == 0))
            {
                return NotFound("No Passed Students Found");
            }
            var passedStudents = StudentDataSemulation.StudentsList.Where(student => student.Grade >= 50);
            return Ok(passedStudents);
        }


        [HttpGet("AverageGrade", Name = "GetAverageGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<double>> GetAverageGrade()
        {
            if (StudentDataSemulation.StudentsList.Count == 0)
            {
                return NotFound("No Students Found.");
            }
            var averageGrade = StudentDataSemulation.StudentsList.Average(student => student.Grade);
            return Ok(averageGrade);
        }



        [HttpGet("StudentID/{StudentID}", Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Student> GetStudentByID(int StudentID)
        {
            if (StudentID < 1)
            {
                return BadRequest($"Not Accepted ID {StudentID}");
            }
            var Std = StudentDataSemulation.StudentsList.FirstOrDefault(student => (student.ID == StudentID));
            if (Std == null)
            {
                return NotFound("Student Not Found");
            }
            return Ok(Std);
        }


        [HttpPost("NewStudent", Name = "NewStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Student> AddNewStudent(Student student)
        {
            if (student == null || string.IsNullOrEmpty(student.Name) || student.Age <= 0)
            {
                return BadRequest("Invalid Student Data.");
            }
            student.ID = StudentDataSemulation.StudentsList.Count > 0 ? StudentDataSemulation.StudentsList.Max(student => student.ID) + 1 : 1;
            StudentDataSemulation.StudentsList.Add(student);
            return CreatedAtRoute($"GetStudentByID", new { StudentID = student.ID }, student);
        }


        [HttpDelete("DeleteStudent/{StudentID}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteStudent(int StudentID)
        {
            if (StudentID < 1)
            {
                return BadRequest($"Not Accepted ID {StudentID}");
            }
            var student = StudentDataSemulation.StudentsList.FirstOrDefault(student => (student.ID == StudentID));
            if (student == null)
            {
                return NotFound($"Student With ID {StudentID} Not Found");
            }

            StudentDataSemulation.StudentsList.Remove(student);
            return Ok($"Student With ID {StudentID} Has been Deleted Successfully");

        }

        [HttpPut("UpdateStudent/{id}", Name = "UpdateStudnet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Student> UpdateStudent(int id, Student updatedStudent)
        {
            if (id < 1 || updatedStudent == null || string.IsNullOrEmpty(updatedStudent.Name) || updatedStudent.Age < 0 || updatedStudent.Grade < 0)
            {
                return BadRequest("Bad Request: Not Accepted ID");
            }
            var student = StudentDataSemulation.StudentsList.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound($"Student With ID {id} Not Found");
            }

            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.Grade = updatedStudent.Grade;
            return Ok(student);

        }



    }
}
