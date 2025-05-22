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

namespace CLientApp.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для DefaultPasswordPage.xaml
    /// </summary>
    public partial class DefaultPasswordPage : Window, INotifyPropertyChanged
    {
        private string key;
        public string Key { get => key; set { key = value; Signal(); } }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public DefaultPasswordPage()
        {
            InitializeComponent();
            DataContext = this;
            Key = "";
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Key) || string.IsNullOrEmpty(Key))
            {
                MessageBox.Show("Заполните название роли!!!", "Ошибка!");
                return;
            }
            if (await DataBaseEndPoint.Instance.PostDefaultPassword(Key))
                MessageBox.Show("Данные успешно изменены, используйте данные, выданные при внедрении системы на предприятие!!", "Успех!");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
            => Close();
    }
}
