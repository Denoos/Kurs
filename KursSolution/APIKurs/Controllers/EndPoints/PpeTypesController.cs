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
        [HttpGet("GetPpeTypes")]
        [Authorize(Roles = "1,AdminHavaetPelmeni,AccessWasInFrontOfYourEyesLOL")]
        public async Task<ActionResult<IEnumerable<PpeType>>> GetPpeTypes()
            => await db.GetPpeTypes();

        // GET: api/PpeTypes/5
        [HttpGet("GetPpeType")]
        [Authorize(Roles = "1,AdminHavaetPelmeni,AccessWasInFrontOfYourEyesLOL")]
        public async Task<ActionResult<PpeType>> GetPpeType(int id)
            => await db.GetPpeType(id);

        // PUT: api/PpeTypes/5
        [HttpPut("PutPpeType")]
        [Authorize(Roles = "AdminHavaetPelmeni,AccessWasInFrontOfYourEyesLOL")]
        public async Task<IActionResult> PutPpeType(PpeType condition)
            => await db.PutPpeType(condition.Id, condition);

        // POST: api/PpeTypes
        [HttpPost("PostPpeType")]
        [Authorize(Roles = "AdminHavaetPelmeni,AccessWasInFrontOfYourEyesLOL")]
        public async Task<ActionResult<PpeType>> PostPpeType(PpeType condition)
            => await db.PostPpeType(condition);

        // DELETE: api/PpeTypes/5
        [HttpDelete("DeletePpeType")]
        [Authorize(Roles = "AdminHavaetPelmeni,AccessWasInFrontOfYourEyesLOL")]
        public async Task<IActionResult> DeletePpeType(int id)
            => await db.ChangeDeletePpeType(id);

        // DELETE: api/PpeTypes/5
        [HttpDelete("DeletePpeTypeForever")]
        [Authorize(Roles = "AccessWasInFrontOfYourEyesLOL")]
        public async Task<IActionResult> DeletePpeTypeForever(int id)
            => await db.DeletePpeTypeForever(id);
    }
}
