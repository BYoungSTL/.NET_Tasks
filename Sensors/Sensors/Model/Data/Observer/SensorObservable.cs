using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sensors.Model.Data.Factory;

namespace Sensors.Model.Data.Observer
{
    public class SensorObservable : IObservable<SensorMonitor>
    {
        private readonly List<IObserver<SensorMonitor>> _observers;

        public SensorObservable()
        {
            _observers = new List<IObserver<SensorMonitor>>();
        }
        public IDisposable Subscribe(IObserver<SensorMonitor> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        public async Task NotifyAsync(bool isXml)
        {
            foreach (var obs in _observers)
            {
                await ((SensorObserver)obs).HandlingNotifyAsync(isXml);
            }
        }
    }
}