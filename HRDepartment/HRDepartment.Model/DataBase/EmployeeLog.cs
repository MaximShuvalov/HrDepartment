using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRDepartment.Model.DataBase
{
    public class EmployeeLog
    {
        [Key]
        public long Key { get; set; }
        [ForeignKey("Employee")]
        public long EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public bool Fired { get; set; }
        public DateTime DateOfDismissal { get; set; }
        [ForeignKey("Department")]
        public long DepartmentId { get; set; }
        public Department Department { get; set; }
        public string Position { get; set; }
    }
}