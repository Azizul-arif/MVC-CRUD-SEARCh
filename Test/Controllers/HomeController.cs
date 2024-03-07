using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;
using Test.AppDbContext;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index()
        {
            var employeedata = GetEmployeeData();
            return View(employeedata);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(EmployeeModel employee)
        {
            if (ModelState.IsValid)
            {
                var context = new ApplicationDbContext();
                context.Employees.Add(employee);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);

        }

        public ActionResult Details(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var employee = context.Employees.Find(id);
                if (employee == null)
                {
                    return HttpNotFound();
                }
                return View(employee);
            }
        }
        public ActionResult Edit(int id)
        {
            using (context)
            {
                var employee = context.Employees.Find(id);
                if (employee == null)
                {
                    return HttpNotFound();
                }
                return View(employee);

            }
        }
        [HttpPost]
        public ActionResult Edit(EmployeeModel employee)
        {
            if (ModelState.IsValid)
            {
                var existingEmployee = context.Employees.Find(employee.Id);

                if (existingEmployee != null)
                {
                    existingEmployee.Name = employee.Name;
                    existingEmployee.Designation = employee.Designation;
                    existingEmployee.age = employee.age;
                    existingEmployee.Salary = employee.Salary;

                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(employee);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var deleteEmployee = context.Employees.Find(id);
                if (deleteEmployee != null)
                {
                    context.Employees.Remove(deleteEmployee);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public List<EmployeeModel> GetEmployeeData()
        {
            using (context)
            {
                var employees = context.Employees.ToList();
                return employees;
            }

        }

        public ActionResult Search(string searchTerm)
        {
            var searchResults = GetEmployeeData(searchTerm);
            return View("Index", searchResults);
        }


        private List<EmployeeModel> GetEmployeeData(string searchTerm = "")
        {
            using (var context = new ApplicationDbContext())
            {
                var employees = context.Employees.ToList();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    employees = employees.Where(e =>
                        e.Name.ToLower().Contains(searchTerm) ||
                        e.Designation.ToLower().Contains(searchTerm) ||
                        e.age.ToString().Contains(searchTerm) ||
                        e.Salary.ToString().Contains(searchTerm)
                    ).ToList();


                }

                return employees;
            }
        }

        public ActionResult SearchExact(string searchExact)
        {
            var searchTermWithoutSpaces = searchExact != null ? searchExact.Trim() : null;
            var searchResult = GetEmployeeDataExact(searchTermWithoutSpaces);
            return View("Index", searchResult);
        }

        private List<EmployeeModel> GetEmployeeDataExact(string searchExact)
        {
            using (context)
            {
                var employeeList = context.Employees.ToList();
                if (!string.IsNullOrEmpty(searchExact))
                {
                    employeeList = employeeList.Where(
                        e => e.Name.Equals(searchExact, StringComparison.OrdinalIgnoreCase) ||
                            e.Designation.Equals(searchExact, StringComparison.OrdinalIgnoreCase) ||
                             e.age.ToString() == searchExact ||
                             e.Salary.ToString() == searchExact).ToList();
                }
                return employeeList;
            }

        }

    }
}
