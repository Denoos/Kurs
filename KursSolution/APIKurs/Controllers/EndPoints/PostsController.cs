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
    public class PostsController : ControllerBase
    {
        DataBaseController db = DataBaseController.Instance;

        // GET: api/Posts
        [HttpGet("GetPosts")]
        [Authorize(Roles = "0,1,AdminHavaetPelmeni")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
            => await db.GetPosts();

        // GET: api/Posts/5
        [HttpGet("GetPost")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<ActionResult<Post>> GetPost(int id)
            => await db.GetPost(id);

        // PUT: api/Posts/5
        [HttpPut("PutPost")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<IActionResult> PutPost(Post condition)
            => await db.PutPost(condition.Id, condition);

        // POST: api/Posts
        [HttpPost("PostPost")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<ActionResult<Post>> PostPost(Post condition)
            => await db.PostPost(condition);

        // DELETE: api/Posts/5
        [HttpDelete("DeletePost")]
        [Authorize(Roles = "AdminHavaetPelmeni")]
        public async Task<IActionResult> DeletePost(int id)
            => await db.DeletePost(id);
    }
}
