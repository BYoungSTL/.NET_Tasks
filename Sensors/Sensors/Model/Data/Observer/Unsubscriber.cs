using System;
using System.Collections.Generic;

namespace Sensors.Model.Data.Observer
{
    public class Unsubscriber : IDisposable
    {
        private readonly List<IObserver<SensorMonitor>> _observers;
        private readonly IObserver<SensorMonitor> _observer;

        public Unsubscriber(List<IObserver<SensorMonitor>> observers, IObserver<SensorMonitor> observer)
        {
            _observer = observer;
            _observers = observers;
        }
        public void Dispose()
        {
            if (_observer != null) _observers.Remove(_observer);
        }
    }
}