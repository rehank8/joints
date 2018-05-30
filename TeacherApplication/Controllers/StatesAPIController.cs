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
using Data_Access;

namespace TeacherApplication.Controllers
{
    public class StatesAPIController : ApiController
    {
        private TeachersEntities db = new TeachersEntities();

        // GET: api/StatesAPI
        public IHttpActionResult GetStates()
        {
            return Ok(DbHelper.GetStates());
        }

        // GET: api/StatesAPI/5
        [ResponseType(typeof(State))]
        public IHttpActionResult GetState(long id)
        {
            State state = DbHelper.GetState(id);
            if (state == null)
            {
                return NotFound();
            }

            return Ok(state);
        }

        // PUT: api/StatesAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutState(State state)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                DbHelper.UpdateState(state);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/StatesAPI
        [ResponseType(typeof(State))]
        public IHttpActionResult PostState(State state)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DbHelper.InsertState(state);
            return Ok();
        }

        // DELETE: api/StatesAPI/5
        [ResponseType(typeof(State))]
        public IHttpActionResult DeleteState(long id)
        {
            DbHelper.DeleteState(id);
            return Ok();
        }


        private bool StateExists(long id)
        {
            return DbHelper.GetStates().Count(e => e.PKStateId == id) > 0;
        }
    }
}