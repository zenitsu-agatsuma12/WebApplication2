using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Repositories.MSSQL
{
    public class EmployeeDBRepository : IEmployeeDBRepository
    {
        EMSDBContext _EMSdbContext;
        public EmployeeDBRepository(EMSDBContext EMSDBContext)
        {
            _EMSdbContext = EMSDBContext;
        }
        public Employee AddEmployee(Employee Employee)
        {
            _EMSdbContext.Employees.Add(Employee);
            _EMSdbContext.SaveChanges();
            return Employee;
        }

        public Employee DeleteEmployee(int EmployeeId)
        {
            var oldEmployee = GetEmployeeById(EmployeeId);
            if (oldEmployee != null)
            {
                _EMSdbContext.Employees.Remove(oldEmployee);
                _EMSdbContext.SaveChanges();
                return oldEmployee;
            }
            return null;
        }

        public Task <List<Employee>> GetAllEmployees()
        {
            return Task.FromResult(_EMSdbContext.Employees.Include(x => x.Department).ToList());
        }

        public Employee GetEmployeeById(int EmployeeId)
        {
            return _EMSdbContext.Employees.AsNoTracking().FirstOrDefault(x => x.Id == EmployeeId);
        }

        public Employee UpdateEmployee(int EmployeeId, Employee Employee)
        {
            _EMSdbContext.Employees.Update(Employee);
            _EMSdbContext.SaveChanges();
            return Employee;
        }

        public List<Department> FetchDepartmentList()
        {
            return _EMSdbContext.Department.ToList();
        }
    }
}
