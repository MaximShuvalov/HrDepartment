﻿using System.Collections.Generic;

namespace HRDepartment.Model.Api
{
    public class DepartmentFullInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<ActiveEmployee> Employees { get; set; }
        public Employee Boss { get; set; }
    }
}