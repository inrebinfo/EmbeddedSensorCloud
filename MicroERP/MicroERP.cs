using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using EmbeddedSensorCloud;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace MicroERP
{
    public class MicroERP : IPlugin
    {
        private string _pluginName = "MicroERP";
        private StreamWriter _writer;
        private CWebURL _url;

        private DBCon _dbCon = new DBCon();
        private CWebResponse _response;


        int toadd;

        #region ContactInformation

        private string _type;

        private string _searchString;

        private string _titel;
        private string _vorname;
        private string _nachname;
        private string _suffix;
        private string _geburtstag;
        private string _strasse;
        private string _plz;
        private string _ort;

        private string _firmenname;
        private string _uid;

        #endregion

        public void Load(StreamWriter writer, CWebURL url)
        {
            Console.WriteLine(_pluginName + " loaded");
            _writer = writer;
            _url = url;
            _response = new CWebResponse(_writer);
        }

        public void doSomething()
        {
            foreach (KeyValuePair<string, string> entry in _url.WebParameters)
            {
                if (entry.Key == "mode" && entry.Value == "search")
                {
                    foreach (KeyValuePair<string, string> entry2 in _url.WebParameters)
                    {
                        if (entry2.Key == "contact")
                        {
                            _searchString = entry2.Value.ToString();

                            List<ContactObject> resultContact = new List<ContactObject>();

                            resultContact = _dbCon.SearchContact(_searchString);

                            sendResponse(resultContact);
                        }
                        else if (entry2.Key == "company")
                        {
                            _searchString = entry2.Value.ToString();

                            List<ContactObject> resultContact = new List<ContactObject>();

                            resultContact = _dbCon.SearchCompany(_searchString);

                            sendResponse(resultContact);
                        }
                        else if (entry2.Key == "invoice")
                        {
                            _searchString = entry2.Value.ToString();

                            List<ContactObject> resultContact = new List<ContactObject>();

                            resultContact = _dbCon.SearchContact(_searchString);

                            sendResponse(resultContact);
                        }
                    }
                }
                else if (entry.Key == "mode" && entry.Value == "insert")
                {
                    foreach (KeyValuePair<string, string> entry2 in _url.WebParameters)
                    {
                        if (entry2.Key == "contact")
                        {
                            _searchString = entry2.Value.ToString();

                            foreach (KeyValuePair<string, string> entry3 in _url.WebParameters)
                            {
                                if (entry3.Key == "toadd")
                                {
                                    toadd = Convert.ToInt32(entry3.Value);
                                }
                            }

                            if (toadd == 1)
                            {
                                _searchString += "=";

                            }
                            else if (toadd == 2)
                            {
                                _searchString += "==";
                            }

                            byte[] arr = Convert.FromBase64String(_searchString);
                            _searchString = System.Text.Encoding.UTF8.GetString(arr);

                            _dbCon.InsertContact(_searchString);
                        }
                        else if (entry2.Key == "invoice")
                        {
                            _searchString = Convert.FromBase64String(entry2.Value + "==").ToString();

                            List<ContactObject> resultContact = new List<ContactObject>();

                            resultContact = _dbCon.SearchCompany(_searchString);

                            sendResponse(resultContact);
                        }
                    }
                }
                else if (entry.Key == "mode" && entry.Value == "update")
                {

                    foreach (KeyValuePair<string, string> entry2 in _url.WebParameters)
                    {

                        _searchString = entry2.Value.ToString();

                        foreach (KeyValuePair<string, string> entry3 in _url.WebParameters)
                        {
                            if (entry3.Key == "toadd")
                            {
                                toadd = Convert.ToInt32(entry3.Value);
                            }
                        }


                        if (entry2.Key == "contact")
                        {
                            _searchString = entry2.Value;

                            if (toadd == 1)
                            {
                                _searchString += "=";

                            }
                            else if (toadd == 2)
                            {
                                _searchString += "==";
                            }

                            byte[] arr = Convert.FromBase64String(_searchString);
                            _searchString = System.Text.Encoding.UTF8.GetString(arr);

                            _dbCon.UpdateContact(_searchString);
                        }
                    }
                }
            }
        }


        //public void sendResponse(ContactObject contact)
        public void sendResponse(List<ContactObject> contacts)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ContactObject>));

            byte[] respBytes;

            StringWriter textWriter = new StringWriter();

            MemoryStream memstream = new MemoryStream();
            //serializer.Serialize(memstream, contacts);
            serializer.Serialize(textWriter, contacts);
            respBytes = memstream.ToArray();

            //string response = System.Text.Encoding.UTF8.GetString(respBytes);
            string response = textWriter.ToString();

            //response = response.Replace("\r\n", "");

            //_response.WriteResponse(textwriter.toString());


            int size = ASCIIEncoding.ASCII.GetByteCount(response);

            _response.ContentType = "text/xml";
            _response.ContentLength = size * 2;
            _response.WriteResponse(response);
        }


        //wpf ding schickt anfrage über verschiedene layer mittels webrequest
        //url schaut so aus: http://localhost:8080/MicroERP.html?mode=insert
        //oder so: http://localhost:8080/MicroERP.html?mode=search
        //oder so: http://localhost:8080/MicroERP.html?mode=edit

        //ganz normal mit _url verfahren wie in den alten plugins
        //je nach mode einfach dann darin die verschiedenen methoden aufrufen
        //die verschiedenen methoden (insert, search, etc.) gehören in DBCon.cs oder in eine eigene Klasse noch, DAL (!)
        //bei ?mode=search dann ContactObject erstellen und returnen
        //anschließend hier (Business Layer) ein xml vom ContactObject erstellen
        //bei ?mode=insert und ?mode=edit irgendein true oder sonst was zurückschicken, das muss im wpfding dann gemacht werden

        //fassade ist eigentlich CWebRequest, muss noch angepasst werden an das Beispiel hier (glaub ich) 












        public void Clean()
        {
            Console.WriteLine("cleaned " + _pluginName);
        }

        public string PluginName
        {
            get
            {
                return this._pluginName;
            }
        }
    }
}
