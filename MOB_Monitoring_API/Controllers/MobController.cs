using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MOB_Monitoring_API.Controllers
{
    public class MobController : ApiController
    {
        MOB_MonitoringEntities ent = new MOB_MonitoringEntities();

        [HttpGet]
        public HttpResponseMessage gethistory()
        {
            try
            {
                List<MOB> all = ent.MOBs.Where(a=>a.mflag == 2 ).ToList();
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
        public HttpResponseMessage getactive()
        {
            try
            {
                List<MOB> all = ent.MOBs.Where(a => a.mflag != 2).ToList();
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
        public HttpResponseMessage getpending()
        {
            try
            {
                List<MOB> all = ent.MOBs.Where(a => a.mflag == 0).ToList();
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
        public HttpResponseMessage getname(int mid)
        {
            try
            {
                MOB all = ent.MOBs.SingleOrDefault(a => a.mid == mid);
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
        public HttpResponseMessage postMOb([FromBody] MOB mob)
        {
            try
            {
                MOB found = ent.MOBs.SingleOrDefault(a=>(a.mflag==0 || a.mflag == 1) && a.mname.Equals(mob.mname));

                if (found == null)
                {
                    Device d = ent.Devices.SingleOrDefault(a => a.did == mob.mdevice);
                    if (d != null)
                    {
                        d.flag = 0;
                        //ent.SaveChanges();
                    } 
                    else
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Device Not Found");

                    
                    mob.mflag = 0;
                    ent.MOBs.Add(mob);
                    
                    ent.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "Mob Added Successfully");
                    
                }
                return Request.CreateResponse(HttpStatusCode.Ambiguous, "Mob Already Found");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage DisposeMob(int mid)
        {
            try
            {
               MOB all = ent.MOBs.SingleOrDefault(a => a.mid == mid);
                if (all != null)
                {
                    all.mflag = 2;

                    var lastTIme = ent.Ariel_shots.Where(a=>a.mid == all.mid).Max(a => a.Ptime);

                    all.me_time = lastTIme;

                    Device updateFlag = ent.Devices.SingleOrDefault(a=>a.did == all.mdevice);
                    updateFlag.flag = 1;

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

        [HttpPut]
        public HttpResponseMessage updatemob([FromBody] MOB dev)
        {
            try
            {
                MOB all = ent.MOBs.SingleOrDefault(a => a.mid == dev.mid);
                if (all != null)
                {
                    Device updateFlag = ent.Devices.SingleOrDefault(a => a.did == all.mdevice);
                    updateFlag.flag = 1;

                    Device allocatenew = ent.Devices.SingleOrDefault(a => a.did == dev.mdevice);
                    allocatenew.flag = 0;

                    all.mdevice = allocatenew.did;
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
    }
}
