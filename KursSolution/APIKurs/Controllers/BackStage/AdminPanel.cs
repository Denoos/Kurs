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
            var superSecretKey = "";

            if (!System.IO.File.Exists($"{Environment.CurrentDirectory}/AdminCode.txt"))
                TryResetFile();
            using (StreamReader sr = new(($"{Environment.CurrentDirectory}/AdminCode.txt")))
            {
                superSecretKey = sr.ReadToEnd().Split(';', StringSplitOptions.RemoveEmptyEntries)[0];
            }

            if (string.IsNullOrEmpty(superSecretKey))
            {
                TryResetFile();
                superSecretKey = System.IO.File.ReadAllText($"{Environment.CurrentDirectory}/AdminCode.txt");
            }

            if (superSecretKey == someStrongString)
            {
                var list = await DataBaseController.Instance.GetUsers();
                var item = list.Value.FirstOrDefault(s => s.IdRole == 0);

                if (item is not null)
                {
                    item.Login = "DevelopersBlessing☺";
                    item.Password = DataBaseController.Instance.EncryptPassword("DeveloperIsGoodGuy");
                    item.IsDeleted = false;

                    DataBaseController.Instance.PutUser(item.Id, item);

                    return Ok();
                }
            }

            return BadRequest();
        }

        private static void TryResetFile()
        {

            if (!System.IO.File.Exists($"{Environment.CurrentDirectory}/AdminCode.txt"))
                System.IO.File.Create($"{Environment.CurrentDirectory}/AdminCode.txt");
            using (StreamWriter sr = new($"{Environment.CurrentDirectory}/AdminCode.txt"))
            {
                sr.WriteLine("super_duper_mega_puper_secret_secret_key_that_you_cant_show_someone_else; login:DevelopersBlessing,password=DeveloperIsGoodGuy");
            }
        }
    }
}
