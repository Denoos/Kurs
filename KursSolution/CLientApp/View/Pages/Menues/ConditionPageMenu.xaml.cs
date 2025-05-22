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
    /// Логика взаимодействия для ConditionPageMenu.xaml
    /// </summary>
    public partial class ConditionPageMenu : Page, INotifyPropertyChanged
    {
        private string search;
        private MainWindow _window;
        private DataBaseEndPoint _db = DataBaseEndPoint.Instance;
        private Model.Condition selectedItem;
        private ObservableCollection<Model.Condition> list;

        public event PropertyChangedEventHandler? PropertyChanged;
        public Model.Condition SelectedItem { get => selectedItem; set { selectedItem = value; Signal(); } }
        public ObservableCollection<Model.Condition> SortedList { get => list; set { list = value; Signal(); } }
        public string Search { get => search; set { search = value; Signal(); RenderList(Search); } }

        public ConditionPageMenu(MainWindow window)
        {
            AdminCheckMethod();
            InitializeComponent();
            _window = window;
            Search = "";
            RenderList();
            DataContext = this;
            Signal();
        }

        private async void AdminCheckMethod()
        {
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
                
                case "В режим администратора":
                    
                    //zaglushka

                    break;

                default:
                    _window.SetPage(new PpePageMenu(_window));
                    break;
            }
        }

        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private async Task RenderList(string? searching = null)
        {
            var list = await _db.GetAllConditions();

            if (!string.IsNullOrEmpty(searching))
                list = [..list.Where(p =>
                p.Title.ToLower().Contains(searching)
                )];

            SortedList = [.. list];
        }

        private void Add_Click(object sender, RoutedEventArgs e)
            => _window.SetPage(new ConditionFormPage(_window, true));
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem is not null)
                _window.SetPage(new ConditionFormPage(_window, true, SelectedItem));
            else MessageBox.Show("Пожалуйста выберите состояние!", "Внимание!");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem is null)
                MessageBox.Show("Пожалуйста выберите состояние!", "Внимание!");
            else
            {
                if (MessageBox.Show("Вы действительно хотите удалить состояние? Все СИЗ с таким состоянием не изменят значения, нужно будет поменять их вручную!", "Удаление!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    _db.DeleteCondition(SelectedItem);
                Thread.Sleep(200);
                RenderList(Search);
            }
        }

        private void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem is not null)
                _window.SetPage(new ConditionFormPage(_window, false, SelectedItem));
            else MessageBox.Show("Пожалуйста выберите состояние!", "Внимание!");
        }

        private void DeleteForever_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem is null)
                MessageBox.Show("Пожалуйста выберите состояние!", "Внимание!");
            else
            {
                if (MessageBox.Show("Вы действительно хотите удалить состояние? Все СИЗ с таким состоянием также удалятся из БД, предупредите пользователей об этом!", "Удаление!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    _db.DeleteConditionForever(SelectedItem);
                Thread.Sleep(200);
                RenderList(Search);
            }
        }
    }
}
