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
    public class PpeTypesController : ControllerBase
    {
        private readonly QwertyContext _context;

        public PpeTypesController(QwertyContext context)
        {
            _context = context;
        }

        // GET: api/PpeTypes
      
       
        DataBaseController db = DataBaseController.Instance;

        // GET: api/PpeTypes
        [HttpGet]
        [Authorize(Roles = "0,1,AdminHavaetPelmeni")]
        public async Task<ActionResult<IEnumerable<PpeType>>> GetPpeTypes()
            => await db.GetPpeTypes();

        // GET: api/PpeTypes/5
        [HttpGet("{id}")]
        [Authorize(Roles = "0,1,AdminHavaetPelmeni")]
        public async Task<ActionResult<PpeType>> GetPpeType(int id)
            => await db.GetPpeType(id);

        // PUT: api/PpeTypes/5
        [HttpPut("{id}")]
        [Authorize(Roles = "1,AdminHavaetPelmeni")]
        public async Task<IActionResult> PutPpeType(int id, PpeType condition)
            => await db.PutPpeType(id, condition);

        // POST: api/PpeTypes
        [HttpPost]
        [Authorize(Roles = "1,AdminHavaetPelmeni")]
        public async Task<ActionResult<PpeType>> PostPpeType(PpeType condition)
            => await db.PostPpeType(condition);

        // DELETE: api/PpeTypes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "1,AdminHavaetPelmeni")]
        public async Task<IActionResult> DeletePpeType(int id)
            => await db.DeletePpeType(id);
    }
}
