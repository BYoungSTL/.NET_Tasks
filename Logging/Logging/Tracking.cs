using System;
using System.Reflection;
using Logging.Attributes;
using Logging.Loading;
using NLog;

namespace Logging
{
    public static class Tracking<T>
    {
        private const string EntityAttributeName = "[Logging.Attributes.TrackingEntity()]";
        private const string PropertyAttributeName = "Logging.Attributes.TrackingProperty";
        /// <summary>
        /// Track objects by TrackingEntity and TrackingProperty attributes
        /// If Entity has a TrackingEntityAttribute and Properties from this Entity also has a TrackingPropertyAttribute
        /// then this property is tracking and logged
        /// </summary>
        /// <param name="obj"></param>

        public static void Track(T obj)
        {
            Type typeWordListener = LoggerLoad.Loader(LoadConfig.LogName, LoadConfig.Log);
            
            //Gets methods from loadable assemblies
            object wordListener = Activator.CreateInstance(typeWordListener);
            MethodInfo methodListener = LoggerLoad.LoadMethod(typeWordListener, Enum.GetName(Methods.LoggerSetting));
            MethodInfo methodDefaultOptions = LoggerLoad.LoadMethod(typeWordListener, Enum.GetName(Methods.DefaultLoggerOptions));
            
            Type entityType = typeof(T);
            var attributes = entityType.CustomAttributes;
            bool isTrackingEntity = false;

            //Checks TrackingEntity
            foreach (var attribute in attributes)
            {
                if (attribute.ToString().Equals(EntityAttributeName,
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
                        foreach (var attr in property.GetCustomAttributes(true))
                        {
                            //Gets Name and Value of property
                            if (attr is TrackingProperty prop)
                            {
                                propName = prop.ToString();
                            }
                        }
                        if (propName != null && propName.Equals(PropertyAttributeName,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            methodDefaultOptions?.Invoke(wordListener, null);
                            methodListener?.Invoke(wordListener, new object[]
                            {
                                LogLevel.Trace,
                                " " + property.Name + " = " + obj.GetType().GetProperty(property.Name)?.GetValue(obj, null)
                            });
                        }
                    }
                }
            }
        }
    }
}