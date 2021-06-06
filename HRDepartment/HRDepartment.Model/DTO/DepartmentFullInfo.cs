using System.Collections.Generic;
using HRDepartment.Model.DataBase;

namespace HRDepartment.Model.DTO
{
    public class DepartmentFullInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<ActiveEmployee> Employees { get; set; }
        public Employee Boss { get; set; }
    }
}