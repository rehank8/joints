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
    public class SubjectsAPIController : ApiController
    {
        private TeachersEntities db = new TeachersEntities();

        // GET: api/SubjectsAPI
        public IQueryable<Subject> GetSubjects()
        {
            return db.Subjects;
        }

        // GET: api/SubjectsAPI/5
        [ResponseType(typeof(Subject))]
        public IHttpActionResult GetSubject(long id)
        {
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return NotFound();
            }

            return Ok(subject);
        }

        // PUT: api/SubjectsAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSubject(long id, Subject subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subject.PKSubjectId)
            {
                return BadRequest();
            }

            db.Entry(subject).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(id))
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

        // POST: api/SubjectsAPI
        [ResponseType(typeof(Subject))]
        public IHttpActionResult PostSubject(Subject subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Subjects.Add(subject);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = subject.PKSubjectId }, subject);
        }

        // DELETE: api/SubjectsAPI/5
        [ResponseType(typeof(Subject))]
        public IHttpActionResult DeleteSubject(long id)
        {
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return NotFound();
            }

            db.Subjects.Remove(subject);
            db.SaveChanges();

            return Ok(subject);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubjectExists(long id)
        {
            return db.Subjects.Count(e => e.PKSubjectId == id) > 0;
        }
    }
}