using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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

namespace CLientApp.View.Pages.Forms
{
    /// <summary>
    /// Логика взаимодействия для SettingsForm.xaml
    /// </summary>
    public partial class SettingsForm : Page, INotifyPropertyChanged
    {
        MainWindow _main;
        private CustomSettings sett;
        public CustomSettings Settings { get => sett; set { sett = value; Signal(); } }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public SettingsForm(MainWindow main)
        {
            InitializeComponent();
            DataContext = this;
            _main = main;
            Settings = SettingsLogic.Instance.GetCurrentSettings();
            Settings = new CustomSettings() { Color = Settings.Color, FontSize = Settings.FontSize, RadioIsWorking = Settings.RadioIsWorking };
            if (DataBaseEndPoint.Instance.CheckAdmin().Result)
                ForAdminStack.Visibility = Visibility.Collapsed;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
            => _main.SetPage(_main.PreviousPage);

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Settings is null ||
                string.IsNullOrEmpty(Settings.Color) ||
                string.IsNullOrWhiteSpace(Settings.Color) ||
                Settings.FontSize <= 0)
            {
                MessageBox.Show("Заполнены не все поля!", "Ошибка!");
                return;
            }

            if (Settings.Color[0] != '#' ||
                Settings.Color.Count() != 7)
            {
                MessageBox.Show("Цвет имеет некорректное значение!!!", "Ошибка!");
                return;
            }

            if (!ValidationCheck(Settings.Color))
            {
                MessageBox.Show("Внимание! Цвету присвоено некорректное значение!\n Введите значние в формате #000000", "Ошибка!");
                return;
            }

            if (MessageBox.Show("Вы действительно уверены, что хотите сохранить эти параметры??", "Внимание!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                SettingsLogic.Instance.SaveNewSettings(Settings);
        }


        private bool ValidationCheck(string color)
        {
            var result = new Regex("#[0-9A-Fa-f]{6}").IsMatch(color);
            return result;
        }

        private void DefaultButton_Click(object sender, RoutedEventArgs e)
        { 
            SettingsLogic.Instance.UseDeafaultSettings();
            Settings = SettingsLogic.Instance.GetCurrentSettings();
            MessageBox.Show("Выставлены значения по умолчанию!", "Уведомление");
        }
    }
}
