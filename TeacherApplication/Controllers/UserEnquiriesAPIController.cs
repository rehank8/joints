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
    public class UserEnquiriesAPIController : ApiController
    {
        private TeachersEntities db = new TeachersEntities();

        // GET: api/UserEnquiriesAPI
        public IQueryable<UserEnquiry> GetUserEnquiries()
        {
            return db.UserEnquiries;
        }

        // GET: api/UserEnquiriesAPI/5
        [ResponseType(typeof(UserEnquiry))]
        public IHttpActionResult GetUserEnquiry(long id)
        {
            UserEnquiry userEnquiry = db.UserEnquiries.Find(id);
            if (userEnquiry == null)
            {
                return NotFound();
            }

            return Ok(userEnquiry);
        }

        // PUT: api/UserEnquiriesAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserEnquiry(long id, UserEnquiry userEnquiry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userEnquiry.PKEnquiryId)
            {
                return BadRequest();
            }

            db.Entry(userEnquiry).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserEnquiryExists(id))
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

        // POST: api/UserEnquiriesAPI
        [ResponseType(typeof(UserEnquiry))]
        public IHttpActionResult PostUserEnquiry(UserEnquiry userEnquiry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserEnquiries.Add(userEnquiry);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userEnquiry.PKEnquiryId }, userEnquiry);
        }

        // DELETE: api/UserEnquiriesAPI/5
        [ResponseType(typeof(UserEnquiry))]
        public IHttpActionResult DeleteUserEnquiry(long id)
        {
            UserEnquiry userEnquiry = db.UserEnquiries.Find(id);
            if (userEnquiry == null)
            {
                return NotFound();
            }

            db.UserEnquiries.Remove(userEnquiry);
            db.SaveChanges();

            return Ok(userEnquiry);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserEnquiryExists(long id)
        {
            return db.UserEnquiries.Count(e => e.PKEnquiryId == id) > 0;
        }
    }
}