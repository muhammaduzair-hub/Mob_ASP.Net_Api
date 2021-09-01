using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MOB_Monitoring_API.Controllers
{
    public class EmployeeController : ApiController
    {
        MOB_MonitoringEntities ent = new MOB_MonitoringEntities();

        [HttpGet]
        public HttpResponseMessage adminlogin(string email, string pass)
        {
            try
            {
                employee e = ent.employees.Single(a => a.eemail.Equals(email) && a.epass.Equals(pass) ); 
                if (e != null)
                    return Request.CreateResponse(HttpStatusCode.OK, e);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message.ToString());
            }
        }

        [HttpGet]
        public HttpResponseMessage emplogin(string email, string pass)
        {
            try
            {
                employee e = ent.employees.Single(a => a.eemail.Equals(email) && a.epass.Equals(pass) && !a.edesg.Equals("admin"));
                if (e != null)
                    return Request.CreateResponse(HttpStatusCode.OK, e);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message.ToString());
            }
        }

        [HttpPost]
        public HttpResponseMessage Signup([FromBody] employee NewEmp)
        {
            try
            {
                employee IfAlreadyFound = ent.employees.SingleOrDefault(a => a.eemail.Equals(NewEmp.eemail));
                if (IfAlreadyFound != null)
                    return Request.CreateErrorResponse(HttpStatusCode.Ambiguous, "Email Already Found");

                ent.employees.Add(NewEmp);
                ent.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, NewEmp.eemail + " successfully added");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateEmployee([FromBody] employee updateEmp)
        {
            try
            {
                employee IFNotFound = ent.employees.SingleOrDefault(a=>a.eemail.Equals(updateEmp.eemail));
                if (IFNotFound != null)
                {
                    IFNotFound.ename = !updateEmp.ename.Equals(String.Empty) ? updateEmp.ename : IFNotFound.ename ;
                    IFNotFound.epass = !updateEmp.epass.Equals(String.Empty) ? updateEmp.epass : IFNotFound.epass;
                    IFNotFound.edesg = !updateEmp.edesg.Equals(String.Empty) ? updateEmp.edesg : IFNotFound.edesg;
                    ent.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, IFNotFound.eemail+" updated successfully!");
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, updateEmp.eemail+" Not Found!");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public HttpResponseMessage getemp()
        {
            try
            {
                List<employee> emps = ent.employees.Where(a => a.edesg.Equals("emp") && a.flag == true).ToList();
                if (emps.Count >0)
                    return Request.CreateResponse(HttpStatusCode.OK, emps);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
        
        [HttpGet]
        public HttpResponseMessage getall()
        {

            try
            {
                List<employee> emps = ent.employees.ToList();
                if (emps != null)
                    return Request.CreateResponse(HttpStatusCode.OK, emps);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetEmpByEmail(string email)
        {
            try
            {
                employee emps = ent.employees.Single(a => a.eemail == email);
                if (emps!=null)
                    return Request.CreateResponse(HttpStatusCode.OK, emps);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
