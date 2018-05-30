using Data_Access;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeacherApplication.Controllers
{
    [HandleError]
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        #region Userprofile
        //Get:UserProfile List
        public ActionResult UserProfiles()
        {
            return View(DbHelper.GetUserProfiles());
        }

        [HttpPost]
        public ActionResult UserProfiles(FormCollection fc)
        {
            string emailId = fc["EmailId"] ?? null;
            int roleId = Convert.ToInt32(fc["RoleType"] ?? "0");
            return View(DbHelper.GetUserProfiles(emailId,roleId));
        }
        //Edit: UserProfile based on Id
        public ActionResult UserProfile(long id = 0)
        {
            return View(DbHelper.GetUserProfile(id));
        }
        [HttpPost]
        public ActionResult UserProfile(UserProfile objUserProfile, HttpPostedFileBase photo)
        {
            if (photo != null)
            {
                string imageName = System.IO.Path.GetFileName(photo.FileName);
                photo.SaveAs(Server.MapPath("~/Images/" + Helper.UserId + "-" + imageName));
                objUserProfile.PhotoUrl = "/Images/" + Helper.UserId + "-" + imageName;
            }
            if (objUserProfile.FKRoleId == 1)
                objUserProfile.IsActive = true;
            DbHelper.UpdateUsers(objUserProfile);
            return View(DbHelper.GetUserProfile(objUserProfile.PKUserId));
        }
        //Get UserProfileDetails Based on Id
        public ActionResult UserProfileDetails(long id = 0)
        {
            return View(DbHelper.GetUserProfile(id));
        }
        #endregion

        #region AvailableTeacherTime
        //Get AvailableTeacherTime
        public ActionResult AvailableTeacherTime()
        {
            return View(DbHelper.GetAvailableTeacherTimes());
        }
        //Get AvailableTeacherTime Details based on Id
        public ActionResult AvailableTeacherTimeDetails(long id = 0)
        {
            return View(DbHelper.GetAvailableTeacherTime(id));
        }
        //Edit Available Teacher Time based on Id
        public ActionResult EditAvailableTeacherTime(long id = 0)
        {
            return View(DbHelper.GetAvailableTeacherTime(id));
        }
        [HttpPost]
        public ActionResult EditAvailableTeacherTime(AvailableTeacherTime objAvailableTeacher)
        {
            DbHelper.UpdateAvailableTeacherTime(objAvailableTeacher);
            return View(objAvailableTeacher);
        }
        #endregion

        #region class
        //Get Class
        public ActionResult Class()
        {
            return View(DbHelper.GetClasses());
        }
        //Get Class on Id
        public ActionResult ClassDetails(int id)
        {
            return View(DbHelper.GetClass(id));
        }
        //Get Class Edit on Id
        public ActionResult EditClass(int id)
        {
            Class cls = DbHelper.GetClass(id);
            ViewBag.Subject = new SelectList(DbHelper.GetSubjects(), "PKSubjectId", "SubjectName", cls.FKSubjectId);
            return View(cls);
        }
        [HttpPost]
        public ActionResult EditClass(Class objClass)
        {
            ViewBag.Subject = new SelectList(DbHelper.GetSubjects(), "PKSubjectId", "SubjectName", objClass.FKSubjectId);
            DbHelper.UpdateClass(objClass);
            return View(objClass);
        }
        #endregion

        #region TeacherProfile
        //Get TeacherProfile
        public ActionResult TeacherProfile()
        {
            return View(DbHelper.GetTeachers());
        }
        //Get TeacherProfile Details on Id
        public ActionResult TeacherprofileDetails(int id)
        {
            return View(DbHelper.GetTeacher(id));
        }
        //get TeacherProfile Edit on Id
        public ActionResult EditTeacherProfile(int id)
        {
            var teacherProfile = DbHelper.GetTeacher(id);
            ViewBag.TeacherName = teacherProfile.UserProfile.UserName;
            return View(teacherProfile);
        }
        [HttpPost]
        public ActionResult EditTeacherProfile(TeacherProfile objTeacherProfile)
        {
            DbHelper.UpdateTeacherProfile(objTeacherProfile);
            return View(objTeacherProfile);
        }
        #endregion

        #region LoginHistory
        //Get LoginHistory
        public ActionResult LoginHistory()
        {
            return View(DbHelper.GetLoginHistories());
        }
        #endregion

        #region UserEnquires
        //Get UserEnquires
        public ActionResult UserEnquires()
        {
            return View(DbHelper.GetUserEnquiries());
        }
        //Get UsereEnquiryDetails Based on id
        public ActionResult UserEnquiryDetails(int id)
        {
            return View(DbHelper.GetUserEnquiry(id));
        }
        #endregion

        #region subjects
        public ActionResult Subjects()
        {
            return View(DbHelper.GetSubjects());
        }
        public ActionResult CreateSubject()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateSubject(Subject objSubject)
        {
            if (ModelState.IsValid)
            {
                DbHelper.InsertSubject(objSubject);
                return View("Index");
            }
            return View();
        }

        public ActionResult EditSubject(long id)
        {
            return View(DbHelper.GetSubject(id));
        }
        [HttpPost]
        public ActionResult EditSubject(Subject objSubject)
        {
            if (ModelState.IsValid)
            {
                DbHelper.UpdateSubject(objSubject);
                return View("Index");
            }
            return View(objSubject);
        }

        #endregion

        #region States
        public ActionResult States()
        {
            return View(DbHelper.GetStates());
        }

        public ActionResult CreateandEditState(State objState)
        {
            if (objState.PKStateId != 0)
                DbHelper.UpdateState(objState);
            else
                DbHelper.InsertState(objState);
            return RedirectToAction("States");
        }

        public ActionResult EditState(long id)
        {
            State state = DbHelper.GetState(id);
            return Json(state, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Deletestate(long id)
        {
            DbHelper.DeleteState(id);
            return RedirectToAction("States");
        }
        #endregion


        #region Cities
        public ActionResult Cities()
        {
            ViewBag.States = new SelectList(DbHelper.GetStates(), "PKStateId", "StateName");
            return View(DbHelper.GetAllCities());
        }

        public ActionResult CreateandEditCity(City objCity)
        {
            if (objCity.PKCityId != 0)
                DbHelper.UpdateCity(objCity);
            else
                DbHelper.InsertCity(objCity);
            return RedirectToAction("Cities");
        }

        public ActionResult EditCity(long id)
        {
            City city = DbHelper.GetCity(id);
            ViewBag.States = new SelectList(DbHelper.GetStates(), "PKStateId", "StateName", city.FKStateId);
            return Json(city, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCity(long id)
        {
            DbHelper.DeleteCity(id);
            return RedirectToAction("Cities");
        }
        #endregion

    }
}