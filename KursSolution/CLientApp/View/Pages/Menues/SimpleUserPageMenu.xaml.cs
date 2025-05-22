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
using CLientApp.View.Pages.Forms;

namespace CLientApp.View.Pages.Menues
{
    /// <summary>
    /// Логика взаимодействия для SimpleUserPageMenu.xaml
    /// </summary>
    public partial class SimpleUserPageMenu : Page, INotifyPropertyChanged
    {
        private CustomSettings _settings; public CustomSettings Settings { get => _settings; set { _settings = value; Signal(); } }
        private MainWindow _mainWindow;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void Signal([CallerMemberName]string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public SimpleUserPageMenu(MainWindow main)
        {
            Settings = SettingsLogic.Instance.GetCurrentSettings(); InitializeComponent();
            DataContext = this;
            this._mainWindow = main;
            MessageBox.Show("Эта часть приложения находтся в разработке!");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
            => _mainWindow.SetPage(new LoginFormPage(_mainWindow));
    }
}
