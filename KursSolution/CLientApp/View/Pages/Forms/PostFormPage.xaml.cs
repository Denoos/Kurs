using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для PostFormPage.xaml
    /// </summary>
    public partial class PostFormPage : Page
    {
        public PostFormPage(MainWindow window, bool IsEn, Post Item = null)
        {
            InitializeComponent();
        }
    }
}
