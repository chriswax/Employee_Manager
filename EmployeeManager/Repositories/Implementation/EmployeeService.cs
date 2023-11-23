using EmployeeManager.DAL;
using EmployeeManager.Models;
using EmployeeManager.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Repositories.Implementation
{
    public class EmployeeService : IEmployeeService
    {

        private readonly DBContext context;
        public EmployeeService(DBContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            var data = (from employees in context.Employees select employees).ToListAsync();
            return await data;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var employee = await context.Employees.FindAsync(id);
            return employee;
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            context.Employees.Add(employee);
            await context.SaveChangesAsync();
            return employee;    

        }
        public async Task<Employee> UpdateEmployeeAsync(int id, Employee employee)
        {
            var record = await GetEmployeeByIdAsync(id);
            if(record == null)
            {
                return record;
            }
            context.Entry(record).CurrentValues.SetValues(employee);
            await context.SaveChangesAsync();
            return employee;
        }
        public async Task<Employee> DeleteEmployeeAsync(int id)
        {
            var employee = await GetEmployeeByIdAsync(id);
            if(employee == null)
            {
                return employee;
            }
            context.Employees.Remove(employee);
            await context.SaveChangesAsync(true);
            return employee;
        }


    }
}
