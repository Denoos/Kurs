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
        [Authorize(Roles = "AccessWasInFrontOfYourEyesLOL")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
            => await db.GetUsers();

        // GET: api/Users
        [HttpGet("GetPosts")]
        [Authorize(Roles = "AccessWasInFrontOfYourEyesLOL")]
        public async Task<ActionResult<IEnumerable<Role>>> GetPosts()
            => await db.GetRoles();

        // GET: api/Users/5
        [HttpGet("GetUser")]
        [Authorize(Roles = "AccessWasInFrontOfYourEyesLOL")]
        public async Task<ActionResult<User>> GetUser(int id)
            => await db.GetUser(id);

        // PUT: api/Users/5
        [HttpPut("PutUser")]
        [Authorize(Roles = "AccessWasInFrontOfYourEyesLOL")]
        public async Task<IActionResult> PutUser(int id, User condition)
            => await db.PutUser(id, condition);

        // POST: api/Users
        [HttpPost("PostUser")]
        [Authorize(Roles = "AccessWasInFrontOfYourEyesLOL")]
        public async Task<ActionResult> PostUser(User condition)
            => await db.PostUser(condition);
        
        // POST: api/Users
        [HttpPost("PostRole")]
        [Authorize(Roles = "AccessWasInFrontOfYourEyesLOL")]
        public async Task<ActionResult> PostRole(Role role)
            => await db.PostRole(role);

        // DELETE: api/Users/5
        [HttpDelete("DeleteUser")]
        [Authorize(Roles = "AccessWasInFrontOfYourEyesLOL")]
        public async Task<IActionResult> DeleteUser(int id)
            => await db.ChangeDeleteUser(id);

        // DELETE: api/Users/5
        [HttpDelete("DeleteUserForever")]
        [Authorize(Roles = "AccessWasInFrontOfYourEyesLOL")]
        public async Task<IActionResult> DeleteUserForever(int id)
            => await db.DeleteUserForever(id);
    }
}
