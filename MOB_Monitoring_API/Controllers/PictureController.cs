using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web;
using System.Drawing;
using System.Text;

namespace MOB_Monitoring_API.Controllers
{
    public class PictureController : ApiController
    {
        MOB_MonitoringEntities ent = new MOB_MonitoringEntities();

        //[HttpGet]
        //public HttpResponseMessage testflask()
        //{
        //    try 
        //    {
        //        //Get strength of mob
        //        String a = "http://127.0.0.1:5000/count?Query="+"C:/Users/Muhammad%20Uzair/Downloads/1.jpg";
        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(a);
        //        httpWebRequest.Method = "Get";
        //        httpWebRequest.KeepAlive = false;
        //        httpWebRequest.Accept = "text/json";
        //        HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

        //        string ans;
        //        using (StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream() ))
        //        {
        //            ans = reader.ReadToEnd();
        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, ans);
        //    }
        //    catch (Exception e)
        //    { return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message.ToString()); }
        //}

        [HttpPost]
        public HttpResponseMessage PostPicture([FromBody] Ariel_shots data)
        {
            //first check is there any picture available in db with same latlong of same mob
            try
            {
                var checkiflatlongavailable = ent.Ariel_shots.Single(a => a.mid == data.mid && a.Platitude==data.Platitude && a.Plongitude==data.Plongitude);
                return Request.CreateResponse(HttpStatusCode.Ambiguous, "The picture of this location is already found ");
            }
            catch 
            {
                
            }
            //Second check is there any picture available in db with same datetime of same mob
            try
            {
                var checkiflatlongavailable = ent.Ariel_shots.Single(a => a.mid == data.mid && a.Ptime == Convert.ToDateTime(data.Ptime) );
                return Request.CreateResponse(HttpStatusCode.Ambiguous, "The same dateTtime is already found of this mob ");
            }
            catch
            {

            }


            try
            {
                string path=String.Empty;
                int min; double speed=1;

                //Create the Directory.
                try
                {
                    //Create the Directory.
                    path = HttpContext.Current.Server.MapPath("~/Images/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                } 
                catch{
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "prob in creating directory");
                }

                //Now save the image in folder
                try
                {
                    try
                    {
                        data.Pname = (ent.Ariel_shots.Max(a => a.pid)+1).ToString();
                    }
                    catch
                    {
                        data.Pname = "1";
                    }
                    path += data.Pname+".jpg";
                    File.WriteAllBytes(path, Convert.FromBase64String(data.Paddress));
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "bytes: "+e.Data.ToString());
                }

                //Get strength of mob through flask api
                try
                {
                    data.Paddress = path;
                    //Get strength of mob through flask api
                    String adress = "http://127.0.0.1:5000/count?Query=" + data.Paddress; //"C:/Users/Muhammad%20Uzair/Downloads/1.jpg";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(adress);
                    httpWebRequest.Method = "Get";
                    httpWebRequest.KeepAlive = false;
                    httpWebRequest.Accept = "text/json";
                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    string ans;
                    using (StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        ans = reader.ReadToEnd();
                    }
                    data.pmobquantity = int.Parse(ans);
                    //IF FLASk api result is null
                    if (data.pmobquantity == null)
                        return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "Flask api connectivity error");
                    //check the threadshold
                    MOB m = ent.MOBs.Single(a => a.mid == data.mid);
                    if (m.threshhold > data.pmobquantity)
                    {
                        File.Delete(data.Paddress);
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "mob quantity is less than Mob Threadshold");
                    }
                }
                catch (Exception e)
                {
                    File.Delete(data.Paddress);
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "flask Connection Error");
                }

                
                //Now give the path of image in folder to db
                try
                {
                    //now save the data in entities
                    data.Ptime = Convert.ToDateTime(data.Ptime);
            
                    try
                    {
                        data.mpic_no = ent.Ariel_shots.Where(a => a.mid == data.mid).Max(a => a.mpic_no)+1;
                        if (data.mpic_no == null)
                        {
                            data.mpic_no = 1;
                            data.pspeed = "0 km/m";
                            
                            //we got 1st pic of mob so we alter the table of mob (starting time and flag)
                            MOB active = ent.MOBs.SingleOrDefault(a => a.mid == data.mid);
                            active.mflag = 1; active.ms_time = data.Ptime;
                            ent.SaveChanges();
                        }
                        else
                        {
                            //if we got 2nd or furture pic then we will detect the speed = distance/time
                            var pre = ent.Ariel_shots.SingleOrDefault(a=>a.mpic_no == data.mpic_no-1);
                            TimeSpan answer = data.Ptime.Value - pre.Ptime.Value;
                            min = answer.Minutes;

                            double _distance = distance((double)pre.Platitude, (double)pre.Plongitude, (double)data.Platitude, (double)data.Plongitude, 'K');

                            speed = _distance / min;
                            speed=Math.Round(speed, 4);
                            data.pspeed = speed + " km/m";
                           // ent.SaveChanges();
                        }
                    }
                    catch 
                    {
                        data.mpic_no = 1;
                    }

                    

                    //MOB_MonitoringEntities e = new MOB_MonitoringEntities();
                    ent.Ariel_shots.Add(data);
                    ent.SaveChanges();

                }
                catch (Exception e)
                {
                    File.Delete(data.Paddress);
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "DB: "+e.Message);
                }

                //now update the zonedetail
                try
                {
                    List<zone> allzones = ent.zones.Where(a=>a.zflag==1).ToList();
                    if (allzones.Count > 0) 
                    {
                        foreach (var _zone in allzones) 
                        {
                            var res = ent.zone_detail.SingleOrDefault(a => a.mid == data.mid && a.zid == _zone.zid);
                            if (res != null)
                            {
                                //its mean zone is in onshow mode
                                var dist = distance((double)data.Platitude, (double)data.Plongitude, (double)_zone.zlatitude, (double)_zone.zlongitude, 'K');
                                var time = dist / speed;

                                if (res.reachtime < (int)time) { res.mstatus = "Away"; }
                                else res.mstatus = "Toward";

                                res.mlatitude = data.Platitude;
                                res.reachtime = (int)time;
                                res.mlongitude = data.Plongitude;
                                res.mtime = data.Ptime;
                                res.pmobquantity = data.pmobquantity;
                                ent.SaveChanges();
                            }
                            else 
                            {
                                zone_detail newdetail = new zone_detail();
                                var dist = distance((double)data.Platitude, (double)data.Plongitude, (double)_zone.zlatitude, (double)_zone.zlongitude, 'K');
                                var time__ = dist / speed;

                                if (dist < _zone.km+3) {
                                    newdetail.zid = _zone.zid;
                                    newdetail.mid = data.mid;
                                    newdetail.mstatus = "Toward";
                                    newdetail.mlatitude = data.Platitude;
                                    newdetail.mlongitude = data.Plongitude;
                                    newdetail.mtime = data.Ptime;
                                    newdetail.reachtime = (int)time__;
                                    newdetail.pmobquantity = data.pmobquantity;
                                    ent.zone_detail.Add(newdetail);
                                    ent.SaveChanges();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Data.ToString());
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Data Saved");
            }
            catch(Exception ex) {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Data.ToString());
            }

            

        }

        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            try
            {
                List<Ariel_shots> all = ent.Ariel_shots.ToList();
                if (all != null)
                    return Request.CreateResponse(HttpStatusCode.OK, all);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message.ToString());
            }
        }


        [HttpGet]
        public HttpResponseMessage Getpicofmob(int mid)
        {
            try
            {
                List<Ariel_shots> all = ent.Ariel_shots.Where(a=>a.mid == mid).OrderBy(a=>a.mpic_no).ToList();
                if (all.Count > 0)
                {
                    foreach (var i in all)
                    {
                        byte[] imageArray = System.IO.File.ReadAllBytes(i.Paddress);
                        i.Paddress = Convert.ToBase64String(imageArray);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, all);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message.ToString());
            }
        }

        [HttpGet]
        public HttpResponseMessage GetDistance(double lat, double lon)
        {
            try
            {
                List<zone> finallist = new List<zone>();
                List<zone> all = ent.zones.Where(a=>a.zflag==1).ToList();
                if (all.Count > 0)
                {
                    foreach (var i in all)
                    {
                        double dis = distance(lat, lon, (double)i.zlatitude, (double)i.zlongitude, 'K');
                        if (dis < (double)i.km + 3)
                            finallist.Add(i);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, finallist);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message.ToString());
            }
        }

        [HttpGet]
        public HttpResponseMessage ZoneDetail(int zid)
        {
            List<Ariel_shots> mobinzone = new List<Ariel_shots>();
            try
            {
                //check wheteher zone is available or not
                zone getzone = ent.zones.SingleOrDefault(a => a.zid == zid);
                if (getzone != null)
                {
                    //get active mobs then get current location of active mobs
                    List<MOB> activemobs = ent.MOBs.Where(a => a.mflag == 1).ToList();
                    List<Ariel_shots> currentlocation = new List<Ariel_shots>();


                    foreach (var mob in activemobs)
                    {
                        List<Ariel_shots> snaplist = ent.Ariel_shots.Where(a => a.mid == mob.mid).ToList();
                        int maxpicno = (int)snaplist.Max(a => a.mpic_no);
                        currentlocation.Add(snaplist.Single(a => a.mpic_no == maxpicno));
                    }

                    //get the distance between zone km and mobs current location
                    foreach (var dangermobs in currentlocation)
                    {
                        double dis = distance((double)getzone.zlatitude, (double)getzone.zlongitude, (double)dangermobs.Platitude, (double)dangermobs.Plongitude, 'K');
                        if (dis <= (double)getzone.km+2)
                            mobinzone.Add(dangermobs);
                    }

                    //encode the image of mob into base64string
                    foreach (var i in mobinzone)
                    {
                        byte[] imageArray = System.IO.File.ReadAllBytes(i.Paddress);
                        i.Paddress = Convert.ToBase64String(imageArray);
                    }
                    //List<Ariel_shots> final = ent.Ariel_shots.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, mobinzone);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Zone not found");

            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message.ToString());
            }
        }

        private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            if ((lat1 == lat2) && (lon1 == lon2))
            {
                return 0;
            }
            else
            {
                double theta = lon1 - lon2;
                double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
                dist = Math.Acos(dist);
                dist = rad2deg(dist);
                dist = dist * 60 * 1.1515;
                if (unit == 'K')
                {
                    dist = dist * 1.609344;
                }
                else if (unit == 'N')
                {
                    dist = dist * 0.8684;
                }
                return (dist);
            }
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
        
    }
}
