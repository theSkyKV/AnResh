﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnResh.Controllers
{
    public class DepartmentsController : Controller
    {
        // GET: Departments
        public ActionResult Index()
        {
            return View();
        }
    }
}