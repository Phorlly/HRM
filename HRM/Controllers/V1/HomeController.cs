using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Controllers.V1
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                if (Session["userId"] == null)
                {
                    return RedirectToAction("LogIn", "Auth");
                }
            }
            catch
            {
                return RedirectToAction("LogIn", "Auth");
            }

            return View();
        }
    }
}