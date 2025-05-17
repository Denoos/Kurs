using System;
using System.CodeDom;
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

namespace CLientApp.View.Pages.Menues
{
    /// <summary>
    /// Логика взаимодействия для NotificationPageMenu.xaml
    /// </summary>
    public partial class NotificationPageMenu : Page, INotifyPropertyChanged
    {
        private MainWindow _main;
        private List<Notify_Model> list;
        private Notify_Model selected;
        private NotifyLogic _logic;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public List<Notify_Model> NotificationsList { get => list; set { list = value; Signal(); } }
        public Notify_Model SelectedNoty { get => selected; set { selected = value; Signal(); } }
        public NotificationPageMenu(MainWindow main, List<Ppe> ppes)
        {
            _logic = new NotifyLogic(ppes);
            _main = main;
            NotificationsList = _logic.GetNotifyes();
            InitializeComponent();
            this.DataContext = this;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
            => _main.SetPage(_main.PreviousPage);

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (SelectedNoty != null)
                NotificationsList.Remove(SelectedNoty);
            else MessageBox.Show("Выберите элемент списка!", "Внимание!");
        }
    }
}
