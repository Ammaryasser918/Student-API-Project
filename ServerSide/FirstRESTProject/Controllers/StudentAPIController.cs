using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using StudentAPIDataAccessLayer;
using StudentAPIBusinessLayer;


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
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents()
        {
            List<StudentDTO> StudentList = clsStudentBusinessLayer.GetAllStduents();
            if (StudentList.Count == 0)
            {
                return NotFound("No Students Found");
            }
            return Ok(StudentList);
        }

        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetPassedStudents()
        {
            var StudentList = clsStudentBusinessLayer.GetPassedStudents();
            if ((StudentList.Count == 0) ||
                (StudentList.Where(student => student.Grade >= 50).ToList().Count == 0))
            {
                return NotFound("No Passed Students Found");
            }
            var passedStudents = StudentList.Where(student => student.Grade >= 50);
            return Ok(passedStudents);
        }


        [HttpGet("AverageGrade", Name = "GetAverageGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<double>> GetAverageGrade()
        {
            double Average = clsStudentBusinessLayer.GetAverageGrade();
            if (Average == 0)
            {
                return NotFound("No Students Found.");
            }
            return Ok(Average);
        }



        [HttpGet("StudentID/{StudentID}", Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> GetStudentByID(int StudentID)
        {
            if (StudentID < 1)
            {
                return BadRequest($"Not Accepted ID {StudentID}");
            }
            var Std = clsStudentBusinessLayer.GetStudentByID(StudentID);
            if (Std == null)
            {
                return NotFound("Student Not Found");
            }
            StudentDTO SDTO = Std.SDTO;
            return Ok(SDTO);
        }


        [HttpPost("NewStudent", Name = "NewStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> AddNewStudent(StudentDTO newStudentDTO)
        {
            if (newStudentDTO == null || string.IsNullOrEmpty(newStudentDTO.Name) || newStudentDTO.Age <= 0)
            {
                return BadRequest("Invalid Student Data.");
            }
            clsStudentBusinessLayer Std = new clsStudentBusinessLayer(new StudentDTO(newStudentDTO.ID, newStudentDTO.Name, newStudentDTO.Age, newStudentDTO.Grade), clsStudentBusinessLayer.enMode.AddNew);
            Std.Save();
            newStudentDTO.ID = Std.ID;

            return CreatedAtRoute($"GetStudentByID", new { StudentID = newStudentDTO.ID }, newStudentDTO);
        }


        [HttpDelete("DeleteStudent/{StudentID}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteStudent(int StudentID)
        {
            if (StudentID < 1)
            {
                return BadRequest($"Not Accepted ID {StudentID}");
            }
            clsStudentBusinessLayer Std = clsStudentBusinessLayer.GetStudentByID(StudentID);
            if (Std == null)
            {
                return NotFound($"Student With ID {StudentID} Not Found");
            }

            if (clsStudentBusinessLayer.DeleteStudent(StudentID))
            {
                return Ok($"Student With ID {StudentID} Deleted Successfully");
            }
            else
            {
                return NotFound($"Student with ID {StudentID} not found. no rows deleted!");
            }

        }

        [HttpPut("UpdateStudent/{id}", Name = "UpdateStudnet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> UpdateStudent(int id, StudentDTO updatedStudent)
        {
            if (id < 1 || updatedStudent == null || string.IsNullOrEmpty(updatedStudent.Name) || updatedStudent.Age < 0 || updatedStudent.Grade < 0)
            {
                return BadRequest("Bad Request: Not Accepted ID");
            }

            var Std = clsStudentBusinessLayer.GetStudentByID(id);
            if (Std == null)
            {
                return NotFound($"Student With ID {id} Not Found");
            }

            Std.Name = updatedStudent.Name;
            Std.Age = updatedStudent.Age;
            Std.Grade = updatedStudent.Grade;
            Std.Save();
            return Ok(Std.SDTO);

        }

        [HttpPost("UploadImage", Name = "UploadImage")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No File Uploaded.");
            }

            var uploadDictionary = @"D:\VS_projects\API\ServerSide\FirstRESTProject\MyUploads";

            var FileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadDictionary, FileName);

            if (!Directory.Exists(uploadDictionary))
            {
                Directory.CreateDirectory(uploadDictionary);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return Ok(new { filePath });

        }


    }
}
