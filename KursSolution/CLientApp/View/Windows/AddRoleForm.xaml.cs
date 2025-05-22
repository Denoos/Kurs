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
using System.Windows.Shapes;
using CLientApp.Logic;
using CLientApp.Model;
using Microsoft.EntityFrameworkCore.Storage;

namespace CLientApp.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddRoleForm.xaml
    /// </summary>
    public partial class AddRoleForm : Window, INotifyPropertyChanged
    {
        private Role role;
        public Role Role { get => role; set { role = value; Signal(); } }
        private CustomSettings _settings; public CustomSettings Settings { get => _settings; set { _settings = value; Signal(); } }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public AddRoleForm()
        {
            Settings = SettingsLogic.Instance.GetCurrentSettings(); Role = new Role();
            DataContext = this;
            InitializeComponent();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Role is null || string.IsNullOrWhiteSpace(Role.Ttle) || string.IsNullOrEmpty(Role.Ttle))
            {
                MessageBox.Show("Заполните название роли!!!", "Ошибка!");
                return;
            }

            if (await DataBaseEndPoint.Instance.AddRole(Role))
            {
                MessageBox.Show("Роль успешно добавлена!", "Успех!");
                Role = new();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
            => Close();
    }
}
