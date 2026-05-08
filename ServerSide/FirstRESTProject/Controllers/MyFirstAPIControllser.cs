using FirstRESTProject.DataSemulation;
using FirstRESTProject.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstRESTProject.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/MyFirstAPI")]
    [ApiController]
    public class MyFirstAPIControllser : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            return Ok(StudentDataSemulation.StudentsList);
        }

    }
}
