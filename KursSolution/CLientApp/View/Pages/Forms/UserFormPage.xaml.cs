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
    /// Логика взаимодействия для UserFormPage.xaml
    /// </summary>
    public partial class UserFormPage : Page, INotifyPropertyChanged
    {
        private MainWindow _mainWindow;
        private DataBaseEndPoint _db = DataBaseEndPoint.Instance;
        private User item;
        private bool isEnabled;
        private bool isAdd = true;
        private List<Role> roles;

        public event PropertyChangedEventHandler? PropertyChanged;

        public User Item { get => item; set { item = value; Signal(); } }
        private List<Role> Items { get => roles; set { roles = value; Signal(); } }
        public bool IsEnabled { get => isEnabled; set { isEnabled = value; Signal(); } }

        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public UserFormPage(MainWindow window, bool IsEn, User item = null)
        {
            InitializeComponent();
            _mainWindow = window;
            GetElements();
            IsEnabled = IsEn;
            if (item is not null)
            {
                isAdd = false;
                Item = item;
            }
            else Item = new();
            DataContext = this;
        }

        private async void GetElements()
            => Items = await _db.GetAllRoles();

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            bool IsFail = true;
            if (Item.IdRoleNavigation is null)
            {
                MessageBox.Show("Выберите текущую роль пользователя!", "Внимание!");
                return;
            }
            Item.IdRole = Item.IdRoleNavigation.Id;

            if (!IsEnabled)
                IsFail = false;
            else
            {
                if (isAdd)
                    IsFail = await _db.AddUser(Item);
                else IsFail = await _db.EditUser(Item);
            }

            if (!IsFail)
                Exit_Click(sender, e);
            else MessageBox.Show("Внимание! ПРоизошла непредвиденная ошибка!", "Ошибка!");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
            => _mainWindow.SetPage(new UserPageMenu(_mainWindow));
    }
}
