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
        private bool isAdd = true;
        private List<Models.Condition> roles;
        private List<PpeType> itemsSecond;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Ppe Item { get => item; set { item = value; Signal(); } }
        private List<Models.Condition> Items { get => roles; set { roles = value; Signal(); } }
        private List<PpeType> ItemsSecond { get => itemsSecond; set { itemsSecond = value; Signal(); } }
        public bool IsEnabled { get => isEnabled; set { isEnabled = value; Signal(); } }

        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public PpeFormPage(MainWindow window, bool IsEn, Ppe item = null)
        {
            InitializeComponent();
            _mainWindow = window;
            IsEnabled = IsEn;
            Items = _db.GetAllConditions().ToList();
            ItemsSecond = _db.GetAllPpeTypes().ToList();
            if (item is not null)
            {
                isAdd = false;
                Item = item;
            }
            else Item = new();
            DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bool IsFail = true;
            Item.IdRole = Item.IdRoleNavigation.Id;

            if (!IsEnabled)
                IsFail = false;
            else
            {
                if (isAdd)
                    IsFail = _db.AddUser(Item);
                else IsFail = _db.EditUser(Item);
            }

            if (!IsFail)
                Exit_Click(sender, e);
            else MessageBox.Show("Внимание! ПРоизошла непредвиденная ошибка!", "Ошибка!");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
            => _mainWindow.SetPage(new UserPageMenu(_mainWindow));
    }
}
