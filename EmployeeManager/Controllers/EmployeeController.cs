using EmployeeManager.Models;
using EmployeeManager.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Controllers
{
    [Authorize]   //   [Authorize(Roles = UserRole.Admin)] ///use this for role base access
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await employeeService.GetEmployeesAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute] int id)
        {
            var record = await employeeService.GetEmployeeByIdAsync(id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

          [HttpPost]
   public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
   {
       try
       {
           await employeeService.AddEmployeeAsync(employee);

           return Ok(employee);
       }
       catch (Exception ex)
       {
           Console.Error.WriteLine($"An error occurred: {ex.Message}");
           return StatusCode(500, new { Message = "An error occurred while processing the request." });
       }
   }
      


        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }
            var UpdateRecord = await employeeService.UpdateEmployeeAsync(id, employee);
            if (UpdateRecord == null)
            {
                return NotFound();
            }
            return Ok(UpdateRecord);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            var record = await employeeService.DeleteEmployeeAsync(id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }
    }
}
