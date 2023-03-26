using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Repositories.MSSQL
{
    public class DepartmentDBRepository : IDepartmentDBRepository
    {
        //ctor
        EMSDBContext _EMSDBContext;
        public DepartmentDBRepository(EMSDBContext EMSDBContext)
        {
            _EMSDBContext = EMSDBContext;
        }

        public Department AddDepartment(Department Department)
        {
            _EMSDBContext.Department.Add(Department);
            _EMSDBContext.SaveChanges();
            return Department;
        }

        public Department DeleteDepartment(int DeptId)
        {
            var oldDepartment = GetDepartmentById(DeptId);
            if (oldDepartment != null)
            {
                _EMSDBContext.Department.Remove(oldDepartment);
                _EMSDBContext.SaveChanges();
                return oldDepartment;
            }
            return null;
        }

        public Department GetDepartmentById(int id)
        {
            return _EMSDBContext.Department.AsNoTracking().FirstOrDefault(x => x.Id == id);
        }

        public List<Department> GetDepartments()
        {
            return _EMSDBContext.Department.ToList();
        }

        public Department UpdateDepartment(int DeptId, Department Department)
        {
            _EMSDBContext.Department.Update(Department);
            _EMSDBContext.SaveChanges();
            return Department;
        }
    }
}
