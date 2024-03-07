using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Test.Models;

namespace Test.AppDbContext
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection") { }
        public DbSet<EmployeeModel> Employees{get;set;}
    }
}