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
        [HttpGet]
        [Authorize(Roles = "0,1,AdminHavaetPelmeni")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
            => await db.GetPeople();

        // GET: api/Peoples/5
        [HttpGet("{id}")]
        [Authorize(Roles = "0,1,AdminHavaetPelmeni")]
        public async Task<ActionResult<Person>> GetPerson(int id)
            => await db.GetPerson(id);

        // PUT: api/Peoples/5
        [HttpPut("{id}")]
        [Authorize(Roles = "1,AdminHavaetPelmeni")]
        public async Task<IActionResult> PutPerson(int id, Person condition)
            => await db.PutPerson(id, condition);

        // POST: api/Peoples
        [HttpPost]
        [Authorize(Roles = "1,AdminHavaetPelmeni")]
        public async Task<ActionResult<Person>> PostPerson(Person condition)
            => await db.PostPerson(condition);

        // DELETE: api/Peoples/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "1,AdminHavaetPelmeni")]
        public async Task<IActionResult> DeletePerson(int id)
            => await db.DeletePerson(id);
    }
}
