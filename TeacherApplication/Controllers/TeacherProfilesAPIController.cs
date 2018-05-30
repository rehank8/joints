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
    public class TeacherProfilesAPIController : ApiController
    {
        private TeachersEntities db = new TeachersEntities();

        // GET: api/TeacherProfilesAPI
        public IQueryable<TeacherProfile> GetTeacherProfiles()
        {
            return db.TeacherProfiles;
        }

        // GET: api/TeacherProfilesAPI/5
        [ResponseType(typeof(TeacherProfile))]
        public IHttpActionResult GetTeacherProfile(long id)
        {
            TeacherProfile teacherProfile = db.TeacherProfiles.Find(id);
            if (teacherProfile == null)
            {
                return NotFound();
            }

            return Ok(teacherProfile);
        }

        // PUT: api/TeacherProfilesAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTeacherProfile(long id, TeacherProfile teacherProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teacherProfile.PKTeachersId)
            {
                return BadRequest();
            }

            db.Entry(teacherProfile).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherProfileExists(id))
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

        // POST: api/TeacherProfilesAPI
        [ResponseType(typeof(TeacherProfile))]
        public IHttpActionResult PostTeacherProfile(TeacherProfile teacherProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TeacherProfiles.Add(teacherProfile);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = teacherProfile.PKTeachersId }, teacherProfile);
        }

        // DELETE: api/TeacherProfilesAPI/5
        [ResponseType(typeof(TeacherProfile))]
        public IHttpActionResult DeleteTeacherProfile(long id)
        {
            TeacherProfile teacherProfile = db.TeacherProfiles.Find(id);
            if (teacherProfile == null)
            {
                return NotFound();
            }

            db.TeacherProfiles.Remove(teacherProfile);
            db.SaveChanges();

            return Ok(teacherProfile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeacherProfileExists(long id)
        {
            return db.TeacherProfiles.Count(e => e.PKTeachersId == id) > 0;
        }
    }
}