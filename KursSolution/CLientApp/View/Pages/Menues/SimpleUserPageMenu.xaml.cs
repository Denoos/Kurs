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
using CLientApp.View.Pages.Forms;

namespace CLientApp.View.Pages.Menues
{
    /// <summary>
    /// Логика взаимодействия для SimpleUserPageMenu.xaml
    /// </summary>
    public partial class SimpleUserPageMenu : Page
    {
        private MainWindow _mainWindow;
        public SimpleUserPageMenu(MainWindow main)
        {
            InitializeComponent();
            DataContext = this;
            this._mainWindow = main;
            MessageBox.Show("Эта часть приложения находтся в разработке!");
            _mainWindow.SetPage(new LoginFormPage(_mainWindow));
        }
    }
}
