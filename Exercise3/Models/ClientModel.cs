using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace REST_WEB.Models
{
    public class ClientModel
    {

        private string username;

        private string fileName = self.username + ".xml";

        List<List<float>> myList;
        private TcpClient client;
        public int time { get; set; }

        private bool connected = false;
        private BinaryReader binaryReader;
        private BinaryWriter binaryWriter;
        private NetworkStream nStream = null;

        private static ClientModel self = null;

        private LocationModel CurrentLocation { get; set; }
        private readonly static object locker = new object();
       

        private ClientModel() {
            CurrentLocation = new LocationModel();
        }

        /// <summary>
        /// Instantiage as a singleton.
        /// </summary>
        public static ClientModel Instance
        {
            get
            {
                lock (locker)
                {
                    // if self is null, need to create a new ClientModel.
                    if (self == null)
                    {
                        self = new ClientModel();
                    }

                    // return the created ClientModel or the current client model
                    return self;
                }
            }
        }

        /// <summary>
        /// isConnected()
        /// </summary>
        /// <returns> true if connected, or false otherwise. </returns>
        private bool isConnected()
        {
            return connected;
        }

        /// <summary>
        /// SaveToFile(string data).
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SaveToFile(string dataToWrite)
        {
            StreamWriter writer = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName);

            writer.WriteLine(dataToWrite);
            writer.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fp"></param>
        public void ReadFile(string fp)
        {
            // get the full path of the file
            string[] lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\" + fp + ".xml");

            List<List<float>> listTmp = new List<List<float>>();

            for (int i = 0; i < lines.Length; i++)
                listTmp.Add(SplitStrToListOfFloats(lines[i]));

            myList = listTmp;
        }

        /// <summary>
        /// SplitStrToListOfFloats(string str).
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private List<float> SplitStrToListOfFloats(string str)
        {
            List<float> list = new List<float>();

            String[] data = str.Split(' ');

            // parse first place
            list.Add(float.Parse(data[0]));


            // parse second place
            list.Add(float.Parse(data[1]));

            return list;
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void OpenConnection(string ip, int port)
        {
            // check if user is already connected
            if (connected)
                return;

            // create a new TCP client
            client = new TcpClient(ip, port);

            nStream = client.GetStream();
            nStream.Flush();

            Console.WriteLine("User has successfully connected! ");
            connected = true;

            // add read write
            self.InstantiareReadWrite();
        }

        private void InstantiareReadWrite()
        {
            binaryReader = new BinaryReader(nStream);
            binaryWriter = new BinaryWriter(nStream);
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            set
            {
                username = value;
            }
            get
            {
                return username;
            }
        }

        /// <summary>
        /// Close().
        /// 
        /// </summary>
        public void Close()
        {
            nStream.Close();
            client.Close();
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="commandToSend"></param>
        /// <returns></returns>
        private double GetInfo(string commandToSend)
        {
            lock (locker)
            {
                // Convert to an array of bytes
                binaryWriter.Write(Encoding.ASCII.GetBytes(commandToSend));

                char currentChar;
                string inputReceived = "";

                // read the whole thing
                while ((currentChar = binaryReader.ReadChar()) != '\n')
                {
                    inputReceived += currentChar;
                }

                nStream.Flush();

                return Parser(inputReceived);
            }
        }

        /// <summary>
        /// Parser(string toParse).
        /// </summary>
        /// <param name="toParse"></param>
        /// <returns></returns>
        private double Parser(string toParse)
        {
            string[] words = toParse.Split('\'');
            return Convert.ToDouble(words[1]);
        }

        /// <summary>
        /// Long.
        /// </summary>
        public double Long
        {
            get
            {
                // get the longitude info
                double l = GetInfo("get /position/longitude-deg\r\n");

                // update CurrentLocationimut
                CurrentLocation.Long = l.ToString();
                return l;
            }

            set {; }
        }

        /// <summary>
        /// Lat.
        /// </summary>
        public double Lat
        {
            get
            {
                // get info
                double l = GetInfo("get /position/latitude-deg\r\n");

                // update
                CurrentLocation.Lat = l.ToString();
                return l;


            }
            set {; }

        }

        /// <summary>
        /// Rudder.
        /// </summary>
        public double Rudder
        {
            get
            {
                // get info
                double r = GetInfo("get /controls/flight/rudder\r\n");


                // update
                CurrentLocation.Rudder = r.ToString();
                return r;
            }
            set {; }
        }

        /// <summary>
        /// Throttle.
        /// </summary>
        public double Throttle
        {
            get
            {
                // get info
                double t = GetInfo("get /controls/engines/current-engine/throttle\r\n");

                // update
                CurrentLocation.Lat = t.ToString();
                return t;


            }
            set {; }

        }

        /// <summary>
        /// Location.
        /// </summary>
        public LocationModel Location
        {
            get
            {
                return CurrentLocation;
            }

            private set {; }
        }
        

        /// <summary>
        /// IsConnected()
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return connected;
        }
    }
}