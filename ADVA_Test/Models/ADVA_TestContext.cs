using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADVA_Test.Models
{
    public class ADVA_TestContext:DbContext
    {public ADVA_TestContext(DbContextOptions option):base(option)
        {

        }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
    }
}
