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
    public class StatusController : ControllerBase
    {
        private readonly QwertyContext _context;

        public StatusController(QwertyContext context)
        {
            _context = context;
        }

        // GET: api/Status
      
        
        DataBaseController db = DataBaseController.Instance;

        // GET: api/Statuss
        [HttpGet("GetStatuses")]
        [Authorize(Roles = "0,1,AdminHavaetPelmeni")]
        public async Task<ActionResult<IEnumerable<Status>>> GetStatuses()
            => await db.GetStatuses();

        // GET: api/Statuss/5
        [HttpGet("GetStatus")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<ActionResult<Status>> GetStatus(int id)
            => await db.GetStatus(id);

        // PUT: api/Statuss/5
        [HttpPut("PutStatus")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<IActionResult> PutStatus(Status condition)
            => await db.PutStatus(condition.Id, condition);

        // POST: api/Statuss
        [HttpPost("PostStatus")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<ActionResult<Status>> PostStatus(Status condition)
            => await db.PostStatus(condition);

        // DELETE: api/Statuss/5
        [HttpDelete("DeleteStatus")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<IActionResult> DeleteStatus(int id)
            => await db.ChangeDeleteStatus(id);

        // DELETE: api/Statuss/5
        [HttpDelete("DeleteStatusForever")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<IActionResult> DeleteStatusForever(int id)
            => await db.DeleteStatusForever(id);
    }
}
