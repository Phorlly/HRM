using AutoMapper;
using HRM.DTOs;
using HRM.Models;
using MailChimp.Net.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace HRM.Controllers.APIs.V1
{
    [RoutePrefix("api/v1/hr-employees")]
    public class EmployeesController : ApiController
    {
        protected readonly ApplicationDbContext connx;
        //===========================================Open Connection==================================//
        public EmployeesController()
        {
            connx = new ApplicationDbContext();
        }

        //===========================================Close Connection=================================//
        protected override void Dispose(bool disposing)
        {
            connx.Dispose();
        }

        //============================================GET: Data======================================//
        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetData()
        {
            try
            {
                var datas = connx.Employees.Select(Mapper.Map<Employee, EmployeeDto>).Where(c => c.Status.Equals(true)).ToList();
                if (datas == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new { info = datas, statusCode = 200, message = "Successfully Query All  Data.!" });
                }
            }
            catch (Exception except)
            {
                return BadRequest(except.Message);
            }
        }

        //============================================GET: Data by Id===============================//
        [HttpGet]
        [Route("get/{id}")]
        public IHttpActionResult GetDataById(int id)
        {
            try
            {
                var findById = connx.Employees.FirstOrDefault(c => c.Id.Equals(id) && c.Status.Equals(true));
                if (findById.Equals(null))
                {
                    return NotFound();
                }
                else
                {
                    var dataById = Mapper.Map<Employee, EmployeeDto>(findById);
                    return Ok(new { data = dataById, statusCode = 200, message = "Successfully Query One Data.!" });
                }
            }
            catch (Exception except)
            {

                return BadRequest(except.Message);
            }
        }

        //============================================POST: Data====================================//
        [HttpPost]
        [Route("post")]
        public IHttpActionResult PostData(EmployeeDto dataObject, HttpPostedFileBase imageFile)
        {
            try
            {
                //var first = HttpContext.Current.Request.Form["FirstName"];
                //var last = HttpContext.Current.Request.Form["LastName"];
                //var code = HttpContext.Current.Request.Form["Code"];
                //var sex = HttpContext.Current.Request.Form["Sex"];
                //var email = HttpContext.Current.Request.Form["Email"];
                //var phone = HttpContext.Current.Request.Form["Phone"];
                //var position = HttpContext.Current.Request.Form["Position"];
                //var address = HttpContext.Current.Request.Form["Address"]; 
                //var pob = HttpContext.Current.Request.Form["PlaceOfBirth"]; 
                //var photo = HttpContext.Current.Request.Files["Profile"];
                //var start = HttpContext.Current.Request.Form["StartedAt"];
                //var end = HttpContext.Current.Request.Form["EndedAt"];
                //var dob = HttpContext.Current.Request.Form["DateOfBirth"];
                //var age = HttpContext.Current.Request.Form["Age"];
                //var currencyId = HttpContext.Current.Request.Form["CurrencyId"];
                //var salary = HttpContext.Current.Request.Form["InitialSalary"];
                //var status = HttpContext.Current.Request.Form["Status"]; 

                //Data Exist not insert into Database
                var isExists = connx.Employees.SingleOrDefault(c => c.LastName.Equals(dataObject.LastName));
                if (isExists != null)
                {
                    return BadRequest("The Data is Already Exist in Database.!");
                }
                else
                {
                    dataObject.CreatedAt = DateTime.Now;
                    dataObject.Status = true;
                    // Save the uploaded image
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        var photoName = Path.Combine(Path.GetDirectoryName(imageFile.FileName)
                                 , string.Concat(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                 , DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss")
                                 , Path.GetExtension(imageFile.FileName)));

                        var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Images"), photoName);
                        imageFile.SaveAs(fileSavePath);

                        dataObject.Profile = photoName;
                    }

                    var currentDate = DateTime.Today;
                    dataObject.Age = currentDate.Year - dataObject.DateOfBirth.Year;

                    if (currentDate < dataObject.DateOfBirth.AddYears(dataObject.Age))
                    {
                        dataObject.Age--; // Adjust age if the current date is before the birth date in the current year
                    }

                    //Convert from Dto to Model
                    var add = Mapper.Map<EmployeeDto, Employee>(dataObject);

                    // Save the data to the database
                    connx.Employees.Add(add);
                    connx.SaveChanges();

                    // SaveData(data);
                    return Ok(new { data = add, statusCode = 200, message = "Successfully Created.!" });
                }
            }
            catch (Exception except)
            {

                return BadRequest(except.Message);
            }
        }

        //============================================PUT: Data by Id==============================//
        [HttpPut]
        [Route("put/{id}")]
        public IHttpActionResult PutDataById(EmployeeDto dataObject, int id)
        {
            try
            {
                var data = connx.Employees.SingleOrDefault(c => c.Id.Equals(id));
                if (data == null)
                {
                    return NotFound();
                }
                else
                {
                    if (data.LastName == dataObject.LastName​​​)
                    {
                        dataObject.UpdatedAt = DateTime.Now;
                        data.UpdatedAt = dataObject.UpdatedAt;
                        data.Status = dataObject.Status;
                        data.LastName = dataObject.LastName;
                        data.Code = dataObject.Code;
                        data.Email = dataObject.Email;
                        data.Status = dataObject.Status;

                        //Convert from Dto to Model
                        //var update = Mapper.Map(dataObject, data);

                        connx.SaveChanges();

                        return Ok(new { dataUpdated = data, statusCode = 200, message = "Successfully Only Data Updated.!" });
                    }

                    var isExist = connx.Employees.FirstOrDefault(c => c.LastName == dataObject.LastName);
                    if (isExist == null)
                    {
                        dataObject.UpdatedAt = DateTime.Now;
                        data.UpdatedAt = dataObject.UpdatedAt;
                        data.Status = dataObject.Status;
                        data.LastName = dataObject.LastName;
                        data.Code = dataObject.Code;
                        data.Email = dataObject.Email;

                        //Convert from Dto to Model
                        //var update = Mapper.Map(dataObject, data);

                        connx.SaveChanges();

                        return Ok(new { dataUpdated = data, statusCode = 200, message = "Successfully Other Data Updated.!" });
                    }
                    else
                    {

                        return BadRequest("The Data is Already Exist in Database !");
                    }
                }
            }
            catch (Exception except)
            {

                return BadRequest(except.Message);
            }
        }

        //============================================DELETE: Data by Id============================//
        [HttpDelete]
        [Route("delete/{id}")]
        public IHttpActionResult DeleteDataById(int id)
        {
            try
            {
                var data = connx.Employees.Find(id);
                if (data == null)
                {
                    return NotFound();
                }
                else
                {
                    //connx.Employees.Remove(data); 
                    data.Status = false;
                    data.DeletedAt = DateTime.Now;

                    connx.SaveChanges();

                    return Ok(new { statusCode = 200, message = "Successfully Disabled.!" });
                }
            }
            catch (Exception except)
            {
                return BadRequest(except.Message);
            }
        }
    }
}
