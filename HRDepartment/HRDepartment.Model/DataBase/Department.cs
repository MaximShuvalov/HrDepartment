using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRDepartment.Model
{
    public class Department
    {
        [Key]
        public long Key { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Employee> Employees { get; set; }
        [ForeignKey("Boss")] 
        public long BossId { get; set; }
        public Employee Boss { get; set; }
    }
}