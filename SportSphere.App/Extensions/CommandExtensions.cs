using System.Windows.Input;

namespace SportSphere.App.Extensions
{
    public static class CommandExtensions
    {
        public static async Task ExecuteAsync(this ICommand command, object parameter)
        {
            if (command is Command cmd)
            {
                await Task.Run(() => cmd.Execute(parameter));
            }
            else if (command is AsyncCommand asyncCmd)
            {
                await asyncCmd.ExecuteAsync(parameter);
            }
            else
            {
                command.Execute(parameter);
            }
        }
    }

    public class AsyncCommand : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool>? _canExecute;
        private bool _isExecuting;

        public AsyncCommand(Func<object, Task> execute, Func<object, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return !_isExecuting && (_canExecute == null || _canExecute(parameter!));
        }

        public async void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    RaiseCanExecuteChanged();
                    await _execute(parameter!);
                }
                finally
                {
                    _isExecuting = false;
                    RaiseCanExecuteChanged();
                }
            }
        }

        public async Task ExecuteAsync(object? parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    RaiseCanExecuteChanged();
                    await _execute(parameter!);
                }
                finally
                {
                    _isExecuting = false;
                    RaiseCanExecuteChanged();
                }
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
} 