using Data_Access;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TeacherApplication.Models;

namespace TeacherApplication.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {
        LoginHistory objLoginHistory = new LoginHistory();
        WebClient webClient = new WebClient();

        // GET: /User/

        public ActionResult Login(string returnurl)
        {
            ViewBag.ReturnUrl = returnurl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel objLoginModel, string returnurl="")
        {
            UserProfile objUserProfile = DbHelper.AuthenticateUser(objLoginModel.UserName);
            if (objUserProfile != null)
            {
                if (objLoginModel.Password == objUserProfile.Password)
                {
                    string userData = objUserProfile.PKUserId + "^" + objUserProfile.FKRoleId + "^" + objUserProfile.Role.RoleName + "^" + objUserProfile.UserName + "^" + objUserProfile.EmailId + "^" + objUserProfile.Password + "^" + objUserProfile.Latitude + "^" + objUserProfile.Longitude + "^" + objUserProfile.PhotoUrl;
                    System.Web.HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(userData), new string[] { userData });
                    FormsAuthentication.SetAuthCookie(Helper.UserData, objLoginModel.RememberMe);
                    Session["UserData"] = userData;
                    //Inserting Login History of user
                    objLoginHistory.FKUserId = Helper.UserId;
                    objLoginHistory.LoginDate = DateTime.Now;
                    //HttpRequestBase _httpRequest=null;
                    //_httpRequest.RequestContext = System.Web.HttpContext.Current.Request;
                    //objLoginHistory.IPAddress = HelperRequests.GetClientIpAddress(_httpRequest);
                    //string ip = webClient.DownloadString("http://icanhazip.com/");
                    //objLoginHistory.IPAddress = ip;
                    objLoginHistory.IPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
                    DbHelper.InsertLoginHistory(objLoginHistory);

                    if (objUserProfile.Role.RoleName == "Admin")
                    {
                        if (returnurl.Contains("Admin"))
                            return Redirect(returnurl);
                        else
                            return RedirectToAction("Index", "Admin");
                    }
                    else if (objUserProfile.Role.RoleName == "Teacher"|| objUserProfile.Role.RoleName=="Vender")
                    {
                        if (returnurl.Contains("/Teacher/"))
                            return Redirect(returnurl);
                        else
                            return RedirectToAction("Index", "Teacher");
                    }
                    else if (objUserProfile.Role.RoleName == "User")
                    {
                        if (!string.IsNullOrEmpty(returnurl))
                            return Redirect(returnurl);
                        else
                            return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Password");
                    TempData["errorMessage"] = "Invalid Password";
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid UserName");
                TempData["errorMessage"] = "Invalid Username";
            }
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            var userDetails = Session["UserData"];
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            foreach (var cookie in Request.Cookies.AllKeys)
            {
                Request.Cookies.Remove(cookie);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Registration()
        {
            Roleslst();
            return View();
        }
        [HttpPost]
        public ActionResult Registration(UserProfile objUserProfile)
        {
            if (ModelState.IsValid)
            {
                objUserProfile.IsActive = true;
                if (string.IsNullOrEmpty(objUserProfile.Latitude))
                    objUserProfile.Latitude = "0";
                if (string.IsNullOrEmpty(objUserProfile.Longitude))
                    objUserProfile.Longitude = "0";
                DbHelper.InsertUsers(objUserProfile);
                TempData["Message"] = "Registered Sucessfully Please Login";
                return View("Login");
            }
            Roleslst();
            return View();

        }

        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(string emailId)
        {
            UserProfile objUserProfile = DbHelper.AuthenticateUser(emailId);
            if(objUserProfile==null)
            {
                ViewBag.ErrorMessage = "Enter the vaild Emailid";
                return View();
            }
            //StringBuilder str = new StringBuilder();
            //str.Append("Dear " + objUserProfile.UserName + ",");
            //str.Append("<br/><br/>");
            //str.Append($"Your Password was:-"+ objUserProfile.Password);
            //str.Append($"<p>This is an automatically generated message to confirm receipt of your Booking via the Internet.You do not need to reply to this e - mail, but you may wish to save it for your records.<br/>Thank you.</p>");
            //str.Append($"<br/>WarmRegards,<br/> SupportTeam");
            //GmailHelper.Send(objUserProfile.EmailId, "Forget Password", str.ToString());
            ViewBag.ForgetPasswordMessage = "Please check your email, the password was sent to your email id";
            return RedirectToAction("Login");
        }
        
        public void Roleslst()
        {
            List<DPRoles> lstRoles = new List<DPRoles>() { new DPRoles() { PKRoleId = 3, RoleName = "User" }, new DPRoles() { PKRoleId = 2, RoleName = "Vender" } };
            ViewBag.Roles = new SelectList(lstRoles, "PKRoleId", "RoleName");
        }
    }
}