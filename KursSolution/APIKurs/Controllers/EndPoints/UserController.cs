using APIKurs.Controllers.BackStage;
using APIKurs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIKurs.Controllers.EndPoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly QwertyContext _context;

        public UserController(QwertyContext context)
        {
            _context = context;
        }
  
        DataBaseController db = DataBaseController.Instance;

        // GET: api/Users
        [HttpGet("GetUsers")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
            => await db.GetUsers();

        // GET: api/Users
        [HttpGet("GetPosts")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<ActionResult<IEnumerable<Role>>> GetPosts()
            => await db.GetRoles();

        // GET: api/Users/5
        [HttpGet("GetUser")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<ActionResult<User>> GetUser(int id)
            => await db.GetUser(id);

        // PUT: api/Users/5
        [HttpPut("PutUser")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<IActionResult> PutUser(int id, User condition)
            => await db.PutUser(id, condition);

        // POST: api/Users
        [HttpPost("PostUser")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<ActionResult<User>> PostUser(User condition)
            => await db.PostUser(condition);

        // DELETE: api/Users/5
        [HttpDelete("DeleteUser")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<IActionResult> DeleteUser(int id)
            => await db.DeleteUser(id);
    }
}
