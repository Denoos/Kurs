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
    /// Логика взаимодействия для ConditionFormPage.xaml
    /// </summary>
    public partial class ConditionFormPage : Page, INotifyPropertyChanged
    {
        private MainWindow _mainWindow;
        private DataBaseEndPoint _db = DataBaseEndPoint.Instance;
        private Model.Condition item;
        private bool isAdd = true;

        private CustomSettings _settings; public CustomSettings Settings { get => _settings; set { _settings = value; Signal(); } }
        public event PropertyChangedEventHandler? PropertyChanged;

        public Model.Condition Item { get => item; set { item = value; Signal(); } }

        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public ConditionFormPage(MainWindow window, bool IsEn, Model.Condition item = null)
        {
            Settings = SettingsLogic.Instance.GetCurrentSettings(); _mainWindow = window;
            InitializeComponent();
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
                    IsFail = await _db.AddCondition(Item);
                else IsFail = await _db.EditCondition(Item);
            }

            if (!IsFail)
                Exit_Click(sender, e);
            else MessageBox.Show("Внимание! Произошла непредвиденная ошибка!", "Ошибка!");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
            => _mainWindow.SetPage(new ConditionPageMenu(_mainWindow));
    }
}
