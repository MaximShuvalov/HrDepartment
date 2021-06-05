using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRDepartment.Model
{
    public class Employee
    {
        [Key]
        public long Key { get; set; }
        public string Fio { get; set; }
        public List<Department> Departments { get; set; }
        public string PhoneNumber { get; set; }
        public bool Fired { get; set; }
        public DateTime DateOfDismissal { get; set; }
    }
}