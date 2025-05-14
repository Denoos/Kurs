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
using CLientApp.Model;
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
        private Ppe selectedPpe;
        private ObservableCollection<Ppe> list;
        private ObservableCollection<PpeType> types;
        private ObservableCollection<Model.Condition> conditions;

        public event PropertyChangedEventHandler? PropertyChanged;
        public Ppe SelectedPpe { get => selectedPpe; set { selectedPpe = value; Signal(); } }
        public ObservableCollection<Ppe> SortedList { get => list; set { list = value; Signal(); } }
        public ObservableCollection<Model.Condition> Conditions { get => conditions; set { conditions = value; Signal(); } }
        public ObservableCollection<PpeType> Types { get => types; set { types = value; Signal(); } }
        public string Search { get => search; set { search = value; Signal(); RenderList(Sorting, Search); } }
        public string Sorting { get => sorting; set { sorting = value; Signal(); RenderList(Sorting, Search); } }

        public PpePageMenu(MainWindow window)
            => BaseStart(window);

        private async Task BaseStart(MainWindow window)
        {
            InitializeComponent();
            _window = window;
            RenderList();
            DataContext = this;
            Thread.Sleep(300);
            Conditions = [.. await _db.GetAllConditions()];
            Thread.Sleep(300);
            Types = [.. await _db.GetAllPpeTypes()];
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

        private async void RenderList(string? sorting = null, string? searching = null)
        {
            var list = await _db.GetAllPpes();

            if (!string.IsNullOrEmpty(searching))
                list = [..list.Where(p =>
                p.Title.Contains(searching) ||
                p.InventoryNumber.Contains(searching) ||
                p.Condition.Title.Contains(searching) ||
                p.Type.Title.Contains(searching) ||
                p.DateGet.ToString().Contains(searching) ||
                p.DateEnd.ToString().Contains(searching)
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
                    var a = (Model.Condition)ComboFilter_Condition.SelectedValue;
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
                    var a = (PpeType)ComboFilter_Type.SelectedValue;
                    b = a.Title.ToLower();
                }
                type = b;
            }

            if (type.ToLower() != "не выбрано")
                list = [.. list.Where(p=>
                p.Type.Title.ToLower() == type.ToLower()
                )];
            
            if (cond.ToLower() != "не выбрано")
                list = [.. list.Where(p=>
                p.Condition.Title.ToLower() == cond.ToLower()
                )];


            if (!string.IsNullOrEmpty(sorting))
                list = sorting switch
                {
                    "По названию" => [.. list.OrderBy(i => i.Title)],
                    "По дате получения" => [.. list.OrderBy(i => i.DateGet)],
                    "По дате окончания" => [.. list.OrderBy(i => i.DateEnd)],
                    "По типу" => [.. list.OrderBy(i => i.TypeId)],
                    "По состоянию" => [.. list.OrderBy(i => i.ConditionId)],
                    _ => [.. list.OrderBy(i => i.InventoryNumber)],
                };

            SortedList = [.. list];
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
                Thread.Sleep(500); 
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
