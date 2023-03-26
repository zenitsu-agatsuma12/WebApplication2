using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public interface IDepartmentDBRepository
    {
        List<Department> GetDepartments();
        Department GetDepartmentById(int id);
        Department AddDepartment(Department Department);
        Department UpdateDepartment(int DeptId, Department Department);
        Department DeleteDepartment(int DeptId);
    }
}
