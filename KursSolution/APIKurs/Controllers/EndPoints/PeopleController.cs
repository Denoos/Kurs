using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIKurs.Models;
using APIKurs.Controllers.BackStage;
using Microsoft.AspNetCore.Authorization;

namespace APIKurs.Controllers.EndPoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        DataBaseController db = DataBaseController.Instance;

        // GET: api/Peoples
        [HttpGet("GetPersons")]
        [Authorize(Roles = "1,AdminHavaetPelmeni,AccessWasInFrontOfYourEyesLOL")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
            => await db.GetPeople();

        // GET: api/Peoples/5
        [HttpGet("GetPerson")]
        [Authorize(Roles = "1,AdminHavaetPelmeni,AccessWasInFrontOfYourEyesLOL")]
        public async Task<ActionResult<Person>> GetPerson(int id)
            => await db.GetPerson(id);

        // PUT: api/Peoples/5
        [HttpPut("PutPerson")]
        [Authorize(Roles = "1,AdminHavaetPelmeni,AccessWasInFrontOfYourEyesLOL")]
        public async Task<IActionResult> PutPerson(int id, Person condition)
            => await db.PutPerson(id, condition);

        // POST: api/Peoples
        [HttpPost("PostPerson")]
        [Authorize(Roles = "1,AdminHavaetPelmeni,AccessWasInFrontOfYourEyesLOL")]
        public async Task<ActionResult> PostPerson(Person condition)
            => await db.PostPerson(condition);

        // DELETE: api/Peoples/5
        [HttpDelete("DeletePerson")]
        [Authorize(Roles = "1,AdminHavaetPelmeni,AccessWasInFrontOfYourEyesLOL")]
        public async Task<IActionResult> DeletePerson(int id)
            => await db.ChangeDeletePerson(id);

        // DELETE: api/Peoples/5
        [HttpDelete("DeletePersonForever")]
        [Authorize(Roles = "AccessWasInFrontOfYourEyesLOL")]
        public async Task<IActionResult> DeletePersonForever(int id)
            => await db.DeletePersonForever(id);
    }
}
