using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLientApp.Models;

namespace CLientApp.ViewModel.Controllers
{
    class LoginController
    {
        private static LoginController inst;
        public static LoginController Instance { get => inst ??= new(); }

        public void LoginMethod(User user)
        {
            throw new NotImplementedException();
        }

        internal void RegisterMethod(User user)
        {
            throw new NotImplementedException();
        }
    }
}
