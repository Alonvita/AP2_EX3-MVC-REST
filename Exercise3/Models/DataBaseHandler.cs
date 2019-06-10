
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Exercise3.Models
{
    public class DataBaseHandler 
    {
        private int index = 0;
        private static DataBaseHandler self = null;
        private readonly static object locker = new object();

        private DataBaseHandler() { }
        
        public static DataBaseHandler Instance
        {
            get
            {
                lock (locker)
                {
                    if (null == self)
                    {
                        self = new DataBaseHandler();
                    }
                    return self;
                }
            }
        }

        /// <summary>
        /// SaveData(string filename).
        /// </summary>
        /// <param name="fp"></param>
        /// <returns></returns>
        public string SaveData(string fp)
        {
            // File doesn't exist -> create a new Data Tree.
            if (!File.Exists(fp))
            {
                XDocument xData = new XDocument(new XElement("Location",
                    new XElement("Lon", ClientModel.Instance.Long),
                    new XElement("Lat", ClientModel.Instance.Lat),
                    new XElement("Rudder", ClientModel.Instance.Rudder),
                    new XElement("Throttle", ClientModel.Instance.Throttle)
                ));

                xData.Save(fp);
                return xData.ToString();
            }

            // File exists -> go
            XDocument xDocData = XDocument.Load(fp);
            XElement root = xDocData.Element("Location");

            IEnumerable<XElement> decendantsd = root.Descendants();

            // Build the last row into the xDocData
            decendantsd.Last().AddAfterSelf(
                new XElement("Lon", ClientModel.Instance.Long),
                new XElement("Lat", ClientModel.Instance.Lat),
                new XElement("Rudder", ClientModel.Instance.Rudder),
                new XElement("Throttle", ClientModel.Instance.Throttle));

            // save
            xDocData.Save(fp);
            // return
            return xDocData.ToString();
        }

        /// <summary>
        /// ReadData(string fp).
        /// </summary>
        /// <param name="fp"></param>
        /// <returns></returns>
        public string ReadData(string fp)
        {
            if (!File.Exists(fp))
                throw new FileNotFoundException("No such file...");

            // file exists -> go
            XDocument root = XDocument.Load(fp);
            IEnumerable<XElement> decendants = root.Element("Location").Descendants();

            if (decendants.Count() <= index)
            {
                // initialzie string builder
                StringBuilder stringBuilder = new StringBuilder();

                // init an XMLWRITTER
                XmlWriterSettings settings = new XmlWriterSettings();
                XmlWriter writer = XmlWriter.Create(stringBuilder, settings);

                // write
                writer.WriteStartDocument();
                writer.WriteElementString("stopFlagOn", "noFlagOn");
                writer.WriteEndDocument();
                writer.Flush();

                return stringBuilder.ToString();
            }
            
            // create a new xDoc 
            XDocument xDoc = new XDocument(new XElement("Location",
                    decendants.ElementAt(index++),
                    decendants.ElementAt(index++)
                ));


            // increase the index
            increaseIndex();

            return xDoc.ToString();
        }

        /// <summary>
        /// utility func.
        /// </summary>
        private void increaseIndex()
        {
            index += 2;
        }

        /// <summary>
        /// clearDataBase()
        /// </summary>
        private void clearDataBase()
        {
            // get the file path
            string fp = AppDomain.CurrentDomain.BaseDirectory + @"\" + ClientModel.Instance.Name + ".xml";

            // check exists
            if (!File.Exists(fp))
                return;

            File.Delete(fp);
            File.WriteAllText(fp, "");
        }
    }
}