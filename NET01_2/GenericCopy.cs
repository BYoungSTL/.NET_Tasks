using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NET01_2
{
    public static class GenericCopy
    {
        /// <summary>
        /// Extension method for generic copying
        /// copying is necessary to track changes
        /// </summary>
        /// <param name="objCopy"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Copy<T>(this T objCopy)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, objCopy);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (T) binaryFormatter.Deserialize(memoryStream);
            }
        }
    }
}