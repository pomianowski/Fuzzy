using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyNumbers
{
    class Encoder
    {
        public static T Read<T>(string filePath) where T : new()
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                return (T)binaryFormatter.Deserialize(stream);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
            }
        }

        public static void Write<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                binaryFormatter.Serialize(stream, objectToWrite);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
            }
        }
    }
}
