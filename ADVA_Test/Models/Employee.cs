using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ADVA_Test.Models
{
    public class Employee
    {
        [Key]
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Please fill this field")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Range(1000, 100000)]
        public decimal Salary { get; set; }
        public Guid DeptID { get; set; }

        [ForeignKey(nameof(DeptID))]
        public virtual Department Dept { get; set; }
        public bool IsManager { get; set; }
        [NotMapped]
        public Employee Manager { get; set; }

    }
}
