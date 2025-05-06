using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using CLientApp.Models;

namespace CLientApp.View.Pages.Forms
{
    /// <summary>
    /// Логика взаимодействия для PersonFormPage.xaml
    /// </summary>
    public partial class PersonFormPage : Page, INotifyPropertyChanged
    {
        public PersonFormPage(MainWindow window, bool IsEn, Person Item = null)
        {
            InitializeComponent();
        }
    }
}
