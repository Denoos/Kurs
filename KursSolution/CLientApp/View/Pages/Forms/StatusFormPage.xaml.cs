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
using CLientApp.Models;
using CLientApp.View.Pages.Menues;

namespace CLientApp.View.Pages.Forms
{
    /// <summary>
    /// Логика взаимодействия для StatusFormPage.xaml
    /// </summary>
    public partial class StatusFormPage : Page, INotifyPropertyChanged
    {
        private MainWindow _mainWindow;
        private DataBaseEndPoint _db = DataBaseEndPoint.Instance;
        private Status item;
        private bool isAdd = true;

        private CustomSettings _settings; public CustomSettings Settings { get => _settings; set { _settings = value; Signal(); } }
        public event PropertyChangedEventHandler? PropertyChanged;

        public Status Item { get => item; set { item = value; Signal(); } }
        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public StatusFormPage(MainWindow window, bool IsEn, Status item = null)
        {
            Settings = SettingsLogic.Instance.GetCurrentSettings(); InitializeComponent();
            _mainWindow = window;
            UsernameTextBox.IsEnabled = IsEn;
            if (item is not null)
            {
                isAdd = false;
                Item = item;
            }
            else Item = new();
            DataContext = this;
            Signal();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            bool IsFail = true;

            if (!IsEnabled)
                IsFail = false;
            else
            {
                if (isAdd)
                    IsFail = await _db.AddStatus(Item);
                else IsFail = await _db.EditStatus(Item);
            }

            if (!IsFail)
                Exit_Click(sender, e);
            else MessageBox.Show("Внимание! ПРоизошла непредвиденная ошибка!", "Ошибка!");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
            => _mainWindow.SetPage(new StatusPageMenu(_mainWindow));
    }
}
