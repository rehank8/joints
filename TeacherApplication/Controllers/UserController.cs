using Data_Access;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.UI;
using TeacherApplication.Models;
using System.Threading.Tasks;

namespace TeacherApplication.Controllers
{
    [HandleError]
    public class UserController : Controller
    {

        // GET: User
        public ActionResult Index()
        {
            //ViewBag.Location = "City";
            ViewBag.CatName = "Photorapher,DJ,Chef,Banquethall,Limousine,Stage,Flower";
            //ViewBag.CategoryName = "Services";
            //ViewBag.Location = "Location";
            //ViewBag.Location = cities;
            //ViewBag.Subjects = new SelectList(DbHelper.GetSubjects(), "SubjectName", "SubjectName");
            ViewBag.FooterCities = DbHelper.GetAllCities();
			// ViewBag.Cities = new SelectList(DbHelper.GetAllCities(), "PKCityId", "CityName");
			//ViewBag.State = new SelectList(DbHelper.GetStates(), "PKStateId", "StateName");
			if (Request.IsAuthenticated)
			{
				if (Helper.RoleName.ToLower() == "vendor"||Helper.RoleName.ToLower()=="teacher")
				{
					return RedirectToAction("Index", "Teacher");
				}
				else if (Helper.RoleName.ToLower() == "admin")
				{
					return RedirectToAction("Index", "Admin");
				}



			}
		
			Roleslst();
            return View(new List<Domain.UserIndex>());
        }

		public ActionResult checkifuseralreadylogin()
		{
			
			return View();
		}
        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            Roleslst();
            var sub = (fc["subjectlink"] == "") ? null : fc["subjectlink"];
            ViewBag.FooterCities = DbHelper.GetAllCities();
            var location = (string.IsNullOrEmpty(fc["txtLocation"]) ? "" : fc["txtLocation"]);
            var city = (string.IsNullOrEmpty(fc["txtCity"]) ? "" : fc["txtCity"]);
            ViewBag.CategoryName = sub;
            ViewBag.Location = city;
            ViewBag.CatName = sub ?? "services";
            ViewBag.City = (city == "" ? location : city);
           IEnumerable<UserIndex> result = new List<UserIndex>();
            if (!string.IsNullOrEmpty(city))
            {
                result = DbHelper.GetTeachersForHome(sub).Where(x => (x.CityName ?? "").ToLower().Contains(city.ToLower()));
                if (result.Count() == 0)
                    result = DbHelper.GetTeachersForHome(sub).Where(x => x.Location.ToLower().Contains(city.ToLower()));
            }
             
            if (result.Count() == 0)
            {
                result = DbHelper.GetTeachersForHome(sub).Where(x => x.Location.ToLower().Contains(location.ToLower().Trim()));
                if (result.Count() == 0)
                    result = DbHelper.GetTeachersForHome(sub).Where(x => (x.CityName ?? "").ToLower().Contains(location.ToLower().Trim()));

            }
            return View(result);
            // var cityId = (string.IsNullOrEmpty(fc["Cities"]) ? 0.ToString() : fc["Cities"]);
            //var stateId = (string.IsNullOrEmpty(fc["states"]) ? 0.ToString() : fc["states"]);
            //ViewBag.Cities = new SelectList(DbHelper.GetAllCities(Convert.ToInt32(stateId)), "PKCityId", "CityName", Convert.ToInt64(cityId));
            //  ViewBag.State = new SelectList(DbHelper.GetStates(), "PKStateId", "StateName", stateId);
            //return View(DbHelper.GetTeachersForHome(sub, Convert.ToInt32(cityId), Convert.ToInt32(stateId)));
        }

        //  [Route("UserDetails/{location=loc}/{services=serv}/{vender=ven}/{id}")]
        public ActionResult UserDetails(long id = 0, string location = "", string services = "", string vender = "")
        {
            Roleslst();
            return View(DbHelper.GetTeacherTimeDetails(id));
        }
        [HttpPost]
        public ActionResult UserDetails(FormCollection fc)
        {
            StringBuilder str = new StringBuilder();
            str.Append("Dear "+fc["VenderName"]);
            str.Append("<br/><br/>");
            str.Append("Enquiry Details:- <br/>");
            str.Append("Name:" + fc["name"]+"<br/> EmailId:"+fc["email"]+"<br/> Phone Number:"+fc["phoneNumber"]+ "<br/> enquiry:"+fc["enquiry"]);
            str.Append($"<p>This is an automatically generated message to confirm receipt of your Booking via the Internet.You do not need to reply to this e - mail, but you may wish to save it for your records.<br/>Thank you.</p>");
            str.Append($"<br/>WarmRegards,<br/> SupportTeam");
            GmailHelper.Send(fc["VendorEmailId"], "Regarding Enquiry", str.ToString());
            Roleslst();
            int id = Convert.ToInt32(Request.QueryString.Get("id"));
            return View(DbHelper.GetTeacherTimeDetails(id));
        }


        [Authorize]
        public ActionResult TeacherBooking(long classId = 0)
        {
            Roleslst();
            return View(DbHelper.TeacherToBookDetails(classId));
        }

        [HttpPost]
        public ActionResult TeacherBooking(TeacherTimeDetails objTeacherDetails)
        {
            Roleslst();
            UserEnquiry objUserEnquiry = new UserEnquiry();
            objUserEnquiry.FollowUP = true;
            objUserEnquiry.FKUserId = Helper.UserId;
            objUserEnquiry.FKClassId = objTeacherDetails.ClassId;
            objUserEnquiry.FKTeacherId = objTeacherDetails.TeacherId;
            objUserEnquiry.FKAvailableTimeId = objTeacherDetails.AvailableTeacherTimeId;
            objUserEnquiry.BookedFromDate = Convert.ToDateTime(objTeacherDetails.FromAvailableDate).Date;
            objUserEnquiry.BookedFromTime = new TimeSpan(Convert.ToInt32((objTeacherDetails.FromAvailableTime).Split(':')[0]), 0, 0);
            objUserEnquiry.BookedToTime = new TimeSpan(Convert.ToInt32((objTeacherDetails.ToAvailableTime).Split(':')[0]), 0, 0).Duration();
            objUserEnquiry.BookedToDate = Convert.ToDateTime(objTeacherDetails.ToAvailableDate).Date;
            objUserEnquiry.CreatedDate = DateTime.Now;
            objUserEnquiry.BookingStatus = "Processing";
            DbHelper.InsertUserEnquiry(objUserEnquiry);
            var enquiryId = objUserEnquiry.PKEnquiryId;
            //var enquiryId = 23;
            TeacherProfile obj = DbHelper.GetTeacher(objUserEnquiry.FKTeacherId);
            UserProfile objUser = DbHelper.GetUserProfile(obj.FKUserId);



            StringBuilder str = new StringBuilder();
            str.Append("Dear " + objUser.UserName + ",");
            str.Append("<br/><br/>");
            str.Append($"Thank you for Booking the {objTeacherDetails.TeacherName}.Your Order or Enquiry Id is {enquiryId}<br/>");
            str.Append($"Your Booking Dates and Times are:-<br/><br/>");
            str.Append($"<table  border=1><tr><th>From Date</th><th> ToDate</th><th> FromTime</th><th> ToTime</th><th>Booking Status</th></tr>");
            str.Append($"<tr><th>{ objUserEnquiry.BookedFromDate.ToShortDateString()}</th><th>  { objUserEnquiry.BookedToDate.ToShortDateString()}</th><th> {objTeacherDetails.FromAvailableTime}</th><th> {objTeacherDetails.ToAvailableTime}</th><th>{objUserEnquiry.BookingStatus}</th></tr></table>");
            str.Append($"<br/><p><strong>The Teacher has to accept your booking,So please wait for confirmation from Teacher</strong></p>");
            str.Append($"<p>This is an automatically generated message to confirm receipt of your Booking via the Internet.You do not need to reply to this e - mail, but you may wish to save it for your records.<br/>Thank you.</p>");
            str.Append($"<br/>WarmRegards,<br/> SupportTeam");
            GmailHelper.Send(objUser.EmailId, "Regarding TeacherBooking", str.ToString());
            // GmailHelper.Send("sanday1994@gmail.com", "Regarding TeacherBooking", str.ToString());
            // str.Replace("Dear " + objUser.UserName + ",", "Dear " + obj.UserProfile.UserName + ",");
            //GmailHelper.Send(obj.UserProfile.EmailId, "Regarding User Booking ", str.ToString());
            return View("SuccessPage", objUserEnquiry);
        }
        [Authorize]
        public ActionResult SuccessPage(UserEnquiry userEnquiry)
        {
            Roleslst();
            return View(userEnquiry);
        }

        //userComments
        public async Task<JsonResult> UserComments(string comment, long classId, decimal rating, int teachId)
        {
            string userName = Helper.UserName;
            Task t1 = Task.Run(() =>
              {
                  if (!string.IsNullOrEmpty(comment))
                      DbHelper.AddUserComments(new UserComment() { Comments = comment, FKClassId = classId, UserName = userName, Rating = rating });
              });
            Task t2 = Task.Run(() =>
            {
                if (rating != 0)
                {
                    TeacherProfile objTeacherProfile = new TeacherProfile();
                    objTeacherProfile = DbHelper.GetTeacher(teachId);
                    objTeacherProfile.Rating = Math.Round((decimal)((objTeacherProfile.Rating + rating) / 2), 1);
                    DbHelper.UpdateTeacherProfile(objTeacherProfile);
                }
            });
            await Task.WhenAll(t1, t2);
            return Json(true);
        }

        //Rating
        //public JsonResult UpdateRating(decimal rating, int teachId)
        //{
        //    TeacherProfile objTeacherProfile = new TeacherProfile();
        //    objTeacherProfile = DbHelper.GetTeacher(teachId);
        //    objTeacherProfile.Rating = Math.Round((decimal)((objTeacherProfile.Rating + rating) / 2), 1);
        //    DbHelper.UpdateTeacherProfile(objTeacherProfile);
        //    return Json(objTeacherProfile.Rating);
        //}

        //Disable the Date
        public JsonResult DisableDate(int teachId = 0)
        {
            List<UserEnquiry> lstUserEnquiry = DbHelper.GetUserEnquiries().Where(t => t.FKTeacherId == teachId).ToList();
            List<string> arr = new List<string>();
            int j = 0;



            foreach (var item in lstUserEnquiry)
            {

                for (DateTime i = item.BookedFromDate; item.BookedFromDate <= item.BookedToDate;)
                {
                    arr.Add(item.BookedFromDate.ToString("yyyy-MM-dd"));

                    item.BookedFromDate = item.BookedFromDate.AddDays(1);
                    j++;

                }
            }
            object date = arr;
            return Json(date, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCities(int stateId)
        {
            return Json(DbHelper.GetAllCities(stateId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubject(int stateId, int cityId)
        {
            var data = DbHelper.GetTeachersForHome(null, cityId, stateId).GroupBy(x => x.SubjectName).Select(x => x.FirstOrDefault());
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetVendors(int cityId, string cityName = "")
        {
            ViewBag.Location = cityName;
            if (string.IsNullOrEmpty(cityName))
                return PartialView(DbHelper.GetTeachersForHome(null, cityId, 0));
            var result = DbHelper.GetTeachersForHome().Where(x => x.Location.ToLower().Contains(cityName.ToLower().Trim()));
            if (result.Count() == 0)
            {
                result = DbHelper.GetTeachersForHome().Where(x => (x.CityName ?? "").ToLower().Contains(cityName.ToLower().Trim()));
            }
            return PartialView(result);
        }

        public void Roleslst()
        {
            List<DPRoles> lstRoles = new List<DPRoles>() { new DPRoles() { PKRoleId = 3, RoleName = "User" }, new DPRoles() { PKRoleId = 2, RoleName = "Vender" } };
            ViewBag.Roles = new SelectList(lstRoles, "PKRoleId", "RoleName");
        }
    }
}