
using System.Xml;

namespace Exercise3.Models
{
    public class LocationModel
    {
        public string Long { get; set; }
        public string Lat { get; set; }
        public string Rudder { get; set; }
        public string Throttle { get; set; }

        public LocationModel()
        {
            Long = "-1";
            Lat = "-1";
        }

        public LocationModel(string longitude, string latiture)
        {
            Long = longitude;
            Lat = latiture;
        }

        public XmlWriter ToXml(XmlWriter writer)
        {
            // Given an XmlWriter, Write the LocationModel to an XML
            writer.WriteStartElement("Location");
            writer.WriteElementString("Long", Long);
            writer.WriteElementString("Lat", Lat);
            writer.WriteElementString("Rudder", Rudder);
            writer.WriteElementString("Throttle", Throttle);
            writer.WriteEndElement();

            return writer;
        }
    }
}