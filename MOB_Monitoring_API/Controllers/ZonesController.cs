using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MOB_Monitoring_API.Controllers
{
    public class ZonesController : ApiController
    {
        MOB_MonitoringEntities ent = new MOB_MonitoringEntities();

        [HttpGet]
        public HttpResponseMessage allzone()
        {
            try
           {
                List<zone> all = ent.zones.Where(a=>a.zflag==1).ToList();
                if (all != null)
                {
                   
                    return Request.CreateResponse(HttpStatusCode.OK, all);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No red zone");
            }
            catch (Exception e) 
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message.ToString());
            }
        }


        [HttpPost]
        public HttpResponseMessage postzone([FromBody] zone newZone)
        {
            try
            {
                zone alreadyFound = ent.zones.SingleOrDefault(a=>a.zlatitude == newZone.zlatitude && a.zlongitude==newZone.zlongitude);
                if (alreadyFound != null)
                    return Request.CreateErrorResponse(HttpStatusCode.Ambiguous, "This Zone Already Found");
                
                newZone.zflag = 1;
                ent.zones.Add(newZone);
                //ent.SaveChanges();

                try
                {
                    employee emp = ent.employees.Single(a => a.eemail.Equals(newZone.employee));
                    //emp.flag = false;
                    ent.SaveChanges();
                }
                catch
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Emp not found");
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Zone add");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message.ToString());
            }
        }

        [HttpPut]
        public HttpResponseMessage updateFlag([FromBody] zone updateZone)
        {
            try
            {
                zone found = ent.zones.SingleOrDefault(a => a.zid == updateZone.zid);
                if (found != null)
                {
                    found.zflag = updateZone.zflag;
                    ent.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Update Successfully");

                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
                
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
        }

        [HttpDelete]
        public HttpResponseMessage delete([FromBody] zone z)
        {
            try
            {
                zone notFound = ent.zones.SingleOrDefault(a => a.zid == z.zid);
                if (notFound != null)
                {
                    ent.zones.Remove(notFound);
                    ent.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Data Deleted Successfully");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message.ToString());
            }
        }

        [HttpGet]
        public HttpResponseMessage getzone(string eemail)
        {
            try
            {
                List<zone> z = ent.zones.Where(a=>a.employee.Equals(eemail)).ToList();
                if (z.Count != 0)
                {

                    return Request.CreateResponse(HttpStatusCode.OK, z);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No red zone");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message.ToString());
            }
        }

        [HttpGet]
        public HttpResponseMessage getzonename(int zid)
        {
            try
            {
                zone all = ent.zones.Single(a => a.zid == zid);
                if (all != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, all);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No red zone");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message.ToString());
            }
        }
        //======================get zone and then update the zone info (bottom sheet work)
        [HttpGet]
        public HttpResponseMessage zoneDetail(int zid)
        {
            try
            {
                zone all = ent.zones.Single(a => a.zid == zid);
                if (all != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, all);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No red zone");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message.ToString());
            }
        }

        [HttpPut]
        public HttpResponseMessage updateZone([FromBody] zone updateZone)
        {
            try
            {
                zone found = ent.zones.SingleOrDefault(a => a.zid == updateZone.zid);
                if (found != null)
                {
                    if (!updateZone.employee.Equals(""))
                    {
                        //update status code of previous emp
                        //employee emp = ent.employees.Single(a => a.eemail.Equals(found.employee));
                        //emp.flag = true;

                        //update status code of new assign emp
                        //emp =  ent.employees.Single(a => a.eemail.Equals(updateZone.employee));
                        //emp.flag = false;

                        //now update emp
                        found.employee = updateZone.employee;
                    }
                    if (updateZone.km != null)
                    {
                        found.km = updateZone.km;
                    }
                    
                    ent.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Update Successfully");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");

            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
        }

    }
}

