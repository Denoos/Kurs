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
    public class PpesController : ControllerBase
    {
        private readonly QwertyContext _context;

        public PpesController(QwertyContext context)
        {
            _context = context;
        }

        
        DataBaseController db = DataBaseController.Instance;

        // GET: api/Ppes
        [HttpGet]
        [Authorize(Roles = "0,1,AdminHavaetPelmeni")]
        public async Task<ActionResult<IEnumerable<Ppe>>> GetPpes()
            => await db.GetPpes();

        // GET: api/Ppes/5
        [HttpGet("{id}")]
        [Authorize(Roles = "0,1,AdminHavaetPelmeni")]
        public async Task<ActionResult<Ppe>> GetPpe(int id)
            => await db.GetPpe(id);

        // PUT: api/Ppes/5
        [HttpPut("{id}")]
        [Authorize(Roles = "1,AdminHavaetPelmeni")]
        public async Task<IActionResult> PutPpe(int id, Ppe condition)
            => await db.PutPpe(id, condition);

        // POST: api/Ppes
        [HttpPost]
        [Authorize(Roles = "1,AdminHavaetPelmeni")]
        public async Task<ActionResult<Ppe>> PostPpe(Ppe condition)
            => await db.PostPpe(condition);

        // DELETE: api/Ppes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "1,AdminHavaetPelmeni")]
        public async Task<IActionResult> DeletePpe(int id)
            => await db.DeletePpe(id);
    }
}