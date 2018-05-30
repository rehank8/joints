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
    public class CitiesAPIController : ApiController
    {
        private TeachersEntities db = new TeachersEntities();

        // GET: api/CitiesAPI
        public IQueryable<City> GetCities()
        {
            return db.Cities;
        }

        // GET: api/CitiesAPI/5
        [ResponseType(typeof(City))]
        public IHttpActionResult GetCity(long id)
        {
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        // PUT: api/CitiesAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCity(long id, City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != city.PKCityId)
            {
                return BadRequest();
            }

            db.Entry(city).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
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

        // POST: api/CitiesAPI
        [ResponseType(typeof(City))]
        public IHttpActionResult PostCity(City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cities.Add(city);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = city.PKCityId }, city);
        }

        // DELETE: api/CitiesAPI/5
        [ResponseType(typeof(City))]
        public IHttpActionResult DeleteCity(long id)
        {
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return NotFound();
            }

            db.Cities.Remove(city);
            db.SaveChanges();

            return Ok(city);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CityExists(long id)
        {
            return db.Cities.Count(e => e.PKCityId == id) > 0;
        }
    }
}