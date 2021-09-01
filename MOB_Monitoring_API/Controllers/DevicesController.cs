using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MOB_Monitoring_API.Controllers
{
    public class DevicesController : ApiController
    {
        MOB_MonitoringEntities ent = new MOB_MonitoringEntities();
        [HttpGet]
        public HttpResponseMessage getAll()
        {
            try
            {
                List<Device> all = ent.Devices.ToList();
                if (all != null)
                    return Request.CreateResponse(HttpStatusCode.OK, all);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage getavailable()
        {
            try
            {
                List<Device> all = ent.Devices.Where(a=>a.flag == 1).ToList();
                if (all != null)
                    return Request.CreateResponse(HttpStatusCode.OK, all);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage getdeviceswithmob()
        {
            try
            {
                List<Device> all = ent.Devices.Where(a => a.flag == 0).ToList();
                if (all != null)
                    return Request.CreateResponse(HttpStatusCode.OK, all);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage getmobwithdevice(int mid)
        {
            try
            {
               MOB all = ent.MOBs.SingleOrDefault(a => a.mdevice == mid);
                if (all != null)
                    return Request.CreateResponse(HttpStatusCode.OK, all);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage postdevice([FromBody] Device dev)
        {
            try
            {
                Device all = ent.Devices.SingleOrDefault(a => a.dname == dev.dname);
                if (all == null)
                {
                    dev.flag = 1;
                    ent.Devices.Add(dev);
                    ent.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, all); 
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        public HttpResponseMessage delete(int d)
        {
            try
            {
                Device notFound = ent.Devices.SingleOrDefault(a => a.did == d);
                if (notFound != null)
                {
                   // notFound.flag = false;
                    ent.Devices.Remove(notFound);
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
    }
}
