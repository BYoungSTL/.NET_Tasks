#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Sensors.Annotations;
using Sensors.Model;
using Sensors.Model.Data.Enums;
using Sensors.Model.Data.Factory;
using Sensors.Model.Data.Observer;

namespace Sensors.ViewModel
{
    class XmlViewModel : INotifyPropertyChanged
    {
        public static event ValueCountHandler Count;
        private EnumType _sensorType;
        private string _mainTextBlock = "";
        private List<ISensor> _sensors = new List<ISensor>();
        private Guid _id;
        private ISensor _sens;
        private EnumMode _sensorMode;
        private StringBuilder _scrollViewerText;
        private static string _message;
        private readonly SensorObservable _sensorObservable;

        public EnumMode SensorMode
        {
            get => _sensorMode;
            set
            {
                _sensorMode = value;
                _sens.Mode = value;
                OnPropertyChanged();
            }
        }

        public EnumType SensorType
        {
            get => _sensorType;
            set
            {
                _sensorType = value;
                _sens.Type = value;
                OnPropertyChanged();
            }
        }


        public Guid Id
        {
            get => _id;
            set
            {
                Sens.Id = value;
                _id = value;
                OnPropertyChanged();
            }
        }

        public List<ISensor> Sensors
        {
            get => _sensors;
            set
            {
                _sensors = value;
                OnPropertyChanged();
            }
        }

        public string MainTextBlock
        {
            get => _mainTextBlock;
            set
            {
                _mainTextBlock = value;
                OnPropertyChanged();
            }
        }

        public StringBuilder ScrollViewerText
        {
            get => _scrollViewerText;
            set
            {
                _scrollViewerText = value;
                OnPropertyChanged();
            }
        }

        public ISensor Sens
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

        public IAsyncCommand StartCommandProperty { get; set; }

        public IAsyncCommand CleanLogCommandProperty { get; set; }

        public XmlViewModel()
        {
            ScrollViewerText = new StringBuilder();
            _sensorObservable = new SensorObservable();
            Count += SensorOptions.StopCount;
            TaskFactory taskFactory = new TaskFactory();
            Sens = new SensorOptions().Create(SensorType);
            SensorObserver observer = new SensorObserver();
            observer.LastProvider = new SensorMonitor
            {
                SensorCount = Sensors.Count
            };
            CleanLogCommandProperty = new AsyncCommand<bool>(async () =>
            {
                ScrollViewerText = new StringBuilder("");
                return true;
            });
            RefreshCommandProperty = new AsyncCommand<bool>(async () =>
            {
                Count.Invoke();
                if (!string.IsNullOrEmpty(_message))
                {
                    ScrollViewerText = ScrollViewerText.Append(_message);
                }
                MainTextBlock = "";
                Sensors = await SensorOptions.XmlDeserializeAsync();
                observer.LastProvider = new SensorMonitor
                {
                    SensorCount = Sensors.Count
                };
                foreach (var sensor in Sensors)
                {
                    MainTextBlockInit(sensor);
                }
                return true;
            });
            CreateCommandProperty = new AsyncCommand<bool>(async () =>
            {
                observer.Subscribe(_sensorObservable);
                await SensorOptions.XmlSerializeAsync(Sens);
                await _sensorObservable.NotifyAsync(true);
                return true;
            });
            DeleteCommandProperty = new AsyncCommand<bool>(async () =>
            {
                await SensorOptions.DeleteAsync(Id, true);
                await _sensorObservable.NotifyAsync(true);
                var startCommandProperty = StartCommandProperty;
                return true;
            });
            ChangeCommandProperty = new AsyncCommand<bool>(async () =>
            {
                await SensorOptions.ChangeAsync(Id, Sens, true);
                var startCommandProperty = StartCommandProperty;
                return true;
            });
            FindCommandProperty = new AsyncCommand<bool>(async () =>
            {
                ISensor findSensor = await SensorOptions.FindAsync(Id, true);
                if (findSensor == null)
                {
                    return false;
                }

                MainTextBlock = "";
                MainTextBlockInit(findSensor);

                Sens = findSensor;
                return true;
            });
            StartCommandProperty = new AsyncCommand<bool>(async () =>
            {
                List<Task> tasks = new List<Task>();
                if (Sensors.Count == 0)
                {
                    return false;
                }
                tasks.Add(taskFactory.StartNew(async () => await SensorOptions.ValueCountingAsync(Sensors)));
                await Task.WhenAny(tasks);
                return true;
            });
        }

        public static void GetMessage(string message)
        {
            _message = message;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MainTextBlockInit(ISensor sensor)
        {
            MainTextBlock += "ID: " + sensor.Id.ToString() + "\n";
            MainTextBlock += "SensorType: " + SensorType + "\n";
            MainTextBlock += "Measured Name: " + sensor.MeasuredName + "\n";
            MainTextBlock += "Measured Value: " + sensor.MeasuredValue + "\n";
            MainTextBlock += "Interval: " + sensor.Interval.ToString() + "\n";
            MainTextBlock += "Mode: " + Enum.GetName(sensor.Mode) + "\n" + "\n";
        }
    }
}