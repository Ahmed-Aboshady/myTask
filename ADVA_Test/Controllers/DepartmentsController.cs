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
    public class DepartmentsController : Controller
    {
        private readonly ADVA_TestContext _context;

        public DepartmentsController(ADVA_TestContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Departments.ToListAsync());
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                department.DeptID = Guid.NewGuid();
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(department);
        }

        public async Task<IActionResult> Edit(Guid id)
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
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Department department)
        {
            if (id != department.DeptID)
            {
                return RedirectToAction("Index");
            }
            var oldData = await _context.Departments.FirstOrDefaultAsync(emp => emp.DeptID == id);

            if (ModelState.IsValid)
            {
                _context.Entry(oldData).CurrentValues.SetValues(department);

                //_context.Departments.Update(department);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(department);
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }
            var department = await _context.Departments.FirstOrDefaultAsync(m => m.DeptID == id);
            if (department == null)
            {
                return RedirectToAction("Index");
            }
            List<Employee> emp = await _context.Employees.Where(n => n.DeptID == department.DeptID).ToListAsync();
            if (emp.Count != 0)
            {
                TempData["AlertMessage"] = "This is Department " + department.Name + "has Employees cannot Delete";
                return RedirectToAction("Index");
            }
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }
    }
}
