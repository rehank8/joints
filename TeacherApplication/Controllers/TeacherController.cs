using Data_Access;
using Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using TeacherApplication.Models;

namespace TeacherApplication.Controllers
{
    [HandleError]
    [Authorize]
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            ViewBag.Cities = new SelectList(DbHelper.GetAllCities(), "PKCityId", "CityName");
            ViewBag.States = new SelectList(DbHelper.GetStates(), "PKStateId", "StateName");
            ViewBag.Subject = new SelectList(DbHelper.GetSubjects(), "PKSubjectId", "SubjectName");
            return View();
        }
        #region class
        public ActionResult Classes()
        {
			return View(DbHelper.GetClasses(Helper.UserId, null, null));
        }
        public ActionResult CreateClass()
        {
            ViewBag.Cities = new SelectList(DbHelper.GetAllCities(), "PKCityId", "CityName");
            ViewBag.States = new SelectList(DbHelper.GetStates(), "PKStateId", "StateName");
            ViewBag.Subject = new SelectList(DbHelper.GetSubjects(), "PKSubjectId", "SubjectName");
            return View();
        }
        [HttpPost]
        public ActionResult CreateClass(Class objClass, HttpPostedFileBase classImg)
        {
            ViewBag.Subject = new SelectList(DbHelper.GetSubjects(), "PKSubjectId", "SubjectName");
            objClass.FKUserId = Helper.UserId;
            objClass.IsApproved = true;//Admin Approvell 
            if (ModelState.IsValid)
            {
                string imageName = System.IO.Path.GetFileName(classImg.FileName);
                classImg.SaveAs(Server.MapPath("~/Images/" + Helper.UserId + "_" + objClass.FKSubjectId + "_" + imageName));
                objClass.ClassImage = "/Images/" + Helper.UserId + "_" + objClass.FKSubjectId + "_" + imageName;
                DbHelper.InsertClass(objClass);
                TempData["ClassId"] = objClass.PKClassId;
                return RedirectToAction("CreateTeacherProfile", "Teacher");
            }
            return View(objClass);
        }
		[Authorize]
        public ActionResult EditClass(int id)
        {
			Category objCat = new Category();
			Class cls = DbHelper.GetClass(id);
			TeacherProfile teacherProfile = DbHelper.GetTeacherProfile(id);
			objCat.Class = cls;
			objCat.TeacherProfile = teacherProfile;
            ViewBag.Subject = new SelectList(DbHelper.GetSubjects(), "PKSubjectId", "SubjectName", cls.FKSubjectId);
			List<City> citiesList = DbHelper.GetAllCities();
			objCat.Cities = citiesList.Where(x => x.FKStateId == cls.City.FKStateId).ToList();
			ViewBag.Cities = objCat.Cities;
			ViewBag.stateId = cls.City.FKStateId;
            ViewBag.States = new SelectList(DbHelper.GetStates(), "PKStateId", "StateName", cls.City.FKStateId);
            return View(objCat);
        }
        [HttpPost]
		[Authorize]
		public ActionResult EditClass(Category catmodel, HttpPostedFileBase classImg)
        {

			//ViewBag.Cities = new SelectList(DbHelper.GetAllCities(), "PKCityId", "CityName", catmodel.Class.FKCityId);
   //         ViewBag.States = new SelectList(DbHelper.GetStates(), "PKStateId", "StateName");
   //         ViewBag.Subject = new SelectList(DbHelper.GetSubjects(), "PKSubjectId", "SubjectName", catmodel.Class.FKSubjectId);
            //if (ModelState.IsValid)
            //{
                if (classImg != null)
                {
                    string imageName = System.IO.Path.GetFileName(classImg.FileName);
                    classImg.SaveAs(Server.MapPath("~/Images/" + Helper.UserId + "_" + catmodel.Class.FKSubjectId + "_" + imageName));
					catmodel.Class.ClassImage = "/Images/" + Helper.UserId + "_" + catmodel.Class.FKSubjectId + "_" + imageName;
                }
				
                DbHelper.UpdateClass(catmodel.Class);
				DbHelper.UpdateTeacherProfile(catmodel.TeacherProfile);
                return RedirectToAction("Classes");
          //  }
           // return View(catmodel);
        }
        //Get Class on Id
        public ActionResult ClassDetails(int id)
        {
            return View(DbHelper.GetClass(id));
        }

		public ActionResult DeleteClass(int id)
		{
			Class cls = DbHelper.GetClass(id);
			cls.IsActive = false;
			DbHelper.UpdateClass(cls);
			return RedirectToAction("Classes");
		}
        #endregion

        #region TeacherProfile
        public ActionResult TeacherProfiles()
        {
            return View(DbHelper.GetTeachers(Helper.UserId));
        }

        public ActionResult CreateTeacherProfile()
        {
            ViewBag.Class = new SelectList(DbHelper.GetClasses(Helper.UserId), "PKClassId", "ClassName");
            return View();
        }
        [HttpPost]
        public ActionResult CreateTeacherProfile(TeacherProfile objTeacherProfile)
        {
            objTeacherProfile.FKClassId = Convert.ToInt64(TempData["ClassId"]);
            TempData.Keep();
            objTeacherProfile.FKUserId = Helper.UserId;
            objTeacherProfile.IsActive = true;
            DbHelper.InsertTeacherProfile(objTeacherProfile);
            return View("CreateTeacherAvailableTime");
        }

        //Get TeacherProfile
        public ActionResult TeacherProfile()
        {
            return View(DbHelper.GetTeachers(Helper.UserId));
        }
        //Get TeacherProfile Details on Id
        public ActionResult TeacherprofileDetails(int id)
        {
            return View(DbHelper.GetTeacher(id));
        }
        //get TeacherProfile Edit on Id
        public ActionResult EditTeacherProfile(int id)
        {
            TeacherProfile teacherProfile = DbHelper.GetTeacher(id);
            ViewBag.Class = new SelectList(DbHelper.GetClasses(Helper.UserId), "PKClassId", "ClassName", teacherProfile.FKUserId);
            return View(DbHelper.GetTeacher(id));
        }
        [HttpPost]
        public ActionResult EditTeacherProfile(TeacherProfile objTeacherProfile)
        {
            DbHelper.UpdateTeacherProfile(objTeacherProfile);
            return RedirectToAction("TeacherProfiles");
        }

        #endregion


        #region TeacherImages

        public ActionResult TeacherImages(int classId = 0)
        {
            ViewBag.ClassId = classId;
            if (classId != 0)
                return View(DbHelper.GetUserImages(0, classId));
            return View(DbHelper.GetUserImages(Helper.UserId));
        }
        //public ActionResult TeacherVideos()
        //{
        //    return View(DbHelper.GetUserImages(Helper.UserId));
        //}
        public ActionResult CreateTeacherImages(int id = 0)
        {
            ViewBag.ClassId = id;
            //int result = 0;
            //int.TryParse(TempData["ClassId"] + "", out result);
            //if (result != 0)
            //    objUserImage.FKClassId = result;
            //ViewBag.Classes = new SelectList(DbHelper.GetClasses(Helper.UserId), "PKClassId", "ClassName", result);
            return View();
        }
        [HttpPost]
        public ActionResult CreateTeacherImages(UserImage objUserImage, IEnumerable<HttpPostedFileBase> image)
        {
            if (image != null)
            {
                if (!String.IsNullOrEmpty(objUserImage.VideoUrl))
                {
                    if (objUserImage.VideoUrl.Contains("watch") && objUserImage.VideoUrl.Contains("youtube.com"))
                    {
                        string querystring = objUserImage.VideoUrl.Split('=')[1];
                        objUserImage.VideoUrl = "https://www.youtube.com/embed/" + querystring;
                    }
                }
                foreach (HttpPostedFileBase img in image)
                {
                    string imageName = System.IO.Path.GetFileName(img.FileName);
                    img.SaveAs(Server.MapPath("~/Images/" + Helper.UserId + "-" + imageName));
                    objUserImage.ImageUrl = "/Images/" + Helper.UserId + "-" + imageName;
                    objUserImage.ImageName = imageName;
                    DbHelper.InsertUserImages(objUserImage);
                }
                return RedirectToAction("TeacherImages", new { classId = objUserImage.FKClassId });
            }
            return View(objUserImage);
        }
        //Editing Teacher Images based on Id 
        public ActionResult EditTeacherImages(int id)
        {
            return View(DbHelper.GetUserImage(id));
        }
        [HttpPost]
        public ActionResult EditTeacherImages(UserImage objUserImage, IEnumerable<HttpPostedFileBase> image)
        {
            if (image != null)
            {
                foreach (HttpPostedFileBase img in image)
                {
                    string imageName = System.IO.Path.GetFileName(img.FileName);
                    img.SaveAs(Server.MapPath("~/Images/" + Helper.UserId + "-" + imageName));
                    objUserImage.ImageUrl = "/Images/" + Helper.UserId + "-" + imageName;
                    objUserImage.ImageName = imageName;
                    DbHelper.UpdateUserImages(objUserImage);
                }
                return RedirectToAction("TeacherImages", new { classId = objUserImage.FKClassId });
            }
            return View(objUserImage);
        }
        //public ActionResult EditTeacherVideos(int id)
        //{
        //    return View(DbHelper.GetUserImage(id));
        //}
        //[HttpPost]
        //public ActionResult EditTeacherVideos(UserImage objUserImage)
        //{
        //    if (!String.IsNullOrEmpty(objUserImage.VideoUrl))
        //    {
        //        if (objUserImage.VideoUrl.Contains("watch") && objUserImage.VideoUrl.Contains("youtube.com"))
        //        {
        //            string querystring = objUserImage.VideoUrl.Split('=')[1];
        //            objUserImage.VideoUrl = "https://www.youtube.com/embed/" + querystring;
        //        }
        //    }
        //    DbHelper.UpdateUserImages(objUserImage);
        //    return RedirectToAction("TeacherVideos");
        //}

        public ActionResult Delete(int id)
        {
            DbHelper.DeleteUserImages(id);
            return RedirectToAction("TeacherImages");
        }
        #endregion

        #region UserEnquires
        public ActionResult UserEnquires()
        {
            TeacherProfile teach = DbHelper.GetTeachers(Helper.UserId).FirstOrDefault();
            if (teach == null)
            {
                List<UserEnquiry> lst = new List<UserEnquiry>();
                return View(lst);
            }
            else
                return View(DbHelper.GetUserEnquiries(Convert.ToInt64(teach.PKTeachersId)));
        }

        public ActionResult UserEnquiryDetails(long id = 0)
        {
            if (id != 0)
            {
                UserEnquiry objUserEnquiry = DbHelper.GetUserEnquiry(id);
                return View(objUserEnquiry);
            }
            return View();
        }
        public ActionResult EditUserEnquiry(long id = 0)
        {
            if (id != 0)
            {
                UserEnquiry objUserEnquiry = DbHelper.GetUserEnquiry(id);
                return View(objUserEnquiry);
            }
            return View();
        }
        [HttpPost]
        public ActionResult EditUserEnquiry(UserEnquiry objUserEnquiry)
        {
            if (ModelState.IsValid)
            {
                if (objUserEnquiry.FollowUP == false)
                {
                    objUserEnquiry.BookingStatus = "Cancelled By Teacher";
                    StringBuilder str = new StringBuilder();
                    str.Append("Dear " + objUserEnquiry.UserName + ",");
                    str.Append("<br/><br/>");
                    str.Append($"Thank you for Booking the {objUserEnquiry.TeacherName}.Your Order or Enquiry Id is {objUserEnquiry.PKEnquiryId}<br/>");
                    str.Append($"Your Booking Dates and Times are:-<br/><br/>");
                    str.Append($"<table  border=1><tr><th>From Date</th><th> ToDate</th><th> FromTime</th><th> ToTime</th><th>Booking Status</th></tr>");
                    str.Append($"<tr><th>{ objUserEnquiry.BookedFromDate.ToShortDateString()}</th><th>  { objUserEnquiry.BookedToDate.ToShortDateString()}</th><th> {objUserEnquiry.BookedFromTime}</th><th> {objUserEnquiry.BookedToTime}</th><th>{objUserEnquiry.BookingStatus}</th></tr></table>");
                    str.Append($"<br/><p><strong>Congra   the teacher has to accepted your booking,So please Send your PayPal Id to Teacher. Teacher Email Id is {objUserEnquiry.TeacherProfile.UserProfile.EmailId} </strong></p>");
                    str.Append($"<p>This is an automatically generated message to confirm receipt of your Booking via the Internet.You do not need to reply to this e - mail, but you may wish to save it for your records.<br/>Thank you.</p>");
                    str.Append($"<br/>WarmRegards,<br/> SupportTeam");
                    GmailHelper.Send(objUserEnquiry.EmailId, "Regarding TeacherBooking", str.ToString());

                }
                else
                {
                    objUserEnquiry.BookingStatus = "Accepted By Teacher";
                    StringBuilder str = new StringBuilder();
                    str.Append("Dear " + objUserEnquiry.UserName + ",");
                    str.Append("<br/><br/>");
                    str.Append($"Thank you for choosing the {objUserEnquiry.TeacherName}.Your Order or Enquiry Id is {objUserEnquiry.PKEnquiryId}<br/>");
                    str.Append($"Sorry say that teacher has cancelled the booking due to heavy booking on teacher");
                    str.Append($"<table  border=1><tr><th>From Date</th><th> ToDate</th><th> FromTime</th><th> ToTime</th><th>Booking Status</th></tr>");
                    str.Append($"<tr><th>{ objUserEnquiry.BookedFromDate.ToShortDateString()}</th><th>  { objUserEnquiry.BookedToDate.ToShortDateString()}</th><th> {objUserEnquiry.BookedFromTime}</th><th> {objUserEnquiry.BookedToTime}</th><th>{objUserEnquiry.BookingStatus}</th></tr></table>");
                    str.Append($"<p>This is an automatically generated message to confirm receipt of your Booking via the Internet.You do not need to reply to this e - mail, but you may wish to save it for your records.<br/>Thank you.</p>");
                    str.Append($"<br/>WarmRegards,<br/> SupportTeam");
                    GmailHelper.Send(objUserEnquiry.EmailId, "Regarding TeacherBooking", str.ToString());
                }
                DbHelper.UpdateUserEnquiry(objUserEnquiry);
                return View("Index");
            }
            return View(objUserEnquiry);
        }
        #endregion

        #region UserProfile
        //Get: UserProfile based on Id
        public ActionResult EditUserProfile(long id = 0)
        {
            return View(DbHelper.GetUserProfile(id));
        }
        [HttpPost]
        public ActionResult EditUserProfile(UserProfile objUserProfile, HttpPostedFileBase photo)
        {
            if (photo != null)
            {
                string imageName = System.IO.Path.GetFileName(photo.FileName);
                photo.SaveAs(Server.MapPath("~/Images/" + Helper.UserId + "-" + imageName));
                objUserProfile.PhotoUrl = "/Images/" + Helper.UserId + "-" + imageName;
            }
            
            DbHelper.UpdateUsers(objUserProfile);
            return View(DbHelper.GetUserProfile(objUserProfile.PKUserId));
        }
        #endregion

        #region AvailableTeacherTime
        public ActionResult TeacherAvailableTimes()
        {
            return View(DbHelper.GetAvailableTeacherTimes(Helper.UserId));
        }
        //Creating Teacher AvailableTeacherTime based on ClassId
        public ActionResult CreateTeacherAvailableTime()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTeacherAvailableTime(AvailableTeacherTime objAvailableTeacherTime)
        {
            objAvailableTeacherTime.IsActive = true;
            objAvailableTeacherTime.FKClassId = Convert.ToInt64(TempData["ClassId"]);
            TempData.Keep();
            DbHelper.InsertAvailableTeacherTime(objAvailableTeacherTime);
            return RedirectToAction("CreateTeacherImages");
        }
        //Edit Teacher AvailableTeacherProfile based on AvailableTeacherProfileId
        public ActionResult EditTeacherAvailableTime(long id = 0)
        {
			Category objCategory = new Category();
			AvailableTeacherTime objAvailableTeacherTime = DbHelper.GetAvailableTeacherTime(id);
			objCategory.AvalilableTeacher = objAvailableTeacherTime;
			objCategory.Class = DbHelper.GetClass((int)objAvailableTeacherTime.FKClassId);
			
			
            return View(objCategory);
        }
        [HttpPost]
        public ActionResult EditTeacherAvailableTime(AvailableTeacherTime objAvailableTeacherTime)
        {
			Category objCategory = new Category();
			//objCategory.AvalilableTeacher.FKClassId = DbHelper.GetClass((int)objAvailableTeacherTime.FKClassId);
			//objAvailableTeacherTime.FKClassId=  DbHelper.GetClass((int)(objAvailableTeacherTime.FKClassId));
			DbHelper.UpdateAvailableTeacherTime(objAvailableTeacherTime);
            return View("Index");
        }
        //Delete AvailableTeacherTime based on AvailableTeacherProfileId
        public ActionResult DeleteAvailableTeacherTime(long id)
        {
            DbHelper.DeleteAvailableTeacherTime(id);
            return View("Index");
        }
        #endregion


        #region Teacher Videos
        public ActionResult TeacherVideos(int id)
        {
            ViewBag.ClassId = id;
            return View(DbHelper.GetUservideos(id));
        }

        public ActionResult CreateTeacherVidoes(UserVideo userVideo)
        {
            ViewBag.ClassId = userVideo.FKClassId;
            if (!String.IsNullOrEmpty(userVideo.VideoUrl))
            {
                if (userVideo.VideoUrl.Contains("watch") && userVideo.VideoUrl.Contains("youtube.com"))
                {
                    string querystring = userVideo.VideoUrl.Split('=')[1];
                    userVideo.VideoUrl = "https://www.youtube.com/embed/" + querystring;
                }
                DbHelper.InsertUserVideo(userVideo);
                return RedirectToAction("TeacherVideos", new { id = userVideo.FKClassId });
            }
            return View(userVideo);
        }

        public ActionResult EditTeacherVideo(int id)
        {
            return View(DbHelper.GetUserVideo(id));
        }

        [HttpPost]
        public ActionResult EditTeacherVideo(UserVideo userVideo)
        {
            ViewBag.ClassId = userVideo.FKClassId;
            if (!String.IsNullOrEmpty(userVideo.VideoUrl))
            {
                if (userVideo.VideoUrl.Contains("watch") && userVideo.VideoUrl.Contains("youtube.com"))
                {
                    string querystring = userVideo.VideoUrl.Split('=')[1];
                    userVideo.VideoUrl = "https://www.youtube.com/embed/" + querystring;
                }
                DbHelper.UpdateUserVideo(userVideo);
                return RedirectToAction("TeacherVideos", new { id = userVideo.FKClassId });
            }
            return View(userVideo);
        }

        public ActionResult DeleteTeacherVideo(long id)
        {
            DbHelper.DeleteUserVideo(id);
            return View();
        }

        #endregion



        public ActionResult AddClass()
        {
            ViewBag.Cities = new SelectList(DbHelper.GetAllCities(), "PKCityId", "CityName");
            ViewBag.States = new SelectList(DbHelper.GetStates(), "PKStateId", "StateName");
            ViewBag.Subject = new SelectList(DbHelper.GetSubjects(), "PKSubjectId", "SubjectName");

            return View();
        }

        [HttpPost]
        public ActionResult AddClass(Category category, HttpPostedFileBase classImg, IEnumerable<HttpPostedFileBase> image)
        {

            ViewBag.Cities = new SelectList(DbHelper.GetAllCities(), "PKCityId", "CityName");
            ViewBag.States = new SelectList(DbHelper.GetStates(), "PKStateId", "StateName");
            ViewBag.Subject = new SelectList(DbHelper.GetSubjects(), "PKSubjectId", "SubjectName");
            category.Class.FKUserId = Helper.UserId;
            category.Class.IsApproved = true;//Admin Approvell 
            if (image != null)
            {
                if (image.Count() > 0)
                {
                    var img = image.FirstOrDefault();
                    string imageName = System.IO.Path.GetFileName(img.FileName);
                    img.SaveAs(Server.MapPath("~/Images/" + Helper.UserId + "_" + category.Class.FKSubjectId + "_" + imageName));
                    category.Class.ClassImage = "/Images/" + Helper.UserId + "_" + category.Class.FKSubjectId + "_" + imageName;
                }
            }
            //if (classImg != null)
            //{
            //    string imageName = System.IO.Path.GetFileName(classImg.FileName);
            //    classImg.SaveAs(Server.MapPath("~/Images/" + Helper.UserId + "_" + category.Class.FKSubjectId + "_" + imageName));
            //    category.Class.ClassImage = "/Images/" + Helper.UserId + "_" + category.Class.FKSubjectId + "_" + imageName;
            //}
            DbHelper.InsertClass(category.Class);
            category.TeacherProfile.Rating = 5;
            category.TeacherProfile.FKClassId = category.Class.PKClassId;
            category.TeacherProfile.FKUserId = Helper.UserId;
            category.TeacherProfile.IsActive = true;
            DbHelper.InsertTeacherProfile(category.TeacherProfile);
            category.AvalilableTeacher.FKClassId = category.Class.PKClassId;
            category.AvalilableTeacher.IsActive = true;
			category.Class.IsActive = true;
            category.AvalilableTeacher.FKClassId = category.Class.PKClassId;
            DbHelper.InsertAvailableTeacherTime(category.AvalilableTeacher);
            category.userImage = new UserImage();
            category.userImage.FKClassId = category.Class.PKClassId;
            if (image != null)
            {
                foreach (HttpPostedFileBase img in image)
                {
                    if (img != null)
                    {
                        string imageName = System.IO.Path.GetFileName(img.FileName);
                        img.SaveAs(Server.MapPath("~/Images/" + Helper.UserId + "-" + imageName));
                        category.userImage.ImageUrl = "/Images/" + Helper.UserId + "-" + imageName;
                        category.userImage.ImageName = imageName;
                        DbHelper.InsertUserImages(category.userImage);
                    }
                }
            }
            category.UserVideo.FKClassId = category.Class.PKClassId;
            if (!String.IsNullOrEmpty(category.UserVideo.VideoUrl))
            {
                if (category.UserVideo.VideoUrl.Contains("watch") && category.UserVideo.VideoUrl.Contains("youtube.com"))
                {
                    string querystring = category.UserVideo.VideoUrl.Split('=')[1];
                    category.UserVideo.VideoUrl = "https://www.youtube.com/embed/" + querystring;
                }
                DbHelper.InsertUserVideo(category.UserVideo);
            }
            return RedirectToAction("Classes");

        }
    }
}