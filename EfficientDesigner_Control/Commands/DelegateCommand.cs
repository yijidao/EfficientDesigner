using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EfficientDesigner_Control.Commands
{
    public class DelegateCommand : ICommand
    {

        public DelegateCommand()
        {

        }

        public DelegateCommand(Action executeDelegate) : this(null, executeDelegate)
        {
        }

        public DelegateCommand(Func<bool> canExecuteDelegate = null, Action executeDelegate = null)
        {
            CanExecuteDelegate = canExecuteDelegate;
            ExecuteDelegate = executeDelegate;
        }

        public Func<bool> CanExecuteDelegate { get; }
        public Action ExecuteDelegate { get; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate == null)
                return true;
            else
                return CanExecuteDelegate();
        }

        public void Execute(object parameter)
        {
            ExecuteDelegate?.Invoke();
        }
    }
}
