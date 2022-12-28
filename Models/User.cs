using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SkrinTestTask.Models
{
    public class User
    {
        [XmlElement("fio")]
        public string FIO { get; set; }
        [XmlElement("email")]
        public string Email { get; set; }
    }
}
