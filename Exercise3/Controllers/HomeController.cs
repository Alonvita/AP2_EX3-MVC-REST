using System;
using System.Web.Mvc;
using System.Text;
using System.Xml;
using Exercise3.Models;

namespace Exercise3.Controllers
{
    public class MainController : Controller
    {
        private ClientModel client = ClientModel.Instance;
        private string requestUserName = null;

        [HttpGet]
        public ActionResult displayPanel(string ipOffest0, string ipOffest1, string ipOffest2, 
                                    string ipOffest3, int port, int timeAsInt)
        {
            string ip = ipOffest0 + "." + ipOffest1 + "." + ipOffest2 + "." + ipOffest3;
            client.OpenConnection(ip, port);

            if (client.IsConnected())
                Session["time"] = timeAsInt;

            return View();
        }

        [HttpGet]
        public ActionResult saveDisplay(string ip, int port, int timeAsInt, int fromStartSeconds, string clientName)
        {
            client.OpenConnection(ip, port);
            client.Name = clientName;

            Session["time"] = fromStartSeconds;
            Session["timoutSave"] = timeAsInt;

            return View();
        }

        [HttpGet]
        public ActionResult displayFile(string username, int interval)
        {
            Session["interval"] = interval;
            Session["name"] = username;

            requestUserName = username;

            return View();
        }

        [HttpPost]
        public string displayToUser()
        {
            string addString = @"\" + Session["name"] + ".xml";

            string filename = 
                AppDomain.CurrentDomain.BaseDirectory + addString;

            return DataBaseHandler.Instance.ReadData(filename);
        }

        [HttpPost]
        public string getLongitudeAndLatitude()
        {
            return ToXml(client.Location);
        }


        public string ToXml(LocationModel loc)
        {
            // Initialize stringbuilder
            StringBuilder stringBuilder = new StringBuilder();

            // create an xmlWriterSettings
            XmlWriterSettings st = new XmlWriterSettings();

            // create an XML writer
            XmlWriter writer = XmlWriter.Create(stringBuilder, st);

            // start writing
            writer.WriteStartDocument();
            writer.WriteStartElement("LocationModel"); // LocationModel

            // write
            loc.ToXml(writer);

            // long, lat
            writer.WriteElementString("Long", loc.Long);
            writer.WriteElementString("Lat", loc.Lat);

            // write
            writer.WriteEndElement();

            // end
            writer.WriteEndDocument();
            writer.Flush(); // empty writer

            return stringBuilder.ToString();
        }

        [HttpPost]
        public string saveCurrentViewToXML()
        {
            LocationModel l = new LocationModel(client.Long.ToString(), client.Lat.ToString());


            string filename = AppDomain.CurrentDomain.BaseDirectory + @"\" + client.Name + ".xml";
            DataBaseHandler.Instance.SaveData(filename);
            return ToXml(l);
        }
    }
}