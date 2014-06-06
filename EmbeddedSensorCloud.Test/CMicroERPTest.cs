using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmbeddedSensorCloud;
using MicroERP;

namespace EmbeddedSensorCloud.Test
{
    [TestClass]
    public class CMicroERPTest
    {
        //http://pastebin.com/aCxH7yT1
        [TestMethod]
        public void CMicroERPTest_CheckContactList()
        {
            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);
            CWebURL urlobject = new CWebURL("MicroERP.html", "mode=search&contact=sdsdfsdf");


            List<ContactObject> list = new List<ContactObject>();
            ContactObject o1 = new ContactObject();
            o1.Vorname = "Niki";
            o1.Nachname = "Helm";

            ContactObject o2 = new ContactObject();
            o2.Vorname = "Berni";
            o2.Nachname = "Bauch";

            list.Add(o1);
            list.Add(o2);

            MicroERP.MicroERP tempPlugin = new MicroERP.MicroERP();

            tempPlugin.Load(writer, urlobject);
            DALFactory.dataAccesObj = new MicroERP.FakeDAL();
            tempPlugin.doSomething();


        }
    }
}
