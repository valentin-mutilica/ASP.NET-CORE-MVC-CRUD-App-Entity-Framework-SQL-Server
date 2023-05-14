using Examen.Data;
using Examen.Models;
using Examen.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Examen.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDbContext mvcdbcontext;

        public EmployeesController(MVCDbContext mvcdbcontext)
        {
            this.mvcdbcontext = mvcdbcontext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employee = await mvcdbcontext.Employees.ToListAsync();
            return View(employee);
        }



        //Methods to add new employees
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth
            };

            await mvcdbcontext.Employees.AddAsync(employee);
            await mvcdbcontext.SaveChangesAsync();
            return RedirectToAction("Index");
        }




        //Methods to update employees
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await mvcdbcontext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            
            if (employee != null)
            {

                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth
                };

                return await Task.Run(() => View("View", viewModel));
            }

            return RedirectToAction("Index");

        }

        [HttpPost]

        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await mvcdbcontext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

                await mvcdbcontext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }


        //Method to delete employees
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await mvcdbcontext.Employees.FindAsync(model.Id);

            if (employee != null) {
                mvcdbcontext.Employees.Remove(employee);

                await mvcdbcontext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
