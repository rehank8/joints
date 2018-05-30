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
    public class AvailableTeacherTimesAPIController : ApiController
    {
        private TeachersEntities db = new TeachersEntities();

        // GET: api/AvailableTeacherTimesAPI
        public IQueryable<AvailableTeacherTime> GetAvailableTeacherTimes()
        {
            return db.AvailableTeacherTimes;
        }

        // GET: api/AvailableTeacherTimesAPI/5
        [ResponseType(typeof(AvailableTeacherTime))]
        public IHttpActionResult GetAvailableTeacherTime(long id)
        {
            AvailableTeacherTime availableTeacherTime = db.AvailableTeacherTimes.Find(id);
            if (availableTeacherTime == null)
            {
                return NotFound();
            }

            return Ok(availableTeacherTime);
        }

        // PUT: api/AvailableTeacherTimesAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAvailableTeacherTime(long id, AvailableTeacherTime availableTeacherTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != availableTeacherTime.PKAvailableTeacherTimeId)
            {
                return BadRequest();
            }

            db.Entry(availableTeacherTime).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvailableTeacherTimeExists(id))
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

        // POST: api/AvailableTeacherTimesAPI
        [ResponseType(typeof(AvailableTeacherTime))]
        public IHttpActionResult PostAvailableTeacherTime(AvailableTeacherTime availableTeacherTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AvailableTeacherTimes.Add(availableTeacherTime);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = availableTeacherTime.PKAvailableTeacherTimeId }, availableTeacherTime);
        }

        // DELETE: api/AvailableTeacherTimesAPI/5
        [ResponseType(typeof(AvailableTeacherTime))]
        public IHttpActionResult DeleteAvailableTeacherTime(long id)
        {
            AvailableTeacherTime availableTeacherTime = db.AvailableTeacherTimes.Find(id);
            if (availableTeacherTime == null)
            {
                return NotFound();
            }

            db.AvailableTeacherTimes.Remove(availableTeacherTime);
            db.SaveChanges();

            return Ok(availableTeacherTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AvailableTeacherTimeExists(long id)
        {
            return db.AvailableTeacherTimes.Count(e => e.PKAvailableTeacherTimeId == id) > 0;
        }
    }
}