using System;
using System.Collections.Generic;
using System.Linq;
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

namespace CLientApp.View.Pages.Forms
{
    /// <summary>
    /// Логика взаимодействия для LoginFormPage.xaml
    /// </summary>
    public partial class LoginFormPage : Page
    {
        private MainWindow _mainWindow;
        public LoginFormPage(MainWindow window)
        {
            InitializeComponent();
            _mainWindow = window;
            DataContext = this;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
