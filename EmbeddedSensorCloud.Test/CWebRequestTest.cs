using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using EmbeddedSensorCloud;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmbeddedSensorCloud.Test
{
    [TestClass]
    public class CWebRequestTest
    {
        [TestMethod]
        public void CWebRequest_ParseRequest_MicroERPPlugin_correct_Parsed()
        {
            string _pluginname = "MicroERP";
            string _header = @"GET /MicroERP.html HTTP/1.1
Host: localhost:8080" + System.Environment.NewLine + System.Environment.NewLine;


            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(_header);
            MemoryStream stream = new MemoryStream(byteArray);

            StreamReader _reader = new StreamReader(stream);

            var _WebReq = new CWebRequest(_reader);

            Assert.AreEqual(_pluginname, _WebReq.RequestedPlugin);
        }
    }
}
