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
    /// Логика взаимодействия для PpeTypePageMenu.xaml
    /// </summary>
    public partial class PpeTypePageMenu : Page, INotifyPropertyChanged
    {
        private string search;
        private MainWindow _window;
        private DataBaseEndPoint _db = DataBaseEndPoint.Instance;
        private PpeType selectedItem;
        private ObservableCollection<PpeType> list;

        public event PropertyChangedEventHandler? PropertyChanged;
        public PpeType SelectedItem { get => selectedItem; set { selectedItem = value; Signal(); } }
        public ObservableCollection<PpeType> SortedList { get => list; set { list = value; Signal(); } }
        public string Search { get => search; set { search = value; Signal(); RenderList(Search); } }

        public PpeTypePageMenu(MainWindow window)
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

        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void RenderList(string? sorting = null, string? searching = null)
        {
            var list = _db.GetAllPpeTypes();

            list = [..list.Where(p =>
                p.Title.Contains(searching)
                )];

            SortedList = list;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
            => _window.SetPage(new PpeTypeFormPage(_window, true));

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem is not null)
                _window.SetPage(new PpeTypeFormPage(_window, true, SelectedItem));
            else MessageBox.Show("Пожалуйста выберите тип СИЗ!", "Внимание!");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem is null)
                MessageBox.Show("Пожалуйста выберите тип СИЗ!", "Внимание!");
            else
            {
                if (MessageBox.Show("Вы действительно хотите удалить тип СИЗ?", "Удаление!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    _db.DeletePpeType(SelectedItem);
                RenderList(Search);
            }
        }

        private void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem is not null)
                _window.SetPage(new PpeTypeFormPage(_window, false, SelectedItem));
            else MessageBox.Show("Пожалуйста выберите тип СИЗ!", "Внимание!");
        }
    }
}
