using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APIKurs.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена

        public const string AUDIENCE = "YourClient"; // потребитель токена

        const string KEY = "sоеvij_licetin_bezvkusni_i_pоhoj_nа_myky_s_vodoi.I.Think.Its.Enоugh.Lоng.Аnd.Difficult.Pаssword.For.Security_v_3.0.1";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new (Encoding.UTF8.GetBytes(KEY));
    }
}
