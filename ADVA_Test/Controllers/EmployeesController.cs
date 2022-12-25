using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ADVA_Test.Models;

namespace ADVA_Test.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ADVA_TestContext _context;

        public EmployeesController(ADVA_TestContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var Getemployees = _context.Employees.Include(e => e.Dept)
                .Select(emp => new Employee
                {
                    ID = emp.ID,
                    DeptID = emp.DeptID,
                    Dept = emp.Dept,
                    IsManager = emp.IsManager,
                    Name = emp.Name,
                    Salary = emp.Salary,
                    Manager = _context.Employees.FirstOrDefault(man => man.DeptID == emp.DeptID && man.IsManager)
                });
            return View(await Getemployees.ToListAsync());
        }


        public async Task<IActionResult> GetEmployeesbyDepartment(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return RedirectToAction("Index");
            }
            var Getemployees = await _context.Employees.Where(e => e.DeptID == id).ToListAsync();

            var manager = await _context.Employees.FirstOrDefaultAsync(emp => emp.DeptID == id && emp.IsManager);
            ViewData["manager"] = manager == null ? "No Manager Available" : manager.Name;
            ViewData["DeptName"] = department.Name;
            if (Getemployees.Count == 0)
            {
                TempData["AlertMessage"] = "This is Department hasn't employees";
                return RedirectToAction("Index", "Departments");
            }
            return View(Getemployees);
        }


        public IActionResult Create()
        {
            ViewData["DeptID"] = new SelectList(_context.Departments, "DeptID", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.ID = Guid.NewGuid();
                var CheckbyManager = await _context.Employees.CountAsync(s => s.DeptID == employee.DeptID && employee.IsManager == true);
                if (CheckbyManager > 0 && employee.IsManager)
                {
                    TempData["AlertMessage"] = "This is Department has Manager";

                    return RedirectToAction("Create");
                }
                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["DeptID"] = new SelectList(_context.Departments, "DeptID", "Name", employee.DeptID);
            return View(employee);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            ViewData["DeptID"] = new SelectList(_context.Departments, "DeptID", "Name", employee.DeptID);
            return View(employee);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Employee employee)
        {
            if (id != employee.ID)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var oldData = await _context.Employees.FirstOrDefaultAsync(emp => emp.ID == id);
                if (oldData == null) return NotFound();

                if (oldData.IsManager != employee.IsManager && employee.IsManager)
                {
                    var CheckbyManager = await _context.Employees.CountAsync(s => s.DeptID == employee.DeptID && s.IsManager == true);
                    if (CheckbyManager > 0)
                    {
                        TempData["AlertMessage"] = "This is Department has Manager";
                        return View(oldData);
                    }
                }

                _context.Entry(oldData).CurrentValues.SetValues(employee);
                //  _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["DeptID"] = new SelectList(_context.Departments, "DeptID", "Name", employee.Name);
            return View(employee);
        }
        public async Task<IActionResult> Delete(Guid id)
        {

            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return RedirectToAction("Index");
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
