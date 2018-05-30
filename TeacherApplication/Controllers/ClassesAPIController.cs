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
    public class ClassesAPIController : ApiController
    {
        private TeachersEntities db = new TeachersEntities();

        // GET: api/ClassesAPI
        public IQueryable<Class> GetClasses()
        {
            return db.Classes;
        }

        // GET: api/ClassesAPI/5
        [ResponseType(typeof(Class))]
        public IHttpActionResult GetClass(long id)
        {
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return NotFound();
            }

            return Ok(@class);
        }

        // PUT: api/ClassesAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClass(long id, Class @class)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @class.PKClassId)
            {
                return BadRequest();
            }

            db.Entry(@class).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(id))
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

        // POST: api/ClassesAPI
        [ResponseType(typeof(Class))]
        public IHttpActionResult PostClass(Class @class)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Classes.Add(@class);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = @class.PKClassId }, @class);
        }

        // DELETE: api/ClassesAPI/5
        [ResponseType(typeof(Class))]
        public IHttpActionResult DeleteClass(long id)
        {
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return NotFound();
            }

            db.Classes.Remove(@class);
            db.SaveChanges();

            return Ok(@class);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClassExists(long id)
        {
            return db.Classes.Count(e => e.PKClassId == id) > 0;
        }
    }
}