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
    /// Логика взаимодействия для PersonPageMenu.xaml
    /// </summary>
    public partial class PersonPageMenu : Page, INotifyPropertyChanged
    {
        private string search;
        private string sorting;
        private MainWindow _window;
        private DataBaseEndPoint _db = DataBaseEndPoint.Instance;
        private Person selectedItem;
        private ObservableCollection<Person> list;
        private ObservableCollection<Status> firstSort;
        private ObservableCollection<Post> secondSort;

        public event PropertyChangedEventHandler? PropertyChanged;
        public Person SelectedItem { get => selectedItem; set { selectedItem = value; Signal(); } }
        public ObservableCollection<Person> SortedList { get => list; set { list = value; Signal(); } }
        public ObservableCollection<Status> FirstSort { get => firstSort; set { firstSort = value; Signal(); } }
        public ObservableCollection<Post> SecondSort { get => secondSort; set { secondSort = value; Signal(); } }
        public string Search { get => search; set { search = value; Signal(); RenderList(Sorting, Search); } }
        public string Sorting { get => sorting; set { sorting = value; Signal(); RenderList(Sorting, Search); } }

        public PersonPageMenu(MainWindow window)
            => BaseStart(window);

        private async Task BaseStart(MainWindow window)
        {
            AdminCheckMethod();
            InitializeComponent();
            _window = window;
            RenderList(null, null);
            DataContext = this;
            Thread.Sleep(300);
            FirstSort = [.. await _db.GetAllStatuses()];
            Thread.Sleep(300);
            SecondSort = [.. await _db.GetAllPosts()];
            Signal();
        }

        private async void AdminCheckMethod()
        {
            if (await _db.CheckAdminTeammate())
            {
                PpeTypeBtn.Visibility = Visibility.Collapsed;
                PersonStatusBtn.Visibility = Visibility.Collapsed;
                PostsBtn.Visibility = Visibility.Collapsed;
            }
            if (await _db.CheckAdmin())
            {
                AdminCheck.Visibility = Visibility.Collapsed;
                DelFor.Visibility = Visibility.Collapsed;
            }
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
            Sorting = item.Content.ToString();
        }

        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private async Task RenderList(string? sorting = null, string? searching = null)
        {
            var list = await _db.GetAllPersons();

            if (!string.IsNullOrEmpty(searching))
                list = [..list.Where(p =>
                p.Name.Contains(searching) ||
                p.Surname.Contains(searching) ||
                p.Patronymic.Contains(searching) ||
                p.Post.Title.Contains(searching) ||
                p.Status.Title.Contains(searching)
                )];

            string cond = "";
            var type = "";
            if (ComboFilter_Condition is not null && ComboFilter_Condition.SelectedValue is not null)
            {
                var b = "";
                try
                {
                    var a = (ComboBoxItem)ComboFilter_Condition.SelectedValue;
                    b = a.Content.ToString().ToLower();
                }
                catch
                {
                    var a = (Status)ComboFilter_Condition.SelectedValue;
                    b = a.Title.ToLower();
                }
                cond = b;
            }

            if (ComboFilter_Type is not null && ComboFilter_Type.SelectedValue is not null)
            {
                var b = "";
                try
                {
                    var a = (ComboBoxItem)ComboFilter_Type.SelectedValue;
                    b = a.Content.ToString().ToLower();
                }
                catch
                {
                    var a = (Post)ComboFilter_Type.SelectedValue;
                    b = a.Title.ToLower();
                }
                type = b;
            }

            if (cond.ToLower() != "не выбрано")
                list = [.. list.Where(p=>
                p.Status.Title.ToLower() == cond.ToLower()
                )];

            if (type.ToLower() != "не выбрано")
                list = [.. list.Where(p=>
                p.Post.Title.ToLower() == type.ToLower()
                )];

            if (!string.IsNullOrEmpty(sorting))
                list = sorting switch
                {
                    "По имени" => [.. list.OrderBy(i => i.Name)],
                    "По фамилии" => [.. list.OrderBy(i => i.Surname)],
                    "По отчеству" => [.. list.OrderBy(i => i.Patronymic)],
                    "По должности" => [.. list.OrderBy(i => i.PostId)],
                    "По статусу" => [.. list.OrderBy(i => i.StatusId)],
                    _ => [.. list.OrderBy(i => i.Id)],
                };

            SortedList = [.. list];
        }

        private async void Filter_Changed(object sender, SelectionChangedEventArgs e)
            => await RenderList(Sorting, Search);

        private void Add_Click(object sender, RoutedEventArgs e)
            => _window.SetPage(new PersonFormPage(_window, true));
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem is not null)
                _window.SetPage(new PersonFormPage(_window, true, SelectedItem));
            else MessageBox.Show("Пожалуйста выберите сотрудника!", "Внимание!");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem is null)
                MessageBox.Show("Пожалуйста выберите сотрудника!", "Внимание!");
            else
            {
                if (MessageBox.Show("Вы действительно хотите удалить сотрудника?", "Удаление!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    _db.DeletePerson(SelectedItem);
                Thread.Sleep(500);
                RenderList(Sorting, Search);
            }
        }

        private void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem is not null)
                _window.SetPage(new PersonFormPage(_window, false, SelectedItem));
            else MessageBox.Show("Пожалуйста выберите сотрудника!", "Внимание!");
        }

        private void DeleteForever_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem is null)
                MessageBox.Show("Пожалуйста выберите сотрудника!", "Внимание!");
            else
            {
                if (MessageBox.Show("Вы действительно хотите удалить сотрудника?", "Удаление!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    _db.DeletePersonForever(SelectedItem);
                Thread.Sleep(500);
                RenderList(Sorting, Search);
            }
        }
    }
}
