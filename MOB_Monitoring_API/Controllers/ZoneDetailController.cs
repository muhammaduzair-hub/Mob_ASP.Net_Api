using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MOB_Monitoring_API.Controllers
{
    public class ZoneDetailController : ApiController
    {

        MOB_MonitoringEntities ent = new MOB_MonitoringEntities();

        //i did this work in post picture
        //[HttpPost]
        //public HttpResponseMessage newupdate([FromBody] zone_detail data) 
        //{
        //            } 

        [HttpGet]
        public HttpResponseMessage viewzone( int id)
        {
            List<zone_detail> result = ent.zone_detail.Where(a => a.zid == id).ToList();
            if (result.Count > 0)
                return Request.CreateResponse(HttpStatusCode.OK, result);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
        }

        [HttpGet]
        public HttpResponseMessage viewzonestatus(int zid, int mid)
        {
            List<zone_detail> result = ent.zone_detail.Where(a => a.zid == zid && a.mid==mid).ToList();
            if (result.Count > 0)
                return Request.CreateResponse(HttpStatusCode.OK, result);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
        }


    }
}
