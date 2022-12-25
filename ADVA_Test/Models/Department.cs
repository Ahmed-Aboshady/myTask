using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ADVA_Test.Models
{
    public class Department
    {
        [Key]
        public Guid DeptID { get; set; }
        [Required(ErrorMessage = "Please fill this field")]
        public string Name { get; set; }
    }
}
