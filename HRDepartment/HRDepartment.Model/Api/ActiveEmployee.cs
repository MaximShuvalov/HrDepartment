using System.Collections.Generic;

namespace HRDepartment.Model.Api
{
    public class ActiveEmployee
    {
        public string Fio { get; set; }
        public List<Department> Departments { get; set; }
        public string PhoneNumber { get; set; }
    }
}