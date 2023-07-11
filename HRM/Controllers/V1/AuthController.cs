using HRM.Helpers;
using HRM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace HRM.Controllers.V1
{
    public class AuthController : Controller
    {

        protected ApplicationDbContext context;

        //Open Connection
        public AuthController()
        {
            context = new ApplicationDbContext();
        }

        //Close Connection
        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }

        //==========================================LogOut for System============================//
        [HttpGet]
        public JsonResult LogOut()
        {
            try
            {
                //Session.Abandon();
                //Response.Cookies.Clear();
                //FormsAuthentication.SignOut();
                Session.Clear();
                return Json(new { success = true, message = "Sign Out is Successfully !" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }

        //==========================================LogIn into system=========================//
        [HttpGet]
        public ActionResult LogIn()
        {
            if (Session["userId"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //===============================LogIn into system=========================//
        [HttpPost]
        public JsonResult LogIn(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = context.Users.FirstOrDefault(c => c.Email.Equals(user.Email) && c.Status.Equals(true));
                    if (account != null)
                    {
                        //Verify password encyption
                        if (EncryptPassword.VerifyHash(user.Password, "SHA512", account.Password))
                        {
                            //var username = new HttpCookie("Username", account.Username);
                            //username.Expires = DateTime.Now.AddDays(7);
                            //Response.Cookies.Add(username);
                            //Response.Cookies["Username"].Value = account.Username;
                            //FormsAuthentication.SetAuthCookie(user.Username, false);

                            Session["userName"] = account.Username.ToString();
                            Session["userId"] = account.Id.ToString();
                            Session["photo"] = account.Photo.ToString();
                            Session["createdAt"] = account.CreatedAt.ToString();
                            Session["emailAddess"] = account.Email.ToString();
                            Session["isAdmin"] = account.IsAdmin;
                            Session["phone"] = account.Phone.ToString();
                            Session["sex"] = account.Gender;
                            Session["address"] = account.Address.ToString();
                            Session["status"] = account.Status;

                            return Json(new { success = true, message = "Sign In is Successfully !" });
                        }
                        else
                        {
                            return Json(new { message = "Email and Password is not match !" });
                        }
                    }
                    else
                    {
                        return Json(new { message = "Email Not found in Database !" });
                    }

                }
                catch (Exception err)
                {
                    return Json(new { message = err.Message.ToString() });
                }
            }

            return Json(user);
        }

        //===============================Register account=============================//
        [HttpGet]
        [Route("create-account")]
        public ActionResult Register()
        {
            if (Session["userId"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //===============================Register account=============================//
        [HttpPost]
        public JsonResult Register(User user, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //Data Exist not insert into Database
                    var checkUser = context.Users.SingleOrDefault(c => c.Email.Equals(user.Email));
                    if (checkUser == null)
                    {
                        // Save the uploaded image
                        if (imageFile != null && imageFile.ContentLength > 0)
                        {
                            string imagePath = Path.GetFileNameWithoutExtension(imageFile.FileName);
                            string extension = Path.GetExtension(imageFile.FileName);
                            imagePath = imagePath + DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss") + extension;
                            var savePath = Path.Combine(Server.MapPath("~/Images/"), imagePath);
                            imageFile.SaveAs(savePath);

                            user.Photo = imagePath;
                        }

                        //Encypt password
                        user.Password = EncryptPassword.ComputeHash(user.Password, "SHA512", null);
                        user.Status = true;
                        user.IsAdmin = false;
                        user.CreatedAt = DateTime.Now;

                        context.Users.Add(user);
                        context.SaveChanges();

                        // Return a JSON response indicating success or failure
                        return Json(new { success = true, message = "User Account Created Successfully !" });
                    }
                    else
                    {
                        return Json(new { message = "Email Already Exixts in Database !" });
                    }

                }
                catch (Exception err)
                {
                    return Json(new { message = err.Message.ToString() });
                }

            }

            return Json(user);
        }
    }
}