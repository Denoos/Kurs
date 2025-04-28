using APIKurs.Controllers.BackStage;
using APIKurs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIKurs.Controllers.EndPoint
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        DataBaseController db = DataBaseController.Instance;

        [HttpGet]
        public async Task<ActionResult<TokEnRole>> Authorise(string login, string password)
            => await db.Authorise(login, password);
        
        [HttpPost]
        public async Task<ActionResult<TokEnRole>> Register(string login, string password)
            => await db.Register(login, password);
    }
}
