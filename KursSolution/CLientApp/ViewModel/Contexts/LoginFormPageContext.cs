using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLientApp.Models;
using CLientApp.ViewModel.Controllers;
using CLientApp.ViewModel.Supports;

namespace CLientApp.ViewModel.Contexts
{
    public class LoginFormPageContext : BaseVM
    {
        private User user;
        public User User { get => user; set { user = value; Signal(); } }
        public BaseCommand LoginCommand { get; set; }
        public BaseCommand RegisterCommand { get; set; }
        private MainWindow _window;

        LoginController _controller = LoginController.Instance;

        public LoginFormPageContext(MainWindow window)
        {
            _window = window;
            LoginCommand = new(() => { _controller.LoginMethod(User); });
            RegisterCommand = new(() => { _controller.RegisterMethod(User); });
        }
    }
}
