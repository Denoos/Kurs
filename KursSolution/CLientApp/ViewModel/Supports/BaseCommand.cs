using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLientApp.ViewModel.Supports
{
    public class BaseCommand(Action action)
    {
        private readonly Action action = action;

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
            => true;

        public void Execute(object? parameter)
            => action();
    }
}
