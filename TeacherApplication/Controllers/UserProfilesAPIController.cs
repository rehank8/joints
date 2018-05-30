using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Domain;

namespace TeacherApplication.Controllers
{
    public class UserProfilesAPIController : ApiController
    {
        private TeachersEntities db = new TeachersEntities();

        // GET: api/UserProfilesAPI
        public IQueryable<UserProfile> GetUserProfiles()
        {
            return db.UserProfiles;
        }

        // GET: api/UserProfilesAPI/5
        [ResponseType(typeof(UserProfile))]
        public IHttpActionResult GetUserProfile(long id)
        {
            UserProfile userProfile = db.UserProfiles.Find(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return Ok(userProfile);
        }

        // PUT: api/UserProfilesAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserProfile(long id, UserProfile userProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userProfile.PKUserId)
            {
                return BadRequest();
            }

            db.Entry(userProfile).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/UserProfilesAPI
        [ResponseType(typeof(UserProfile))]
        public IHttpActionResult PostUserProfile(UserProfile userProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserProfiles.Add(userProfile);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userProfile.PKUserId }, userProfile);
        }

        // DELETE: api/UserProfilesAPI/5
        [ResponseType(typeof(UserProfile))]
        public IHttpActionResult DeleteUserProfile(long id)
        {
            UserProfile userProfile = db.UserProfiles.Find(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            db.UserProfiles.Remove(userProfile);
            db.SaveChanges();

            return Ok(userProfile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserProfileExists(long id)
        {
            return db.UserProfiles.Count(e => e.PKUserId == id) > 0;
        }
    }
}