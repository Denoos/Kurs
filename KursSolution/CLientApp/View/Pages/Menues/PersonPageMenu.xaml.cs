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
        //private ObservableCollection<PpeType> types;
        //private ObservableCollection<Models.Condition> conditions;

        public event PropertyChangedEventHandler? PropertyChanged;
        public Person SelectedItem { get => selectedItem; set { selectedItem = value; Signal(); } }
        public ObservableCollection<Person> SortedList { get => list; set { list = value; Signal(); } }
        //public ObservableCollection<Models.Condition> Conditions { get => conditions; set { conditions = value; Signal(); } }
        //public ObservableCollection<PpeType> Types { get => types; set { types = value; Signal(); } }
        public string Search { get => search; set { search = value; Signal(); RenderList(Sorting, Search); } }
        public string Sorting { get => sorting; set { sorting = value; Signal(); RenderList(Sorting, Search); } }

        public PersonPageMenu(MainWindow window)
        {
            InitializeComponent();
            _window = window;
            //Conditions = _db.GetAllConditions();
            //Types = _db.GetAllTypes();
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

        private void RenderList(string? sorting = null, string? searching = null)
        {
            var list = _db.GetAllPersons();

            list = [..list.Where(p =>
            p.Name.Contains(search) ||
            p.Surname.Contains(search) ||
            p.Patronymic.Contains(search) ||
            p.Post.Title.Contains(search) ||
            p.Status.Title.Contains(search)
            )];

            var cond = (ComboBoxItem)ComboFilter_Condition.SelectedValue;
            var type = (ComboBoxItem)ComboFilter_Type.SelectedValue;
            //list = [.. list.Where(p=>
            //p.Condition.Title == cond.Content ||
            //p.Type.Title == type.Content
            //)];

            list = sorting switch
            {
                "По имени" => [.. list.OrderBy(i => i.Name)],
                "По дате получения" => [.. list.OrderBy(i => i.DateGet)],
                "По дате окончания" => [.. list.OrderBy(i => i.DateEnd)],
                "По типу" => [.. list.OrderBy(i => i.TypeId)],
                "По состоянию" => [.. list.OrderBy(i => i.ConditionId)],
                _ => [.. list.OrderBy(i => i.InventoryNumber)],
            };

            SortedList = list;
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
            => RenderList(Sorting, Search);

        private void Add_Click(object sender, RoutedEventArgs e)
            => _window.SetPage(new PpeFormPage(_window, true));
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPpe is not null)
                _window.SetPage(new PpeFormPage(_window, true, SelectedPpe));
            else MessageBox.Show("Пожалуйста выберите СИЗ!", "Внимание!");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPpe is null)
                MessageBox.Show("Пожалуйста выберите СИЗ!", "Внимание!");
            else
            {
                if (MessageBox.Show("Вы действительно хотите удалить СИЗ?", "Удаление!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    _db.DeletePpe(SelectedPpe);
                RenderList(Sorting, Search);
            }
        }

        private void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPpe is not null)
                _window.SetPage(new PpeFormPage(_window, false, SelectedPpe));
            else MessageBox.Show("Пожалуйста выберите СИЗ!", "Внимание!");
        }
    }
}
