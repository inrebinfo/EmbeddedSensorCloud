﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace EmbeddedSensorCloud
{
    public class CWebServer : IDisposable
    {

        private bool _isRunning;
        private ArrayList _loadedPlugins;
        private TcpListener listener;
        public void Start()
        {
            new Thread(RunServer).Start();
            _isRunning = true;

            CPluginManager PluginManager = new CPluginManager();
            _loadedPlugins = PluginManager.LoadPlugins("/plugins/", "*.dll", typeof(EmbeddedSensorCloud.IPlugin));
        }

        public void RunServer()
        {
            Console.WriteLine("Server now listening on port 8080\n");
            //start listener on port 1337
            listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();

            //loop for server instances
            while (true)
            {
                //if we get an request accept socket and create new thread for the instance
                Socket SocketForClient = listener.AcceptSocket();
                if (SocketForClient.Connected)
                {
                    ParameterizedThreadStart ThreadsForClient = new ParameterizedThreadStart(NewConnection);
                    Thread ThreadForNewClient = new Thread(NewConnection);
                    ThreadForNewClient.Start(SocketForClient);
                }
            }
        }

        public void NewConnection(object SessionClient)
        {
            Socket sClient = (Socket)SessionClient;

            //networkstream for the data between the client and us
            NetworkStream StreamFromClient = new NetworkStream(sClient);

            //we need to write something back!
            StreamWriter WriterForClient = new StreamWriter(StreamFromClient);

            //we also need to read something
            StreamReader ReaderForClient = new StreamReader(StreamFromClient);

            CWebRequest WebRequest = new CWebRequest(ReaderForClient);
            
            CWebURL url = WebRequest.URLObject;

            

            int counter = 0;

            foreach (IPlugin plug in _loadedPlugins)
            {
                if (plug.PluginName == WebRequest.RequestedPlugin)
                {
                    counter++;
                    Console.WriteLine("requested plugin: " + plug.PluginName);

                    plug.Load(WriterForClient, url);
                    
                    plug.doSomething();
                }
            }

            Console.WriteLine();


            //CREATE RESPONSE
            /*if (counter == 0)
            {
                string html = @"
<html>
    <head>
        <title>EmbeddedSensorCloud</title>
    </head>
    <body>
        <h1>EmbeddedSensorCloud</h1>
        <p><a href='http://localhost:8080/TemperaturePlugin.html'>Temperature Plugin</a></p>
        <p><a href='http://localhost:8080/StaticPlugin.html'>Static Plugin</a></p>
        <p><a href='http://localhost:8080/NaviPlugin.html'>Navi Plugin</a></p>
        <p><a href='http://localhost:8080/StockPlugin.html'>Stock Plugin</a></p>6
        <br>
        <p><a href='http://localhost:8080/'>Startseite</a></p>
    </body>
</html>";

                int size = ASCIIEncoding.ASCII.GetByteCount(html);

                CWebResponse response = new CWebResponse(WriterForClient);
                response.ContentLength = html.Length;
                response.ContentType = "text/html";
                response.WriteResponse(html);
            }*/

            //close all writers and readers
            StreamFromClient.Close();
            WriterForClient.Close();
            ReaderForClient.Close();
            sClient.Close();
        }

        public bool isRunning
        {
            get { return _isRunning; }
            set { this._isRunning = value; }
        }

        public void Dispose()
        {
            listener.Stop();
        }
    }
}
