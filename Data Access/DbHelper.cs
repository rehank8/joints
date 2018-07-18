using Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;


namespace Data_Access
{
    public static class DbHelper
    {
        static ObjectCache cache = MemoryCache.Default;
        #region UserProfile
        //UserProfile 
        public static UserProfile AuthenticateUser(string userName)
        {
            TeachersEntities db = new TeachersEntities();
            return db.UserProfiles.Where(up => up.EmailId == userName && up.IsActive == true).SingleOrDefault();
        }
        public static UserProfile GetUserProfile(long userId)
        {
            TeachersEntities db = new TeachersEntities();
            return db.UserProfiles.Where(up => up.PKUserId == userId).SingleOrDefault();
        }
        public static List<UserProfile> GetUserProfiles(string emaild = null, int roletype = 0)
        {
            IQueryable<UserProfile> qry = null;
            if (cache["UserProfile"] == null)
            {
                TeachersEntities db = new TeachersEntities();
                qry = db.UserProfiles;
                cache["UserProfile"] = qry;
            }
            if (qry == null)
                qry = cache["UserProfile"] as IQueryable<UserProfile>;
            if (!string.IsNullOrEmpty(emaild))
                qry = qry.Where(up => up.EmailId.Contains(emaild));
            if (roletype > 0)
                qry = qry.Where(up => up.FKRoleId == roletype);
            return qry.ToList();
        }
        public static void InsertUsers(UserProfile objUser)
        {
            TeachersEntities db = new TeachersEntities();
            db.UserProfiles.Add(objUser);
            db.SaveChanges();
            cache.Remove("UserProfile");
        }
        public static void UpdateUsers(UserProfile objuser)
        {
            TeachersEntities db = new TeachersEntities();
            db.Entry(objuser).State = EntityState.Modified;
            db.SaveChanges();
            cache.Remove("UserProfile");
        }
        public static void DeleteUsers(int userId)
        {
            TeachersEntities db = new TeachersEntities();
            UserProfile objUser = db.UserProfiles.Find(userId);
            db.UserProfiles.Remove(objUser);
            db.SaveChanges();
            cache.Remove("UserProfile");
        }
        #endregion

        #region Class
        //Class
        public static Class GetClass(int classId)
        {
            TeachersEntities db = new TeachersEntities();
            return db.Classes.Where(c => c.PKClassId == classId).SingleOrDefault();
        }
		public static TeacherProfile GetTeacherProfile(int classId)
		{
			TeachersEntities db = new TeachersEntities();
			return db.TeacherProfiles.Where(c => c.FKClassId == classId).SingleOrDefault();
		}
		public static List<Class> GetClasses(long userId = 0, bool? isApproved = null,bool? isActive=null)
        {
            TeachersEntities db = new TeachersEntities();
           
            var qry = from c in db.Classes
                      where c.FKUserId == ((userId == 0) ? c.FKUserId : userId)
                      select c;
			if (isActive != null)
				qry = qry.Where(x=>x.IsActive==isActive.Value);

            if (isApproved != null)
            {
                qry = qry.Where(c => c.IsApproved == isApproved);
            }
            return qry.ToList();
            //return db.Classes.ToList();
        }
        public static void InsertClass(Class objcls)
        {
			try
			{
				TeachersEntities db = new TeachersEntities();
				db.Classes.Add(objcls);
				db.SaveChanges();
				cache.Remove("GetTeachersHome");
			}


			catch (Exception ex)
			{
				//throw ex;
			}
        }
        public static void UpdateClass(Class objcls)
        {
			
            TeachersEntities db = new TeachersEntities();
			try
			{
				db.Entry(objcls).State = EntityState.Modified;
				db.SaveChanges();
				cache.Remove("GetTeachersHome");
			}
			catch (Exception)
			{
				var oldEntry = db.TeacherProfiles.Find(objcls.PKClassId);
				db.Entry(oldEntry).CurrentValues.SetValues(objcls);
				db.SaveChanges();
				cache.Remove("GetTeachersHome");
			}
		}
        public static void DeleteClass(int classId)
        {
            TeachersEntities db = new TeachersEntities();
            Class objClass = db.Classes.Find(classId);
            db.Classes.Remove(objClass);
            db.SaveChanges();
            cache.Remove("GetTeachersHome");
        }
        #endregion

        #region TeacherProfile
        //Teacher Profile
        public static TeacherProfile GetTeacher(long teacherProfileId)
        {

            TeachersEntities db = new TeachersEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.TeacherProfiles.Include("UserProfile").Where(t => t.PKTeachersId == teacherProfileId).SingleOrDefault();
        }

        public static List<TeacherProfile> GetTeachers(long userId = 0)
        {
            TeachersEntities db = new TeachersEntities();
            var tch = from t in db.TeacherProfiles
                      join u in db.UserProfiles
                      on t.FKUserId equals u.PKUserId
                      where t.FKUserId == ((userId > 0) ? userId : t.FKUserId)
                      select t;
            return tch.ToList();
        }
        public static void InsertTeacherProfile(TeacherProfile objTeacherProfile)
        {
            TeachersEntities db = new TeachersEntities();
            db.TeacherProfiles.Add(objTeacherProfile);
            db.SaveChanges();
            cache.Remove("GetTeachersHome");
        }
        public static void UpdateTeacherProfile(TeacherProfile objTeacher)
        {
            TeachersEntities db = new TeachersEntities();
            try
            {
                db.Entry(objTeacher).State = EntityState.Modified;
				db.SaveChanges();
                cache.Remove("GetTeachersHome");
            }
            catch (Exception)
            {
                TeacherProfile oldEntry = db.TeacherProfiles.Find(objTeacher.PKTeachersId);
                db.Entry(oldEntry).CurrentValues.SetValues(objTeacher);
                db.SaveChanges();
                cache.Remove("GetTeachersHome");
            }
        }
        public static void DeleteTeacherProfile(int teacherProfileId)
        {
            TeachersEntities db = new TeachersEntities();
            TeacherProfile objTeacherProfile = db.TeacherProfiles.Find(teacherProfileId);
            db.TeacherProfiles.Remove(objTeacherProfile);
            db.SaveChanges();
            cache.Remove("GetTeachersHome");
        }
        #endregion


        #region Teacher Videos
        public static List<UserVideo> GetUservideos(long classId = 0)
        {
            TeachersEntities db = new TeachersEntities();
            return db.UserVideos.Where(x => x.FKClassId == classId).ToList();
        }


        public static UserVideo GetUserVideo(long userVideoId)
        {
            TeachersEntities db = new TeachersEntities();
            return db.UserVideos.Where(x => x.PKUserVideoId == userVideoId).SingleOrDefault();
        }

        public static void InsertUserVideo(UserVideo userVideo)
        {
            TeachersEntities db = new TeachersEntities();
            db.UserVideos.Add(userVideo);
            db.SaveChanges();
        }

        public static void UpdateUserVideo(UserVideo userVideo)
        {
            TeachersEntities db = new TeachersEntities();
            db.Entry(userVideo).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void DeleteUserVideo(long id)
        {
            TeachersEntities db = new TeachersEntities();
            UserVideo userVideo = db.UserVideos.Find(id);
            db.UserVideos.Remove(userVideo);
            db.SaveChanges();
        }
        #endregion

        #region UserImages&Teacher
        //UserImages
        public static UserImage GetUserImage(int userImageId)
        {
            TeachersEntities db = new TeachersEntities();
            return db.UserImages.Find(userImageId);
        }
        public static List<UserImage> GetUserImages(long userId = 0, long classId = 0)
        {
            TeachersEntities db = new TeachersEntities();
            if (userId != 0)
                return db.UserImages.Include(x => x.Class).Where(x => x.Class.FKUserId == userId).ToList();
            else
                return db.UserImages.Where(x => x.FKClassId == classId).ToList();
        }
        public static void InsertUserImages(UserImage objUserImage)
        {
            TeachersEntities db = new TeachersEntities();
            db.UserImages.Add(objUserImage);
            db.SaveChanges();
        }
        public static void UpdateUserImages(UserImage objUserImage)
        {
            TeachersEntities db = new TeachersEntities();
            db.Entry(objUserImage).State = EntityState.Modified;
            db.SaveChanges();
        }
        public static void DeleteUserImages(int userImageId)
        {
            TeachersEntities db = new TeachersEntities();
            UserImage objUserImage = db.UserImages.Find(userImageId);
            db.UserImages.Remove(objUserImage);
            db.SaveChanges();
        }
        #endregion

        #region UserLoginHistory
        //UserLoginHistory
        public static LoginHistory GetLoginHistory(int loginHistoryId)
        {
            TeachersEntities db = new TeachersEntities();
            return db.LoginHistories.Find(loginHistoryId);
        }
        public static List<LoginHistory> GetLoginHistories()
        {
            TeachersEntities db = new TeachersEntities();
            return db.LoginHistories.ToList();
        }
        public static void InsertLoginHistory(LoginHistory objLoginHistory)
        {
            TeachersEntities db = new TeachersEntities();
            db.LoginHistories.Add(objLoginHistory);
            db.SaveChanges();
        }
        public static void UpdateLoginHistory(LoginHistory objLoginHistory)
        {
            TeachersEntities db = new TeachersEntities();
            db.Entry(objLoginHistory).State = EntityState.Modified;
            db.SaveChanges();
        }
        public static void DeleteLoginHistory(int loginHistoryId)
        {
            TeachersEntities db = new TeachersEntities();
            LoginHistory objLoginHistory = db.LoginHistories.Find(loginHistoryId);
            db.LoginHistories.Remove(objLoginHistory);
            db.SaveChanges();
        }
        #endregion

        #region UserEnquiry
        //UserEnquiry
        public static UserEnquiry GetUserEnquiry(long userEnquiryId = 0)
        {
            TeachersEntities db = new TeachersEntities();
            return db.UserEnquiries.Where(ue => ue.PKEnquiryId == userEnquiryId).SingleOrDefault();
        }
        public static List<UserEnquiry> GetUserEnquiries(long teachId = 0)
        {
            TeachersEntities db = new TeachersEntities();
            IQueryable<UserEnquiry> qry = db.UserEnquiries;

            var q = (from ue in qry
                     join u in db.UserProfiles on ue.FKUserId equals u.PKUserId
                     join t in db.TeacherProfiles on ue.FKTeacherId equals t.PKTeachersId
                     // join a in db.AvailableTeacherTimes on ue.FKAvailableTimeId equals a.PKAvailableTeacherTimeId
                     where ue.FKTeacherId == ((teachId == 0) ? ue.FKTeacherId : teachId)
                     select new
                     {
                         PKEnquiryId = ue.PKEnquiryId,
                         FKUserId = ue.FKUserId,
                         FKTeacherId = ue.FKTeacherId,
                         FKAvailableTimeId = ue.FKAvailableTimeId,
                         FKClassId = ue.FKClassId,
                         UserName = u.UserName,
                         TeacherName = t.UserProfile.UserName,
                         BookedFromDate = ue.BookedFromDate,
                         BookedToDate = ue.BookedToDate,
                         BookedFromTime = ue.BookedFromTime,
                         BookedToTime = ue.BookedToTime,
                         CreatedDate = ue.CreatedDate,
                         BookingStatus = ue.BookingStatus,
                         FollowUP = ue.FollowUP,
                         PhoneNo = u.PhoneNo,
                         EmailId = u.EmailId,
                         subject = t.Class.Subject.SubjectName
                     }).AsEnumerable().Select(x => new UserEnquiry
                     {
                         PKEnquiryId = x.PKEnquiryId,
                         FKUserId = x.FKUserId,
                         FKTeacherId = x.FKTeacherId,
                         FKAvailableTimeId = x.FKAvailableTimeId,
                         FKClassId = x.FKClassId,
                         UserName = x.UserName,
                         TeacherName = x.TeacherName,
                         BookedFromDate = x.BookedFromDate,
                         BookedFromTime = x.BookedFromTime,
                         BookedToTime = x.BookedToTime,
                         BookedToDate = x.BookedToDate,
                         CreatedDate = x.CreatedDate,
                         BookingStatus = x.BookingStatus,
                         FollowUP = x.FollowUP,
                         PhoneNo = x.PhoneNo,
                         EmailId = x.EmailId,
                         Subject = x.subject
                     });

            return q.ToList();
        }

        public static void InsertUserEnquiry(UserEnquiry objUserEnquiry)
        {
            TeachersEntities db = new TeachersEntities();
            db.UserEnquiries.Add(objUserEnquiry);
            db.SaveChanges();
        }
        public static void UpdateUserEnquiry(UserEnquiry objUserEnquiry)
        {
            TeachersEntities db = new TeachersEntities();
            db.Entry(objUserEnquiry).State = EntityState.Modified;
            db.SaveChanges();
        }
        public static void DeleteUserEnquiry(int userEnquiryId)
        {
            TeachersEntities db = new TeachersEntities();
            UserEnquiry objUserEnquiry = db.UserEnquiries.Find(userEnquiryId);
            db.UserEnquiries.Remove(objUserEnquiry);
            db.SaveChanges();
        }
        #endregion

        #region Available Teacher Time
        //Available Teacher Time
        public static AvailableTeacherTime GetAvailableTeacherTime(long availableTeacherTimeId)
        {
            TeachersEntities db = new TeachersEntities();
            return db.AvailableTeacherTimes.Find(availableTeacherTimeId);
        }
        public static List<AvailableTeacherTime> GetAvailableTeacherTimes(long userId = 0)
        {
            TeachersEntities db = new TeachersEntities();
            var availableTeacher = from at in db.AvailableTeacherTimes
                                   join cls in db.Classes
                                   on at.FKClassId equals cls.PKClassId
                                   where cls.FKUserId == ((userId != 0) ? userId : cls.FKUserId)
                                   select at;
            return availableTeacher.ToList();
            //return db.AvailableTeacherTimes.ToList();
        }
        public static void InsertAvailableTeacherTime(AvailableTeacherTime objAvailableTeacherTime)
        {
            TeachersEntities db = new TeachersEntities();
            db.AvailableTeacherTimes.Add(objAvailableTeacherTime);
            db.SaveChanges();
            cache.Remove("GetTeachersHome");
        }
        public static void UpdateAvailableTeacherTime(AvailableTeacherTime objAvailableTeacherTime)
        {
            TeachersEntities db = new TeachersEntities();
			db.Entry(objAvailableTeacherTime).State = EntityState.Modified;
			db.SaveChanges();
			//db.AvailableTeacherTimes.Add(objAvailableTeacherTime);
			//db.SaveChanges();
			cache.Remove("GetTeachersHome");

        }
        public static void DeleteAvailableTeacherTime(long availableTeacherTimeId)
        {
            TeachersEntities db = new TeachersEntities();
            AvailableTeacherTime objAvailableTeacherTime = db.AvailableTeacherTimes.Find(availableTeacherTimeId);
            db.AvailableTeacherTimes.Remove(objAvailableTeacherTime);
            db.SaveChanges();
            cache.Remove("GetTeachersHome");
        }
        #endregion

        #region Roles
        //Roles
        public static List<Role> GetRoles()
        {
            TeachersEntities db = new TeachersEntities();
            return db.Roles.ToList();
        }
        #endregion

        #region Homepage(Getting of Userprofile,Class,Teacher&Time)
        //UserIndex
        public static List<UserIndex> GetTeachersForHome(string subject = null, int cityId = 0, int stateId = 0)
        {
            TeachersEntities db = new TeachersEntities();
            //using (TeachersEntities db = new TeachersEntities())
            //{
          //  by mohammed connection discconedted issue
            IQueryable<UserIndex> query = null;
            if (cache["GetTeachersHome"] == null)
            {
                
                query = (from up in db.UserProfiles
                             join tp in db.TeacherProfiles
                             on up.PKUserId equals tp.FKUserId
                             join cls in db.Classes
                             on tp.FKClassId equals cls.PKClassId
						 //join at in db.AvailableTeacherTimes
						 //on cls.PKClassId equals at.FKClassId
						 join ct in db.Cities
						 on cls.FKCityId equals ct.PKCityId
						 join sub in db.Subjects
                             on cls.FKSubjectId equals sub.PKSubjectId
						 where cls.IsActive== true
                             //where sub.SubjectName == ((subject == null) ? sub.SubjectName : subject) //|| cls.IsApproved == true
                             select new UserIndex
                             {
                                 PKUserId = up.PKUserId,
                                 TeacherName = up.UserName,
                                 PhotoUrl = cls.ClassImage,
                                 ClassName = cls.ClassName,
                                 SubjectName = sub.SubjectName,
                                 Experience = tp.Experience,
                                 Description = tp.Description,
								 //AvailableDate = at.FromAvailableDate.ToString(),
								 //AvailableTime = at.FromAvailableTime.ToString(),
								 //ToDate = at.ToAvailableDate.ToString(),
								 //ToTime = at.ToAvailableTime.ToString(),
								 Location = ct.CityName,
								 Rating = tp.Rating.Value,
                                 Teacherid = tp.PKTeachersId,
                                // PKAvailableTeacherId = at.PKAvailableTeacherTimeId,
                                 EmailId = up.EmailId,
                                 FKRoleId = up.FKRoleId,
								 CityId = ct.PKCityId,
								 StateId = ct.FKStateId,
								 ClassId = cls.PKClassId,
                                 CityName = cls.AreaName
                             });
                cache["GetTeachersHome"] = query;
            }
            var q = cache["GetTeachersHome"] as IEnumerable<UserIndex>;
            if (!string.IsNullOrEmpty(subject))
            {
                q = q.Where(x => x.SubjectName == subject);
            }
            if (cityId != 0)
            {
                q = q.Where(x => x.CityId == cityId).Select(x => x);
            }
            else if (stateId != 0)
            {
                q = q.Where(x => x.StateId == stateId).Select(x => x);
            }
            // by mohammed for avoiding connnection fail to open
            //  var q1 = q.GroupBy(x => x.EmailId).Select(x => x.FirstOrDefault()).ToList();yerror
            return q.ToList();
        
        }
        #endregion

        #region Teacher Details base on Id
        public static TeacherTimeDetails GetTeacherTimeDetails(long classId)
        {
            TeachersEntities db = new TeachersEntities();
            var qry = from up in db.UserProfiles
                      join cls in db.Classes on up.PKUserId equals cls.FKUserId
                      join at in db.AvailableTeacherTimes on cls.PKClassId equals at.FKClassId
                      join tp in db.TeacherProfiles on cls.PKClassId equals tp.FKClassId
                      join sub in db.Subjects on cls.FKSubjectId equals sub.PKSubjectId
                      where cls.PKClassId == classId
                      select new TeacherTimeDetails
                      {
                          UserId = up.PKUserId,
                          ClassId = cls.PKClassId,
                          TeacherName = up.UserName,
                          FromAvailableDate = at.FromAvailableDate.ToString(),
                          ToAvailableDate = at.ToAvailableDate.ToString(),
                          FromAvailableTime = at.FromAvailableTime.ToString(),
                          ToAvailableTime = at.ToAvailableTime.ToString(),
                          SubjectName = sub.SubjectName,
                          Rating = tp.Rating.Value,
                          TeacherImageUrl = up.PhotoUrl,
                          TeacherId = tp.PKTeachersId,
                          EmailId = up.EmailId,
                          PhoneNo = up.PhoneNo,
                          Description = tp.Description
                      };
            //var q1 = qry.GroupBy(x => x.EmailId).Select(x => x.FirstOrDefault()).ToList();
            // return qry.ToList(); ;
            return qry.FirstOrDefault();
        }
        #endregion

        #region Teacher To Book details
        public static TeacherTimeDetails TeacherToBookDetails(long classId)
        {
            TeachersEntities db = new TeachersEntities();
            var qry = from up in db.UserProfiles
                      join cls in db.Classes on up.PKUserId equals cls.FKUserId
                      join at in db.AvailableTeacherTimes on cls.PKClassId equals at.FKClassId
                      join tch in db.TeacherProfiles on cls.PKClassId equals tch.FKClassId
                      join sub in db.Subjects on cls.FKSubjectId equals sub.PKSubjectId
                      where cls.PKClassId == classId
                      select new TeacherTimeDetails
                      {
                          UserId = up.PKUserId,
                          ClassId = cls.PKClassId,
                          TeacherId = tch.PKTeachersId,
                          AvailableTeacherTimeId = at.PKAvailableTeacherTimeId,
                          TeacherName = up.UserName,
                          //ImageName = ui.ImageName,
                          //ImageUrl = ui.ImageUrl,
                          //VideoUrl = ui.VideoUrl,
                          FromAvailableDate = at.FromAvailableDate.ToString(),
                          ToAvailableDate = at.ToAvailableDate.ToString(),
                          FromAvailableTime = at.FromAvailableTime.ToString(),
                          ToAvailableTime = at.ToAvailableTime.ToString(),
                          SubjectName = sub.SubjectName,
                          EmailId = up.EmailId
                      };
            // var q1 = qry.GroupBy(x => x.EmailId).Select(x => x.FirstOrDefault()).SingleOrDefault();
            return qry.SingleOrDefault();
        }
        #endregion


        #region Subjects
        public static List<Subject> GetSubjects()
        {

            if (cache["GetSubjects"] == null)
            {
                using (TeachersEntities db = new TeachersEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    cache["GetSubjects"] = db.Subjects.ToList();

                }
            }
            return cache["GetSubjects"] as List<Subject>;
        }

        public static Subject GetSubject(long subjectId)
        {
            TeachersEntities db = new TeachersEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.Subjects.Where(s => s.PKSubjectId == subjectId).SingleOrDefault();
        }

        public static void InsertSubject(Subject objSubject)
        {
            using (TeachersEntities db = new TeachersEntities())
            {
                db.Subjects.Add(objSubject);
                db.SaveChanges();
            }
            cache.Remove("GetSubjects");
        }

        public static void UpdateSubject(Subject objSubject)
        {
            using (TeachersEntities db = new TeachersEntities())
            {
                db.Entry(objSubject).State = EntityState.Modified;
                db.SaveChanges();
            }
            cache.Remove("GetSubjects");
        }

        public static void DeleteSubject(long subjectId)
        {
            using (TeachersEntities db = new TeachersEntities())
            {
                Subject objSubject = db.Subjects.Find(subjectId);
                db.Subjects.Remove(objSubject);
                db.SaveChanges();
            }
            cache.Remove("GetSubjects");
        }
        #endregion

        #region Cities
        //List of Cities
        public static List<UserProfile> GetCities()
        {
            TeachersEntities db = new TeachersEntities();
            var q = db.UserProfiles.Distinct().GroupBy(x => x.City).Select(x => x.FirstOrDefault()).Where(x => x.FKRoleId == 2);
            return q.ToList();
        }
		#endregion

		#region City

		

		public static List<City> GetAllCities(int stateId = 0)
        {
            using (TeachersEntities db = new TeachersEntities())
            {
                if (stateId != 0)
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.Cities.Where(x => x.FKStateId == stateId).ToList();
                }
                else
                    return db.Cities.Include(x => x.State).ToList();
            }

        }
        public static City GetCity(long CityId)
        {
            using (TeachersEntities db = new TeachersEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Cities.Find(CityId);
            }

        }

        public static void InsertCity(City objCity)
        {
            using (TeachersEntities db = new TeachersEntities())
            {

                db.Cities.Add(objCity);
                db.SaveChanges();
            }
        }

        public static void UpdateCity(City objCity)
        {

            using (TeachersEntities db = new TeachersEntities())
            {

                db.Entry(objCity).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static void DeleteCity(long cityId)
        {
            using (TeachersEntities db = new TeachersEntities())
            {
                City objCity = db.Cities.Find(cityId);
                db.Cities.Remove(objCity);
                db.SaveChanges();
            }
        }
        #endregion

        #region States
        public static List<State> GetStates()
        {
            if (cache["GetStates"] == null)
            {
                using (TeachersEntities db = new TeachersEntities())
                {

                    cache["GetStates"] = db.States.ToList();
                }
            }
            return cache["GetStates"] as List<State>;
        }
        public static State GetState(long stateId)
        {

            using (TeachersEntities db = new TeachersEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.States.Where(x => x.PKStateId == stateId).FirstOrDefault();
            }
        }

        public static void InsertState(State objState)
        {
            using (TeachersEntities db = new TeachersEntities())
            {

                db.States.Add(objState);
                db.SaveChanges();
                cache.Remove("GetStates");
            }
        }

        public static void UpdateState(State objState)
        {

            using (TeachersEntities db = new TeachersEntities())
            {

                db.Entry(objState).State = EntityState.Modified;
                db.SaveChanges();
                cache.Remove("GetStates");
            }
        }

        public static void DeleteState(long stateId)
        {
            using (TeachersEntities db = new TeachersEntities())
            {
                State objState = db.States.Find(stateId);
                db.States.Remove(objState);
                db.SaveChanges();
                cache.Remove("GetStates");
            }
        }
        #endregion



        #region UserComments Section
        public static List<UserComment> GetUserComments(long classId)
        {
            using (TeachersEntities db = new TeachersEntities())
            {
                return db.UserComments.Where(x => x.FKClassId == classId).ToList();
            }

        }

        public static void AddUserComments(UserComment userComments)
        {
            using (TeachersEntities db = new TeachersEntities())
            {
                db.UserComments.Add(userComments);
                db.SaveChanges();
            }
        }
        #endregion
    }
}