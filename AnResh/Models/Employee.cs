﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnResh.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public int Salary { get; set; }
    }
}