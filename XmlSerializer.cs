using SkrinTestTask.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SkrinTestTask
{
    public class XmlSerializer<T> where T : class
    {
        public T Deserialize(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                T model = (T)xmlSerializer.Deserialize(fs);
                return model;
            }
        }
    }
}
