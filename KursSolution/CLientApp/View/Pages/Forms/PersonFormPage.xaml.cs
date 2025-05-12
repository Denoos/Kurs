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
    /// Логика взаимодействия для PersonFormPage.xaml
    /// </summary>
    public partial class PersonFormPage : Page, INotifyPropertyChanged
    {
        private MainWindow _mainWindow;
        private DataBaseEndPoint _db = DataBaseEndPoint.Instance;
        private Person item;
        private bool isEnabled;
        private bool isAdd = true;
        private List<Post> roles;
        private List<Status> itemsSecond;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Person Item { get => item; set { item = value; Signal(); } }
        public List<Post> Items { get => roles; set { roles = value; Signal(); } }
        public List<Status> ItemsSecond { get => itemsSecond; set { itemsSecond = value; Signal(); } }
        public bool IsEnabled { get => isEnabled; set { isEnabled = value; Signal(); } }

        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public PersonFormPage(MainWindow window, bool IsEn, Person item = null)
        {
            InitializeComponent();
            _mainWindow = window;
            IsEnabled = IsEn;
            Items = [.. _db.GetAllPosts()];
            ItemsSecond = [.. _db.GetAllStatuses()];
            if (item is not null)
            {
                isAdd = false;
                Item = item;
            }
            else Item = new();
            DataContext = this;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            bool IsFail = true;

            if (Item.Post is null || Item.Status is null)
            {
                MessageBox.Show("Выберите текущий статус сотрудника, а также его должность!", "Внимание!");
                return;
            }
            Item.PostId = Item.Post.Id;
            Item.StatusId = Item.Status.Id;

            if (!IsEnabled)
                IsFail = false;
            else
            {
                if (isAdd)
                    IsFail = await _db.AddPerson(Item);
                else IsFail = await _db.EditPerson(Item);
            }

            if (!IsFail)
                Exit_Click(sender, e);
            else MessageBox.Show("Внимание! Произошла непредвиденная ошибка!", "Ошибка!");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
            => _mainWindow.SetPage(new UserPageMenu(_mainWindow));
    }
}
