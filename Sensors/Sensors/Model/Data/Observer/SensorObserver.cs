using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sensors.Model.Data.Factory;
using Sensors.ViewModel;

namespace Sensors.Model.Data.Observer
{
    public class SensorObserver : IObserver<SensorMonitor>
    {
        private const string OnCompletedMessage = "Data will not be transmitted";
        private IDisposable _unsubscriber;
        public SensorMonitor LastProvider { get; set; }

        public int PrevCount { get; set; }


        public virtual void Subscribe(IObservable<SensorMonitor> provider)
        {
            _unsubscriber = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }

        public void OnCompleted()
        {
            JsonViewModel.GetMessage(OnCompletedMessage);
            XmlViewModel.GetMessage(OnCompletedMessage);
        }

        public void OnError(Exception error)
        {
            JsonViewModel.GetMessage(error.Message);
            XmlViewModel.GetMessage(error.Message);
        }

        public void OnNext(SensorMonitor value)
        {
            JsonViewModel.GetMessage($"Sensors Count:{LastProvider.SensorCount} -> {value.SensorCount}\n");
            XmlViewModel.GetMessage($"Sensors Count:{LastProvider.SensorCount} -> {value.SensorCount}\n");
            LastProvider.SensorCount = value.SensorCount;
        }

        public async Task HandlingNotifyAsync(bool isXml)
        {
            List<ISensor> sensors;
            if (isXml)
            {
                sensors = await SensorOptions.XmlDeserializeAsync();
            }
            else
            {
                sensors = await SensorOptions.JsonDeserializeAsync();
            }
            SensorMonitor provider = new SensorMonitor
            {
                SensorCount = sensors.Count
            };
            foreach (var sensor in sensors)
            {
                if (sensor != null)
                {
                    if (PrevCount != sensors.Count)
                    {
                        provider.SensorCount = sensors.Count;
                        OnNext(provider);
                        PrevCount = sensors.Count;
                    }
                }
                else
                {
                    Unsubscribe();
                    OnCompleted();
                    break;
                }
            }
        }
    }
}