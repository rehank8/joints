using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Data_Access;


namespace TeacherApplication.Controllers
{
    public class LandingPageAPIController : ApiController
    {
        // GET: api/LandingPageAPI
        public IEnumerable<UserIndex> Get()
        {
            return  DbHelper.GetTeachersForHome();
        }

        // GET: api/LandingPageAPI/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LandingPageAPI
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/LandingPageAPI/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LandingPageAPI/5
        public void Delete(int id)
        {
        }
    }
}
