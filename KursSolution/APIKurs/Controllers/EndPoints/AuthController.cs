using APIKurs.Controllers.BackStage;
using APIKurs.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIKurs.Controllers.EndPoint
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController() { }

        DataBaseController db = DataBaseController.Instance;

        [HttpGet("Authorise")]
        public async Task<ActionResult<TokEnRole>> Authorise(string login, string password)
            => await db.Authorise(login, password);
        
        [HttpPost("Register")]
        public async Task<ActionResult<TokEnRole>> Register(User user)
            => await db.Register(user);
    }
}
