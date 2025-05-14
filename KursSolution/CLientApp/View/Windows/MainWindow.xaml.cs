using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CLientApp.Logic;
using CLientApp.View.Pages.Forms;

namespace CLientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Page page;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Page CurrentPage { get =>  page; set { page = value; Signal(); } }
        public JsonSerializerOptions options;

        private void Signal([CallerMemberName]string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            SetPage(new LoginFormPage(this));
            options = new()
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
            };

            DataBaseEndPoint.Instance.SetOptions(options);
        }
        public void SetPage(Page _page)
            => CurrentPage = _page;
    }
}