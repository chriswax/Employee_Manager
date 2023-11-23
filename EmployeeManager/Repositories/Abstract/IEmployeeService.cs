using EmployeeManager.Models;

namespace EmployeeManager.Repositories.Abstract
{
    public interface IEmployeeService
    {

        Task<IEnumerable<Employee>> GetEmployeesAsync();

        Task<Employee> GetEmployeeByIdAsync(int id);

        Task<Employee> AddEmployeeAsync(Employee employee);

        Task<Employee> DeleteEmployeeAsync(int id);

        Task<Employee> UpdateEmployeeAsync(int id, Employee employee);

    }
}
