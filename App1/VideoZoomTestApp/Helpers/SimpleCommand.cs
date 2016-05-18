using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VideoZoomTestApp.Helpers
{
    public class SimpleCommand : ICommand
    {
        private Action _task;

        public SimpleCommand(Action task)
        {
            _task = task;
            active = true;
        }

        public bool CanExecute(object parameter)
        {
            return active;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _task();
        }

        private bool active;

        public bool Active
        {
            get { return active; }
            set { active = value; CanExecuteChanged(this, new EventArgs()); }
        }
    }
}