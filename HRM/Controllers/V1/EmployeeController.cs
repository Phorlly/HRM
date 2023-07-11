using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Controllers.V1
{
    public class EmployeeController : Controller
    {
        // GET: Employees
        public ActionResult Index()
        {
            return View();
        }
    }
}