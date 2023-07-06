using AutoMapper;
using HRM.DTOs;
using HRM.Helpers;
using HRM.Models;
using MailChimp.Net.Core;
using MailChimp.Net.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Razor.Parser.SyntaxTree;

namespace HRM.Controllers.APIs
{
    [RoutePrefix("api/hr-user")]
    public class UsersController : ApiController
    {
        protected readonly ApplicationDbContext connx;
        //===========================================Open Connection==================================//
        public UsersController()
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
        public async Task<IHttpActionResult> GetData()
        {
            try
            {
                var datas = connx.Users.Select(Mapper.Map<User, UserDto>).ToList();
                if (datas == null)
                {
                    await Task.Delay(1000);
                    return NotFound();
                }
                else
                {
                    await Task.Delay(1000);
                    return Ok(new { info = datas, statusCode = 200, message = "Successfully Query All  Data.!" });
                }
            }
            catch (Exception except)
            {
                await Task.Delay(1000);
                return BadRequest(except.Message);
            }
        }

        //============================================GET: Data by Id===============================//
        [HttpGet]
        [Route("get/{id}")]
        public async Task<IHttpActionResult> GetDataById(int id)
        {
            try
            {
                var findById = connx.Users.Find(id);
                if (findById.Equals(null))
                {
                    await Task.Delay(1000);
                    return NotFound();
                }
                else
                {
                    await Task.Delay(1000);
                    var dataById = Mapper.Map<User, UserDto>(findById);
                    return Ok(new { data = dataById, statusCode = 200, message = "Successfully Query One Data.!" });
                }
            }
            catch (Exception except)
            {
                await Task.Delay(1000);
                return BadRequest(except.Message);
            }
        }

        //============================================POST: Data====================================//
        [HttpPost]
        [Route("post")]
        public async Task<IHttpActionResult> PostData()
        {
            try
            {
                var username = HttpContext.Current.Request.Form["Username"];
                var sex = HttpContext.Current.Request.Form["Gender"];
                var email = HttpContext.Current.Request.Form["Email"];
                var address = HttpContext.Current.Request.Form["Address"];
                var phone = HttpContext.Current.Request.Form["Phone"];
                var password = HttpContext.Current.Request.Form["Password"];
                var photo = HttpContext.Current.Request.Files["Photo"];

                //Data Exist not insert into Database
                var isExists = connx.Users.SingleOrDefault(c => c.Username.Equals(username));
                if (isExists != null)
                {
                    await Task.Delay(1000);
                    return BadRequest("The Data is Already Exist in Database.!");
                }
                else
                {
                    // Simulate an asynchronous database operation
                    await Task.Delay(1000);

                    var photoName = "";
                    if (photo != null && photo.ContentLength > 0)
                    {
                        photoName = Path.Combine(Path.GetDirectoryName(photo.FileName)
                                  , string.Concat(Path.GetFileNameWithoutExtension(photo.FileName)
                                  , DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss")
                                  , Path.GetExtension(photo.FileName)));

                        var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Images"), photoName);
                        photo.SaveAs(fileSavePath);
                    }

                    //==========combine field into object of Model==============================//
                    var dataObject = new UserDto()
                    {
                        Username = username,
                        Email = email,
                        Address = address,
                        Phone = phone,
                        IsAdmin = false,
                        Status = true,
                        CreatedAt = DateTime.Now,
                        Gender = bool.Parse(sex),
                        //Encypt password
                        Password = EncryptPassword.ComputeHash(password, "SHA512", null),
                        Photo = photoName
                    };

                    //Convert from Dto to Model
                    var data = Mapper.Map<UserDto, User>(dataObject);

                    // Save the data to the database
                    connx.Users.Add(data);
                    await connx.SaveChangesAsync();

                    //await SaveData(data);
                    return Ok(new { data = data, statusCode = 200, message = "Successfully Created.!" });
                }
            }
            catch (Exception except)
            {
                await Task.Delay(1000);
                return BadRequest(except.Message);
            }
        }

        //============================================PUT: Data by Id==============================//
        [HttpPut]
        [Route("put/{id}")]
        public async Task<IHttpActionResult> PutDataById(int id)
        {
            try
            {
                var username = HttpContext.Current.Request.Form["Username"];
                var sex = HttpContext.Current.Request.Form["Gender"];
                var email = HttpContext.Current.Request.Form["Email"];
                var address = HttpContext.Current.Request.Form["Address"];
                var phone = HttpContext.Current.Request.Form["Phone"];
                var password = HttpContext.Current.Request.Form["Password"];
                var photo = HttpContext.Current.Request.Files["Photo"];
                var status = HttpContext.Current.Request.Form["Status"];
                var isAdmin = HttpContext.Current.Request.Form["IsAdmin"];

                var data = await connx.Users.FindAsync(id);
                if (data.Equals(null))
                {
                    await Task.Delay(1000);
                    return NotFound();
                }
                else
                {
                    var photoName = "";
                    if (photo != null && photo.ContentLength > 0)
                    {
                        photoName = Path.Combine(Path.GetDirectoryName(photo.FileName)
                                  , string.Concat(Path.GetFileNameWithoutExtension(photo.FileName)
                                  , DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss")
                                  , Path.GetExtension(photo.FileName)));

                        var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Images"), photoName);
                        photo.SaveAs(fileSavePath);

                        //Delete Old Image
                        string oldPhotoPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Images"), data.Photo);
                        if (File.Exists(oldPhotoPath))
                        {
                            File.Delete(oldPhotoPath);
                        }

                        //==========combine field into object of Model==============================//
                        var dataObject = new UserDto()
                        {
                            Username = username,
                            Email = email,
                            Address = address,
                            Phone = phone,
                            IsAdmin = bool.Parse(isAdmin),
                            //Status = bool.Parse(status),
                            //IsAdmin = false,
                            Status = true,
                            UpdatedAt = DateTime.Now,
                            CreatedAt = data.CreatedAt,
                            Gender = bool.Parse(sex),
                            //Encypt password
                            Password = EncryptPassword.ComputeHash(password, "SHA512", null),
                            Photo = photoName
                        };
                        if (data.Username == username​​​)
                        {
                            // Simulate an asynchronous database operation
                            await Task.Delay(1000);

                            //Convert from Dto to Model
                            var dataUpdate = Mapper.Map(dataObject, data);

                            // Save the data to the database
                            await connx.SaveChangesAsync();

                            return Ok(new { data = dataUpdate, statusCode = 200, message = "Successfully Only Data Updated.!" });
                        }

                        var isExist = await connx.Users.FirstOrDefaultAsync(c => c.Username == username);
                        if (isExist == null)
                        {
                            // Simulate an asynchronous database operation
                            await Task.Delay(1000);

                            //Convert from Dto to Model
                            var dataUpdate = Mapper.Map(dataObject, data);

                            // Save the data to the database
                            await connx.SaveChangesAsync();

                            return Ok(new { data = dataUpdate, statusCode = 200, message = "Successfully Other Data Updated.!" });
                        }
                        else
                        {
                            await Task.Delay(1000);
                            return BadRequest("The Data is Already Exist in Database !");
                        }
                    }
                    else
                    {
                        //==========combine field into object of Model==============================//
                        var dataObject = new UserDto()
                        {
                            Username = username,
                            Email = email,
                            Address = address,
                            Phone = phone,
                            IsAdmin = bool.Parse(isAdmin),
                            //Status = bool.Parse(status),
                            //IsAdmin = false,
                            Status = true,
                            UpdatedAt = DateTime.Now,
                            CreatedAt = data.CreatedAt,
                            Gender = bool.Parse(sex),
                            //Encypt password
                            Password = EncryptPassword.ComputeHash(password, "SHA512", null),
                            Photo = data.Photo
                        };
                        if (data.Username == username​​​)
                        {
                            // Simulate an asynchronous database operation
                            await Task.Delay(1000);

                            //Convert from Dto to Model
                            var dataUpdate = Mapper.Map(dataObject, data);

                            // Save the data to the database
                            await connx.SaveChangesAsync();

                            return Ok(new { data = dataUpdate, statusCode = 200, message = "Successfully Only Data Updated.!" });
                        }

                        var isExist = await connx.Users.FirstOrDefaultAsync(c => c.Username == username);
                        if (isExist == null)
                        {
                            // Simulate an asynchronous database operation
                            await Task.Delay(1000);

                            //Convert from Dto to Model
                            var dataUpdate = Mapper.Map(dataObject, data);

                            // Save the data to the database
                            await connx.SaveChangesAsync();

                            return Ok(new { data = dataUpdate, statusCode = 200, message = "Successfully Other Data Updated.!" });
                        }
                        else
                        {
                            await Task.Delay(1000);
                            return BadRequest("The Data is Already Exist in Database !");
                        }
                    }
                }
            }
            catch (Exception except)
            {
                await Task.Delay(1000);
                return BadRequest(except.Message);
            }
        }

        //============================================DELETE: Data by Id============================//
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IHttpActionResult> DeleteDataById(int id)
        {
            try
            {
                var data = await connx.Users.FindAsync(id);
                if (data == null)
                {
                    await Task.Delay(1000);
                    return NotFound();
                }
                else
                {
                    // Simulate an asynchronous database operation
                    await Task.Delay(1000);

                    //connx.Users.Remove(data); 
                    data.Status = false;
                    //var distroyedAt = data.DeletedAt = DateTime.Now;

                    await connx.SaveChangesAsync();

                    return Ok(new {statusCode = 200, message = "Successfully Disabled.!" });
                }
            }
            catch (Exception except)
            {
                await Task.Delay(1000);
                return BadRequest(except.Message);
            }
        }
    }
}
