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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CLientApp.Logic;
using CLientApp.Model;
using CLientApp.Models;
using CLientApp.View.Pages.Menues;

namespace CLientApp.View.Pages.Forms
{
    /// <summary>
    /// Логика взаимодействия для PpeFormPage.xaml
    /// </summary>
    public partial class PpeFormPage : Page, INotifyPropertyChanged
    {
        private MainWindow _mainWindow;
        private DataBaseEndPoint _db = DataBaseEndPoint.Instance;
        private Ppe item;
        private bool isEnabled;
        private Person selectPerson;
        private string selectPersonText;
        private bool isAdd = true;
        private List<Model.Condition> roles;
        private List<PpeType> itemsSecond;
        private List<Person> itemsThird;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Person SelectPerson { get => selectPerson; set { selectPerson = value; Signal(); } }
        public string SelectPersonText { get => selectPersonText; set { selectPersonText = value; Signal(); } }
        public Ppe Item { get => item; set { item = value; Signal(); } }
        public List<Model.Condition> Items { get => roles; set { roles = value; Signal(); } }
        public List<PpeType> ItemsSecond { get => itemsSecond; set { itemsSecond = value; Signal(); } }
        public List<Person> ItemsThird { get => itemsThird; set { itemsThird = value; Signal(); } }
        public bool IsEnabled { get => isEnabled; set { isEnabled = value; Signal(); } }

        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public PpeFormPage(MainWindow window, bool IsEn, Ppe item = null)
        {
            InitializeComponent();
            _mainWindow = window;
            IsEnabled = IsEn;
            GetItems();
            if (item is not null)
            {
                isAdd = false;
                Item = item;
                GetSelectedValue();
                Signal();
            }
            else Item = new();
            DataContext = this;
            SelectPersonText = "";
            if (Item.Condition is not null)
                Conditionus.SelectedValue = Item.Condition;
            else Item.Condition = new Model.Condition() { Title = "Не выбрано!" };

            if (Item.Type is not null)
                Typus.SelectedValue = Item.Type;
            else Item.Type = new PpeType() { Title = "Не выбрано!" };

            Signal();
        }

        private async Task GetSelectedValue()
            => SelectPerson = await _db.GetPerson(Item.PeopleId);

        private async Task GetItems()
        {
            Items = [.. await _db.GetAllConditions()];
            ItemsSecond = [.. await _db.GetAllPpeTypes()];
            ItemsThird = [.. await _db.GetAllPersons()];
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            bool IsFail = true;
            if (Item.Condition is null || Item.Type is null)
            {
                MessageBox.Show("Выберите состояние СИЗ, а также его тип!", "Внимание!");
                return;
            }
            Item.PeopleId = ItemsThird.FirstOrDefault(s => s.AllToString == SelectPerson.AllToString).Id;

            Item.ConditionId = Item.Condition.Id;
            Item.TypeId = Item.Type.Id;

            if (Item.DateGet >= Item.DateEnd)
            {
                MessageBox.Show("Дата окончания должна быть позже даты получения!", "Ошибка!");
                return;
            }

            if (!IsEnabled)
                IsFail = false;
            else
            {
                if (isAdd)
                    IsFail = await _db.AddPpe(Item);
                else IsFail = await _db.EditPpe(Item);
            }

            if (!IsFail)
                Exit_Click(sender, e);
            else MessageBox.Show("Внимание! ПРоизошла непредвиденная ошибка!", "Ошибка!");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
            => _mainWindow.SetPage(new PpePageMenu(_mainWindow));

        private void GetDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var snd = (DatePicker)sender;
            Item.DateGet = DateOnly.FromDateTime((DateTime)snd.SelectedDate);
            getDate.Text = Item.DateGet.ToString();
        }

        private void EndDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var snd = (DatePicker)sender;
            Item.DateEnd = DateOnly.FromDateTime((DateTime)snd.SelectedDate);
            endDate.Text = Item.DateEnd.ToString();
        }
    }
}
