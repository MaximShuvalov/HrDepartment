using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRDepartment.Model
{
    public class Employee
    {
        [Key] 
        public long Key { get; set; }
        public string Fio { get; set; }
        public List<EmployeeLog> EmployeeLogs { get; set; } = new List<EmployeeLog>();
        public string PhoneNumber { get; set; }
    }
}