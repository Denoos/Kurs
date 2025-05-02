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
using CLientApp.Models;
using CLientApp.View.Pages.Menues;

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

        public event PropertyChangedEventHandler? PropertyChanged;

        public User User { get => user; set { user = value; Signal(); } }

        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public LoginFormPage(MainWindow window)
        {
            InitializeComponent();
            _mainWindow = window;
            DataContext = this;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (_db.Login(User))
                _mainWindow.SetPage(new PpePageMenu(_mainWindow));
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (_db.Login(User))
                LoginButton_Click(sender, e);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
            => _mainWindow.Close();
    }
}
