using AutoMapper;
using HRM.DTOs;
using HRM.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HRM.Controllers.APIs
{
    [RoutePrefix("api/hr-currency")]
    public class CurrenciesController : ApiController
    {
        protected readonly ApplicationDbContext connx;
        //===========================================Open Connection==================================//
        public CurrenciesController()
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
                var datas = connx.Currencies.Select(Mapper.Map<Currency, CurrencyDto>).Where(c=>c.Status.Equals(true)).ToList();
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
                var findById = connx.Currencies.FirstOrDefault(c=>c.Id.Equals(id) && c.Status.Equals(true));
                if (findById.Equals(null))
                {
                    return NotFound();
                }
                else
                {
                    var dataById = Mapper.Map<Currency, CurrencyDto>(findById);
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
        public IHttpActionResult PostData(CurrencyDto dataObject)
        {
            try
            {
                //Data Exist not insert into Database
                var isExists = connx.Currencies.SingleOrDefault(c => c.Name.Equals(dataObject.Name));
                if (isExists != null)
                {
                    return BadRequest("The Data is Already Exist in Database !");
                }
                else
                {
                    dataObject.CreatedAt = DateTime.Now;
                    dataObject.Status = true;

                    //Convert from Dto to Model
                    var add = Mapper.Map<CurrencyDto, Currency>(dataObject);

                    // Save the data to the database
                    connx.Currencies.Add(add);
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
        public IHttpActionResult PutDataById(CurrencyDto dataObject, int id)
        {
            try
            {
                var data = connx.Currencies.SingleOrDefault(c => c.Id.Equals(id));
                if (data == null)
                {
                    return NotFound();
                }
                else
                {
                    if (data.Name == dataObject.Name​​​)
                    {
                        dataObject.UpdatedAt = DateTime.Now;
                        data.UpdatedAt = dataObject.UpdatedAt;
                        data.Status = dataObject.Status;
                        data.Name = dataObject.Name;
                        data.Code = dataObject.Code;
                        data.Symbol = dataObject.Symbol;
                        data.Status = dataObject.Status;

                        //Convert from Dto to Model
                        //var update = Mapper.Map(dataObject, data);

                        connx.SaveChanges();

                        return Ok(new { dataUpdated = data, statusCode = 200, message = "Successfully Only Data Updated.!" });
                    }

                    var isExist = connx.Currencies.FirstOrDefault(c => c.Name == dataObject.Name);
                    if (isExist == null)
                    {
                        dataObject.UpdatedAt = DateTime.Now;
                        data.UpdatedAt = dataObject.UpdatedAt;
                        data.Status = dataObject.Status;
                        data.Name = dataObject.Name;
                        data.Code = dataObject.Code;
                        data.Symbol = dataObject.Symbol;

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
                var data = connx.Currencies.Find(id);
                if (data == null)
                {
                    return NotFound();
                }
                else
                {
                    //connx.Currencies.Remove(data); 
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
