using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public interface IEmployeeDBRepository
    {
     
        Task<List<Employee>> GetAllEmployees();
        // Return Type -> Function Name -> Parameters
        Employee GetEmployeeById(int EmployeeId);
        Employee AddEmployee(Employee Employee);
        Employee UpdateEmployee(int EmployeeId,Employee Employee);
        Employee DeleteEmployee(int EmployeeId);
        List<Department> FetchDepartmentList();
    }
}
