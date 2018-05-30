using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeacherApplication
{
    public class Helper
    {
        public static string UserData
        {
            get
            {
                return System.Web.HttpContext.Current.User.Identity.Name;
            }
        }
        public static int RoleId
        {
            get
            {
                return Convert.ToInt32(UserData.Split('^')[1]);
            }
        }

        public static string RoleName
        {
            get
            {
                return UserData.Split('^')[2];
            }
        }
        public static int UserId
        {
            get
            {
                return Convert.ToInt32(UserData.Split('^')[0]);
            }

        }
        public static string UserName
        {
            get
            {
                return UserData.Split('^')[3];
            }
        }

        public static string EmailId
        {
            get
            {
                return UserData.Split('^')[4];
            }
        }
        public static string Password
        {
            get
            {
                return UserData.Split('^')[5];
            }
        }

        public static DateTime Latitude
        {
            get
            {
                return Convert.ToDateTime(UserData.Split('^')[6]);
            }
        }
        public static DateTime Longitude
        {
            get
            {
                return Convert.ToDateTime(UserData.Split('^')[7]);
            }
        }
        public static string PhotoUrl
        {
            get
            {
                return UserData.Split('^')[8];
            }
        }

    }

}