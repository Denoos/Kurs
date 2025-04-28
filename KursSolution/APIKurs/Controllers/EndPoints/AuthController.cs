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
        public ActionResult<TokEnRole> Authorise(string login, string password)
            => db.Authorise(login, password);
        
        [HttpPost]
        public ActionResult<TokEnRole> Register(string login, string password)
            => db.Register(login, password);
    }
}
