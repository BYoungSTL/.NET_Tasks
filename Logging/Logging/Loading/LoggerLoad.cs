using System;
using System.Reflection;

namespace Logging.Loading
{
    /// <summary>
    /// load 3 different assemblies from different dll
    /// </summary>
    public static class LoggerLoad
    {
        public static MethodInfo LoadMethod(Type entityType, string methodName)
        {
            return entityType.GetMethod(methodName);
        }
        public static Type Loader(string assemblyConfName, string assemblyName)
        {
            Assembly assemblyWord = Assembly.Load(new AssemblyName(assemblyConfName));
            return assemblyWord.GetType(assemblyName, true, true);
        }
    }
}