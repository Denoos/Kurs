using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace APIKurs.Controllers.BackStage
{
    public class AdminPanel : ControllerBase
    {
        public static AdminPanel Instance { get => instance ??= new(); }

        private static AdminPanel instance;

        public async Task<ActionResult> SetDefaultPassword(string someStrongString)
        {
            if (string.IsNullOrWhiteSpace(someStrongString))
                return NoContent();

            var superSecretKey = System.IO.File.ReadAllText($"{Environment.CurrentDirectory}/AdminCode.txt").Split(';', StringSplitOptions.RemoveEmptyEntries)[0] ;
            if (string.IsNullOrEmpty(superSecretKey))
            {
                System.IO.File.Create($"{Environment.CurrentDirectory}/AdminCode.txt");
                System.IO.File.WriteAllText($"{Environment.CurrentDirectory}/AdminCode.txt", "super_duper_mega_puper_secret_secret_key_that_you_cant_show_someone_else; login:DevelopersBlessing,password=DeveloperIsGoodGuy");
                superSecretKey = System.IO.File.ReadAllText($"{Environment.CurrentDirectory}/AdminCode.txt");
            }

            if (superSecretKey == someStrongString)
            {
                try { DataBaseController.Instance.DeleteUserForever(DataBaseController.Instance.GetUser(0).Id); } catch { }

                DataBaseController.Instance.Register(new Models.User() { Login = "DevelopersBlessing", Password = "DeveloperIsGoodGuy" });

                return Ok();
            }

            return BadRequest();
        }
    }
}
