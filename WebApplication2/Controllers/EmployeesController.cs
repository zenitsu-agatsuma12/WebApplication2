using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers
{
    public class EmployeesController : Controller
    {
        // private readonly EMSDBContext _employeesRepository;
        IEmployeeDBRepository _employeesRepository;
        IDepartmentDBRepository _departmentRepository;

        public EmployeesController(IEmployeeDBRepository employeesRepository, IDepartmentDBRepository departmentRepository)
        {
            _employeesRepository = employeesRepository;
            _departmentRepository = departmentRepository;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
           // var eMSDBContext = _repo.Employees.Include(e => e.Department);
            return View(await _employeesRepository.GetAllEmployees());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeesRepository.GetEmployeeById(id)

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create([Bind("")])
        {
            CreateEmployeeViewModel createEmployeeViewModel = new CreateEmployeeViewModel {
                Departments = await _departmentRepository.GetDepartments()
            };

            return View(createEmployeeViewModel);
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (CreateEmployeeViewModel createEmployeeViewModel)
        {
            if (ModelState.IsValid)
            {
                Employee newEmployee = new Employee {
                    FullName = createEmployeeViewModel.NewEmployee.FullName,
                    EmailAddress = createEmployeeViewModel.NewEmployee.FullName,
                    DOB = createEmployeeViewModel.NewEmployee.FullName,
                    PhoneNumber = createEmployeeViewModel.NewEmployee.FullName,
                    DeptId = createEmployeeViewModel.NewEmployee.DeptId
                }

                await _employeesRepository.AddEmployee(newEmployee);

                return RedirectToAction(nameof(Index));
            }

            CreateEmployeeViewModel createEmployeeViewModel = new CreateEmployeeViewModel {
                Departments = _departmentRepository.GetDepartments()
            };

            return View(createEmployeeViewModel);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _employeesRepository.Employees == null)
            {
                return NotFound();
            }

            var employee = await _employeesRepository.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DeptId"] = new SelectList(_employeesRepository.Department, "Id", "Name", employee.DeptId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,EmailAdress,DOB,PhoneNumber,DeptId")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _employeesRepository.Update(employee);
                    await _employeesRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptId"] = new SelectList(_employeesRepository.Department, "Id", "Name", employee.DeptId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeesRepository.FindEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_employeesRepository.GetAllEmployees() == null)
            {
                return Problem("Entity set 'EMSDBContext.Employees'  is null.");
            }

            var employee = await _employeesRepository.GetEmployeeById(id);
            if (employee != null)
            {
               await _employeesRepository.DeleteEmployee(employee);
            }
            
            await _employeesRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return (_employeesRepository.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
