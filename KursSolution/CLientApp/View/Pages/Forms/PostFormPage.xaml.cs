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
    /// Логика взаимодействия для PostFormPage.xaml
    /// </summary>
    public partial class PostFormPage : Page, INotifyPropertyChanged
    {
        private MainWindow _mainWindow;
        private DataBaseEndPoint _db = DataBaseEndPoint.Instance;
        private Post item;
        private bool isEnabled;
        private bool isAdd = true;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Post Item { get => item; set { item = value; Signal(); } }
        public bool IsEnabled { get => isEnabled; set { isEnabled = value; Signal(); } }

        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public PostFormPage(MainWindow window, bool IsEn, Post item = null)
        {
            InitializeComponent();
            _mainWindow = window;
            IsEnabled = IsEn;
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

            if (!IsEnabled)
                IsFail = false;
            else
            {
                if (isAdd)
                    IsFail = _db.AddPost(Item);
                else IsFail = _db.EditPost(Item);
            }

            if (!IsFail)
                Exit_Click(sender, e);
            else MessageBox.Show("Внимание! ПРоизошла непредвиденная ошибка!", "Ошибка!");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
            => _mainWindow.SetPage(new PostPageMenu(_mainWindow));
    }
}
