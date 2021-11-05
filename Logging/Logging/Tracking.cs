using System;
using System.Reflection;
using Logging.Attributes;
using NLog;

namespace Logging
{
    public static class Tracking<T>
    {
        /// <summary>
        /// Track objects by TrackingEntity and TrackingProperty attributes
        /// If Entity has a TrackingEntityAttribute and Properties from this Entity also has a TrackingPropertyAttribute
        /// then this property is tracking and logged
        /// </summary>
        /// <param name="obj"></param>
        public static void Track(T obj)
        {
            Type typeTextListener = LoggerLoad.LoadTxtListener();
            Type typeLogListener = LoggerLoad.LoadLogListener();
            Type typeWordListener = LoggerLoad.LoadWordListener();
            
            Type entityType = typeof(T);
            var attributes = entityType.CustomAttributes;
            
            //Gets methods from loadable assemblies
            object textListener = Activator.CreateInstance(typeTextListener);
            MethodInfo textOptionsMethod = typeTextListener.GetMethod("DefaultLoggerOptions");
            MethodInfo textListenerMethod = typeTextListener.GetMethod("Listener");
            
            object wordListener = Activator.CreateInstance(typeWordListener);
            MethodInfo wordOptionsMethod = typeWordListener.GetMethod("DefaultLoggerOptions");
            MethodInfo wordListenerMethod = typeWordListener.GetMethod("Listener");
            
            bool isTrackingEntity = false;

            //Checks TrackingEntity
            foreach (var attribute in attributes)
            {
                if (attribute.ToString().Equals("[Logging.Attributes.TrackingEntity()]",
                    StringComparison.OrdinalIgnoreCase))
                {
                    isTrackingEntity = true;
                }
            }

            //Checks TrackingProperty
            if (isTrackingEntity)
            {
                var properties = entityType.GetProperties();
                foreach (var property in properties)
                {
                    var attrs = property.CustomAttributes;
                    foreach (var attribute in attrs)
                    {
                        string propName = null;
                        string propValue = null;
                        foreach (var attr in property.GetCustomAttributes(true))
                        {
                            //Gets Name and Value of property
                            if (attr is TrackingProperty prop)
                            {
                                propValue = prop._name;
                                propName = prop.ToString();
                            }
                        }
                        if (propName.Equals("Logging.Attributes.TrackingProperty",
                            StringComparison.OrdinalIgnoreCase))
                        {
                            textOptionsMethod?.Invoke(textListener, null);
                            textListenerMethod?.Invoke(textListener, new object[]
                            {
                                LogLevel.Trace,
                                " " + property.Name + " = " + obj.GetType().GetProperty(property.Name)?.GetValue(obj, null)
                            });

                            
                            wordOptionsMethod?.Invoke(wordListener, null);
                            wordListenerMethod?.Invoke(wordListener, new object[]
                            {
                                LogLevel.Trace,
                                " " + propValue + " = " + obj.GetType().GetProperty(property.Name)?.GetValue(obj, null)
                            });
                        }
                    }
                }
            }
        }
    }
}