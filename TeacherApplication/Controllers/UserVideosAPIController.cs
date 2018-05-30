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
    public class UserVideosAPIController : ApiController
    {
        private TeachersEntities db = new TeachersEntities();

        // GET: api/UserVideosAPI
        public IQueryable<UserVideo> GetUserVideos()
        {
            return db.UserVideos;
        }

        // GET: api/UserVideosAPI/5
        [ResponseType(typeof(UserVideo))]
        public IHttpActionResult GetUserVideo(long id)
        {
            UserVideo userVideo = db.UserVideos.Find(id);
            if (userVideo == null)
            {
                return NotFound();
            }

            return Ok(userVideo);
        }

        // PUT: api/UserVideosAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserVideo(long id, UserVideo userVideo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userVideo.PKUserVideoId)
            {
                return BadRequest();
            }

            db.Entry(userVideo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserVideoExists(id))
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

        // POST: api/UserVideosAPI
        [ResponseType(typeof(UserVideo))]
        public IHttpActionResult PostUserVideo(UserVideo userVideo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserVideos.Add(userVideo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userVideo.PKUserVideoId }, userVideo);
        }

        // DELETE: api/UserVideosAPI/5
        [ResponseType(typeof(UserVideo))]
        public IHttpActionResult DeleteUserVideo(long id)
        {
            UserVideo userVideo = db.UserVideos.Find(id);
            if (userVideo == null)
            {
                return NotFound();
            }

            db.UserVideos.Remove(userVideo);
            db.SaveChanges();

            return Ok(userVideo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserVideoExists(long id)
        {
            return db.UserVideos.Count(e => e.PKUserVideoId == id) > 0;
        }
    }
}