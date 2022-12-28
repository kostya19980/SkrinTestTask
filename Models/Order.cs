using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace SkrinTestTask.Models
{
    [XmlRoot("orders")]
    public class Orders
    {
        [XmlElement("order")]
        public List<Order> orders { get; set; }
    }
    public class Order
    {
        [XmlElement("no")]
        public int Id { get; set; }
        [XmlElement("reg_date")]
        public string RegDateString { get; set; }
        [XmlElement("sum")]
        public string TotalSumString { get; set; }
        [XmlElement("user")]
        public User User { get; set; }
        [XmlElement("product")]
        public List<Product> Products { get; set; }
        [XmlIgnore]
        public decimal TotalSum
        {
            get
            {
                decimal val;
                if (decimal.TryParse(TotalSumString.Replace(".", ","), out val))
                {
                    return val;
                }
                else return 0;
            }
        }

        [XmlIgnore]
        public DateTime RegDate
        {
            get
            {
                DateTime date;
                if (RegDateString != null && DateTime.TryParse(RegDateString, out date))
                {
                    return date;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }
    }
    public class Product
    {
        [XmlElement("quantity")]
        public int Quantity { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("price")]
        public string PriceString { get; set; }
        [XmlIgnore]
        public decimal Price
        {
            get
            {
                decimal val;
                if (decimal.TryParse(PriceString.Replace(".", ","), out val))
                {
                    return val;
                }
                else return 0;
            }
        }
    }
}
