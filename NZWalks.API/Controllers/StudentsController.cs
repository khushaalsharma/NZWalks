using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//each controller must have "Controller" as a suffix in its name else ASP compiler won't recognise it

namespace NZWalks.API.Controllers
{
    //this API URL : http://localhost:5283/api/students students as it is the controller name
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //GET : http://localhost:5283/api/students
        [HttpGet] //this ensures that when this API is called with GET endpoint following function is executed
        public IActionResult GetAllStudents()
        {
            //say following is retrieved from database
            string[] students = new string[] { "John", "Jane", "Mark", "Emily", "David" };

            return Ok(students); //this returns  a 200 response with the array of student names
            
        }
    }
}
