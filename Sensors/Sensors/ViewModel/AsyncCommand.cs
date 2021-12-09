using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Toolkit.Helpers;

namespace Sensors.ViewModel
{
    class AsyncCommand<TResult> : IAsyncCommand, INotifyPropertyChanged
    {
        private readonly Func<Task<TResult>> _command;
        private NotifyTaskCompletion<TResult> _execution;

        public AsyncCommand(Func<Task<TResult>>? command)
        {
            _command = command;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _command();
        }

        public Task ExecuteAsync(object parameter)
        {
            return new NotifyTaskCompletion<TResult>(_command()).TaskCompletion;
        }

        public event EventHandler? CanExecuteChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public NotifyTaskCompletion<TResult> Execution;
    }
}