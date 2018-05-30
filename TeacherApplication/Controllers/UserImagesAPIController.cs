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
    public class UserImagesAPIController : ApiController
    {
        private TeachersEntities db = new TeachersEntities();

        // GET: api/UserImagesAPI
        public IQueryable<UserImage> GetUserImages()
        {
            return db.UserImages;
        }

        // GET: api/UserImagesAPI/5
        [ResponseType(typeof(UserImage))]
        public IHttpActionResult GetUserImage(long id)
        {
            UserImage userImage = db.UserImages.Find(id);
            if (userImage == null)
            {
                return NotFound();
            }

            return Ok(userImage);
        }

        // PUT: api/UserImagesAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserImage(long id, UserImage userImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userImage.PKUserImageId)
            {
                return BadRequest();
            }

            db.Entry(userImage).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserImageExists(id))
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

        // POST: api/UserImagesAPI
        [ResponseType(typeof(UserImage))]
        public IHttpActionResult PostUserImage(UserImage userImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserImages.Add(userImage);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userImage.PKUserImageId }, userImage);
        }

        // DELETE: api/UserImagesAPI/5
        [ResponseType(typeof(UserImage))]
        public IHttpActionResult DeleteUserImage(long id)
        {
            UserImage userImage = db.UserImages.Find(id);
            if (userImage == null)
            {
                return NotFound();
            }

            db.UserImages.Remove(userImage);
            db.SaveChanges();

            return Ok(userImage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserImageExists(long id)
        {
            return db.UserImages.Count(e => e.PKUserImageId == id) > 0;
        }
    }
}