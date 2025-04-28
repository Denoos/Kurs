using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIKurs.Models;

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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PpeType>>> GetPpeTypes()
        {
            return await _context.PpeTypes.ToListAsync();
        }

        // GET: api/PpeTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PpeType>> GetPpeType(int id)
        {
            var ppeType = await _context.PpeTypes.FindAsync(id);

            if (ppeType == null)
            {
                return NotFound();
            }

            return ppeType;
        }

        // PUT: api/PpeTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPpeType(int id, PpeType ppeType)
        {
            if (id != ppeType.Id)
            {
                return BadRequest();
            }

            _context.Entry(ppeType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PpeTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PpeTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PpeType>> PostPpeType(PpeType ppeType)
        {
            _context.PpeTypes.Add(ppeType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPpeType", new { id = ppeType.Id }, ppeType);
        }

        // DELETE: api/PpeTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePpeType(int id)
        {
            var ppeType = await _context.PpeTypes.FindAsync(id);
            if (ppeType == null)
            {
                return NotFound();
            }

            _context.PpeTypes.Remove(ppeType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PpeTypeExists(int id)
        {
            return _context.PpeTypes.Any(e => e.Id == id);
        }
    }
}
