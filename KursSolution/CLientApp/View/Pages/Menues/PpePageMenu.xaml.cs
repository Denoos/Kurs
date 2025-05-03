using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
using Microsoft.VisualBasic;

namespace CLientApp.View.Pages.Menues
{
    public partial class PpePageMenu : Page, INotifyPropertyChanged
    {
        private string search;
        private string sorting;
        private MainWindow _window;
        private DataBaseEndPoint _db = DataBaseEndPoint.Instance;
        private ObservableCollection<Ppe> list;

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<Ppe> SortedList { get => list; set { list = value; Signal(); } }
        public string Search { get => search; set { search = value; Signal(); RenderList(Sorting, Search); } }
        public string Sorting { get => sorting; set { sorting = value; Signal(); RenderList(Sorting, Search); } }

        public PpePageMenu(MainWindow window)
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

        private void RenderList(string? sorting = null, string? searching = null)
        {
            //zero step - get all
            var list = _db.GetAllPpes();

            //first step - filtering
            list = [..list.Where(p =>
                p.Title.Contains(search) ||
                p.InventoryNumber.Contains(search) ||
                p.Condition.Title.Contains(search) ||
                p.Type.Title.Contains(search) ||
                p.DateGet.ToString().Contains(search) ||
                p.DateEnd.ToString().Contains(search)
                )];


            var cond = (ComboBoxItem)ComboFilter_Condition.SelectedValue;
            var type = (ComboBoxItem)ComboFilter_Type.SelectedValue;
            list = [.. list.Where(p=>
                p.Condition.Title == cond.Content ||
                p.Type.Title == type.Content
                )];

            //second step - sorting
            list = sorting switch
            {
                "По названию" => [.. list.OrderBy(i => i.Title)],
                "По дате получения" => [.. list.OrderBy(i => i.DateGet)],
                "По дате окончания" => [.. list.OrderBy(i => i.DateEnd)],
                "По типу" => [.. list.OrderBy(i => i.TypeId)],
                "По состоянию" => [.. list.OrderBy(i => i.ConditionId)],
                _ => [.. list.OrderBy(i => i.InventoryNumber)],
            };

            //third step - setting value
            SortedList = list;
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
            => RenderList(Sorting, Search);
    }
}
