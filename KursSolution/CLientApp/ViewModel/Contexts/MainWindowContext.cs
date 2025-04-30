using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CLientApp.View.Pages.Forms;
using CLientApp.ViewModel.Supports;

namespace CLientApp.ViewModel.Contexts
{
    class MainWindowContext : BaseVM
    {
        public MainWindowContext()
            => CurrentPage = new LoginFormPage(this);

        private Page curPage;
        public Page CurrentPage { get => curPage; set { curPage = value; Signal(); } }
    }
}
