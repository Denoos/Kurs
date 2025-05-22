using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CLientApp.Logic;
using CLientApp.Model;
using CLientApp.View.Pages.Menues;
using CLientApp.View.Windows;

namespace CLientApp.View.Pages.Forms
{
    /// <summary>
    /// Логика взаимодействия для LoginFormPage.xaml
    /// </summary>
    public partial class LoginFormPage : Page, INotifyPropertyChanged
    {
        private MainWindow _mainWindow;
        private DataBaseEndPoint _db = DataBaseEndPoint.Instance;
        private User user;
        private CustomSettings _settings; public CustomSettings Settings { get => _settings; set { _settings = value; Signal(); } }
        public event PropertyChangedEventHandler? PropertyChanged;

        public User User { get => user; set { user = value; Signal(); } }

        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public LoginFormPage(MainWindow window)
        {
            Settings = SettingsLogic.Instance.GetCurrentSettings(); 
            InitializeComponent();
            _mainWindow = window;
            DataContext = this;
            Signal();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            User = new() { Login = UsernameTextBox.Text, Password = PasswordBox.Password };
            if (await _db.Login(User))
                if (DataBaseEndPoint.Instance.CurrentAccount.IdRoleNavigation.Ttle == "0")
                    _mainWindow.SetPage(new SimpleUserPageMenu(_mainWindow));
                else _mainWindow.SetPage(new PpePageMenu(_mainWindow));
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            User = new() { Login = UsernameTextBox.Text, Password = PasswordBox.Password };

            //мы вникаем в код, но не осуждаем)
            if (await _db.Register(User))
                LoginButton_Click(sender, e);
            //после этого всё равно осуждать нельзя
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
            => _mainWindow.Close();

        private void DefPassword_Click(object sender, RoutedEventArgs e)
            => new DefaultPasswordPage().Show();
    }
}
