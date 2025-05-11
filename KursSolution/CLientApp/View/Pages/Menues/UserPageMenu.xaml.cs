using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CLientApp.View.Pages.Forms;

namespace CLientApp.View.Pages.Menues
{
    /// <summary>
    /// Логика взаимодействия для UserPageMenu.xaml
    /// </summary>
    public partial class UserPageMenu : Page, INotifyPropertyChanged
    {
        private string search;
        private string sorting;
        private MainWindow _window;
        private DataBaseEndPoint _db = DataBaseEndPoint.Instance;
        private User selectedItem;
        private ObservableCollection<User> list;
        private ObservableCollection<Role> firstSort;

        public event PropertyChangedEventHandler? PropertyChanged;
        public User SelectedItem { get => selectedItem; set { selectedItem = value; Signal(); } }
        public ObservableCollection<User> SortedList { get => list; set { list = value; Signal(); } }
        public ObservableCollection<Role> FirstSort { get => firstSort; set { firstSort = value; Signal(); } }
        public string Search { get => search; set { search = value; Signal(); RenderList(Sorting, Search); } }
        public string Sorting { get => sorting; set { sorting = value; Signal(); RenderList(Sorting, Search); } }

        public UserPageMenu(MainWindow window)
        {
            InitializeComponent();
            _window = window;
            RenderList();
            DataContext = this;
        }

        private void NavigationButtonClicked(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            switch (button.Content)
            {
                case "Типы СИЗ":
                    _window.SetPage(new PpeTypePageMenu(_window));
                    break;

                case "Состояния СИЗ":
                    _window.SetPage(new ConditionPageMenu(_window));
                    break;

                case "Сотрудники":
                    _window.SetPage(new PersonPageMenu(_window));
                    break;

                case "Должности":
                    _window.SetPage(new PostPageMenu(_window));
                    break;

                case "Статусы сотрудников":
                    _window.SetPage(new StatusPageMenu(_window));
                    break;

                case "Пользователи":
                    _window.SetPage(new UserPageMenu(_window));
                    break;

                case "Выход":
                    _window.SetPage(new LoginFormPage(_window));
                    break;

                default:
                    _window.SetPage(new PpePageMenu(_window));
                    break;
            }
        }

        private void SortingChanged(object sender, SelectionChangedEventArgs e)
        {
            var snd = (ComboBox)sender;
            var item = (ComboBoxItem)snd.SelectedItem;
            Sorting = item.ContentStringFormat;
        }

        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private async Task RenderList(string? sorting = null, string? searching = null)
        {
            var list = _db.GetAllUsers();

            list = [..list.Where(p =>
            p.Login.Contains(searching) ||
            p.Password.Contains(searching) ||
            p.IdRoleNavigation.Ttle.Contains(searching)
            )];

            var cond = (ComboBoxItem)ComboFilter_Condition.SelectedValue;
            list = [.. list.Where(p=>
            p.IdRoleNavigation.Ttle == cond.Content
            )];

            list = sorting switch
            {
                "По логину" => [.. list.OrderBy(i => i.Login)],
                "По паролю" => [.. list.OrderBy(i => i.Password)],
                "По роли" => [.. list.OrderBy(i => i.IdRole)],
                _ => [.. list.OrderBy(i => i.Id)],
            };

            SortedList = [.. list];
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
            => RenderList(Sorting, Search);

        private void Add_Click(object sender, RoutedEventArgs e)
            => _window.SetPage(new PersonFormPage(_window, true));
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem is not null)
                _window.SetPage(new UserFormPage(_window, true, SelectedItem));
            else MessageBox.Show("Пожалуйста выберите пользователя!", "Внимание!");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem is null)
                MessageBox.Show("Пожалуйста выберите пользователя!", "Внимание!");
            else
            {
                if (MessageBox.Show("Вы действительно хотите удалить пользователя?", "Удаление!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    _db.DeleteUser(SelectedItem);
                RenderList(Sorting, Search);
            }
        }

        private void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem is not null)
                _window.SetPage(new UserFormPage(_window, false, SelectedItem));
            else MessageBox.Show("Пожалуйста выберите пользователя!", "Внимание!");
        }
    }
}
