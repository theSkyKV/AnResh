﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnResh.Models;

namespace AnResh.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        public ActionResult Index(int id = 0)
        {
            return View(DapperORM.GetAllItemsById<Employee>(id));
        }
    }
}