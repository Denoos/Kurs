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
    public class ConditionsController : ControllerBase
    {
        DataBaseController db = DataBaseController.Instance;

        // GET: api/Conditions
        [HttpGet("GetConditions")]
        [Authorize(Roles = "0,1,AdminHavaetPelmeni")]
        public async Task<ActionResult<IEnumerable<Condition>>> GetConditions()
            => await db.GetConditions();

        // GET: api/Conditions/5
        [HttpGet("GetCondition")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<ActionResult<Condition>> GetCondition(int id)
            => await db.GetCondition(id);

        // PUT: api/Conditions/5
        [HttpPut("PutCondition")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<IActionResult> PutCondition(Condition condition)
            => await db.PutCondition(condition.Id, condition);

        // POST: api/Conditions
        [HttpPost("PostCondition")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<ActionResult<Condition>> PostCondition(Condition condition)
            => await db.PostCondition(condition);

        // DELETE: api/Conditions/5
        [HttpDelete("DeleteCondition")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<IActionResult> DeleteCondition(int id)
            => await db.ChangeDeleteCondition(id);

        // DELETE: api/Conditions/5
        [HttpDelete("DeleteConditionForever")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<IActionResult> DeleteConditionForever(int id)
            => await db.DeleteConditionForever(id);
    }
}
