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
        // private readonly EMSDBContext _repo;
        IEmployeeDBRepository _repo;

        public EmployeesController(IEmployeeDBRepository repo)
        {
            _repo = repo;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
           // var eMSDBContext = _repo.Employees.Include(e => e.Department);
            return View(await _repo.GetAllEmployees());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _repo.Employees == null)
            {
                return NotFound();
            }

            var employee = await _repo.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["DeptId"] = new SelectList(_repo.Department, "Id", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,EmailAdress,DOB,PhoneNumber,DeptId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(employee);
                await _repo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptId"] = new SelectList(_repo.Department, "Id", "Name", employee.DeptId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _repo.Employees == null)
            {
                return NotFound();
            }

            var employee = await _repo.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DeptId"] = new SelectList(_repo.Department, "Id", "Name", employee.DeptId);
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
                    _repo.Update(employee);
                    await _repo.SaveChangesAsync();
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
            ViewData["DeptId"] = new SelectList(_repo.Department, "Id", "Name", employee.DeptId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _repo.Employees == null)
            {
                return NotFound();
            }

            var employee = await _repo.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_repo.Employees == null)
            {
                return Problem("Entity set 'EMSDBContext.Employees'  is null.");
            }
            var employee = await _repo.Employees.FindAsync(id);
            if (employee != null)
            {
                _repo.Employees.Remove(employee);
            }
            
            await _repo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return (_repo.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
