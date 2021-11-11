﻿#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Extensions;
using Microsoft.Toolkit.Helpers;
using Sensors.Annotations;
using Sensors.Model;
using Sensors.Model.Data.Factory;

namespace Sensors.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private Sensor _sens = new Sensor();
        private string _mainTextBlock;
        private List<Sensor> sensors = new List<Sensor>();
        public Guid Id { get; set; }

        public string MainTextBlock
        {
            get => _mainTextBlock;
            set
            {
                _mainTextBlock = value;
                OnPropertyChanged();
            }

        }
        public Sensor Sens
        {
            get => _sens;
            set
            {
                _sens = value;
                OnPropertyChanged();
            }
        }
        public IAsyncCommand CreateCommandProperty { get; set; }
        public IAsyncCommand DeleteCommandProperty { get; set; }
        public IAsyncCommand ChangeCommandProperty { get; set; }
        public IAsyncCommand FindCommandProperty { get; set; }
        public IAsyncCommand RefreshCommandProperty { get; set; }

        public MainViewModel()
        {
            RefreshCommandProperty = new AsyncCommand<bool>(async () =>
            {
                MainTextBlock = "";
                sensors = await SensorOptions.JsonDeserialize();
                foreach (var sensor in sensors)
                {
                    MainTextBlock += "ID: " + sensor.Id.ToString() + "\n";
                    MainTextBlock += "Type: " + sensor.Type + "\n";
                    MainTextBlock += "Measured Name: " + sensor.MeasuredName + "\n";
                    MainTextBlock += "Measured Value: " + sensor.MeasuredValue + "\n";
                    MainTextBlock += "Interval: " + sensor.Interval.ToString() + "\n";
                    MainTextBlock += "Mode: " + Enum.GetName(sensor.Mode) + "\n" + "\n";
                }
                return true;
            });
            CreateCommandProperty = new AsyncCommand<bool>(async () =>
            {
                await SensorOptions.JsonSerialize(Sens);
                return true;
            });
            DeleteCommandProperty = new AsyncCommand<bool>(async () =>
            {
                await SensorOptions.JsonDelete(Id);
                return true;
            });
            ChangeCommandProperty = new AsyncCommand<bool>(async () =>
            {
                await SensorOptions.JsonChange(Id, Sens);
                return true;
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

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
