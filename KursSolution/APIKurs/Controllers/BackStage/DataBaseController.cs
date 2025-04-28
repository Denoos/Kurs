using APIKurs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APIKurs.Controllers.BackStage
{
    public class DataBaseController : ControllerBase
    {
        public static DataBaseController Instance { get => instance ??= new(); }

        private static DataBaseController instance;
        private QwertyContext _context;
        DataBaseController()
            => _context = new();

        //System Metods
        private async Task Save()
            => await _context.SaveChangesAsync();


        //Security Methods
        private string EncryptPassword(string someString)
        {
            return "";
        }
        private string EncryptToken(string someString)
        {
            return "";
        }
        private string DecryptToken(string someString)
        {
            return "";
        }

        private string PeperSecurity(string someString)
        {
            var nums = System.IO.File.ReadAllText("Models/NumsFile.txt");
            return "";
        }

        //Model Methods

        //Enter
        public ActionResult<TokEnRole> Authorise(string someString, string otherString)
        {
            Save();
            return new(new TokEnRole());
        }

        public ActionResult<TokEnRole> Register(string someString, string otherString)
        {
            Save();
            return Authorise(someString, otherString);
        }

        //Add

        public async Task<ActionResult<Condition>> AddCondition(Condition obj)
        {
            _context.Conditions.Add(obj);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCondition", new { id = obj.Id }, obj);
        }

        //Put

        //Delete

        //Get One

        //Get Many

    }
}
