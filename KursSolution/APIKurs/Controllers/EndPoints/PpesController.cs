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
    public class PpesController : ControllerBase
    {
        private readonly QwertyContext _context;

        public PpesController(QwertyContext context)
        {
            _context = context;
        }

        // GET: api/Ppes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ppe>>> GetPpes()
        {
            return await _context.Ppes.ToListAsync();
        }

        // GET: api/Ppes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ppe>> GetPpe(int id)
        {
            var ppe = await _context.Ppes.FindAsync(id);

            if (ppe == null)
            {
                return NotFound();
            }

            return ppe;
        }

        // PUT: api/Ppes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPpe(int id, Ppe ppe)
        {
            if (id != ppe.Id)
            {
                return BadRequest();
            }

            _context.Entry(ppe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PpeExists(id))
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

        // POST: api/Ppes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ppe>> PostPpe(Ppe ppe)
        {
            _context.Ppes.Add(ppe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPpe", new { id = ppe.Id }, ppe);
        }

        // DELETE: api/Ppes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePpe(int id)
        {
            var ppe = await _context.Ppes.FindAsync(id);
            if (ppe == null)
            {
                return NotFound();
            }

            _context.Ppes.Remove(ppe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PpeExists(int id)
        {
            return _context.Ppes.Any(e => e.Id == id);
        }
    }
}
