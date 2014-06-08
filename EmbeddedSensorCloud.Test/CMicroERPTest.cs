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
        [TestMethod]
        public void ContactListCompareLength()
        {
            List<ContactObject> list = new List<ContactObject>();
            ContactObject o1 = new ContactObject();
            o1.Vorname = "Niki";
            o1.Nachname = "Helm";

            ContactObject o2 = new ContactObject();
            o2.Vorname = "Berni";
            o2.Nachname = "Bauch";

            list.Add(o1);
            list.Add(o2);

            DALFactory.dataAccesObj = new MicroERP.FakeDAL();
            List<ContactObject> result = BusinessLayer.blSearchContacts("string");

            Assert.AreEqual(list.Count, result.Count);
        }

        [TestMethod]
        public void ContactListCompareLists()
        {
            List<ContactObject> list = new List<ContactObject>();
            ContactObject o1 = new ContactObject();
            o1.Vorname = "Niki";
            o1.Nachname = "Helm";

            ContactObject o2 = new ContactObject();
            o2.Vorname = "Berni";
            o2.Nachname = "Bauch";

            list.Add(o1);
            list.Add(o2);

            DALFactory.dataAccesObj = new MicroERP.FakeDAL();
            List<ContactObject> result = BusinessLayer.blSearchContacts("string");


            int count = 0;

            foreach (ContactObject obj in list)
            {
                Assert.IsTrue(obj.Equals(result[count]));
                count++;
            }
        }

        [TestMethod]
        public void InvoiceListCompareLength()
        {
            List<InvoiceObject> list = new List<InvoiceObject>();
            InvoiceObject o1 = new InvoiceObject();
            o1.ErstellungsDatum = DateTime.Now;
            o1.FaelligkeitsDatum = DateTime.Now;
            o1.ID = "1";
            o1.FK_Kontakt = "1";
            o1.Kommentar = "Rechnungskommentar 1";
            o1.Nachricht = "Rechnungsnachricht 1";

            InvoiceObject o2 = new InvoiceObject();
            o2.ErstellungsDatum = DateTime.Now;
            o2.FaelligkeitsDatum = DateTime.Now;
            o2.ID = "2";
            o2.FK_Kontakt = "2";
            o2.Kommentar = "Rechnungskommentar 2";
            o2.Nachricht = "Rechnungsnachricht 2";

            list.Add(o1);
            list.Add(o2);

            DALFactory.dataAccesObj = new MicroERP.FakeDAL();
            List<InvoiceObject> result = BusinessLayer.blSearchInvoice("string");

            Assert.AreEqual(list.Count, result.Count);
        }

        [TestMethod]
        public void InvoiceListCompareLists()
        {
            List<InvoiceObject> list = new List<InvoiceObject>();
            InvoiceObject o1 = new InvoiceObject();
            o1.ErstellungsDatum = DateTime.Now;
            o1.FaelligkeitsDatum = DateTime.Now;
            o1.ID = "1";
            o1.FK_Kontakt = "1";
            o1.Kommentar = "Rechnungskommentar 1";
            o1.Nachricht = "Rechnungsnachricht 1";

            InvoiceObject o2 = new InvoiceObject();
            o2.ErstellungsDatum = DateTime.Now;
            o2.FaelligkeitsDatum = DateTime.Now;
            o2.ID = "2";
            o2.FK_Kontakt = "2";
            o2.Kommentar = "Rechnungskommentar 2";
            o2.Nachricht = "Rechnungsnachricht 2";

            list.Add(o1);
            list.Add(o2);

            DALFactory.dataAccesObj = new MicroERP.FakeDAL();
            List<InvoiceObject> result = BusinessLayer.blSearchInvoice("string");


            int count = 0;

            foreach (InvoiceObject obj in list)
            {
                Assert.IsTrue(obj.Equals(result[count]));
                count++;
            }
        }

        [TestMethod]
        public void InvoiceLineListCompareLength()
        {
            List<InvoiceLineObject> list = new List<InvoiceLineObject>();
            InvoiceLineObject o1 = new InvoiceLineObject();
            o1.Menge = "4";
            o1.Stkpreis = "8.89";
            o1.UST = "20";
            o1.FK_Rechnung = "1";

            InvoiceLineObject o2 = new InvoiceLineObject();
            o2.Menge = "4";
            o2.Stkpreis = "8.89";
            o2.UST = "20";
            o2.FK_Rechnung = "1";

            list.Add(o1);
            list.Add(o2);

            DALFactory.dataAccesObj = new MicroERP.FakeDAL();
            List<InvoiceLineObject> result = BusinessLayer.blSearchInvoiceLines("string");

            Assert.AreEqual(list.Count, result.Count);
        }

        [TestMethod]
        public void InvoiceLineListCompareLists()
        {
            List<InvoiceLineObject> list = new List<InvoiceLineObject>();
            InvoiceLineObject o1 = new InvoiceLineObject();
            o1.Menge = "4";
            o1.Stkpreis = "8.89";
            o1.UST = "20";
            o1.FK_Rechnung = "1";

            InvoiceLineObject o2 = new InvoiceLineObject();
            o2.Menge = "4";
            o2.Stkpreis = "8.89";
            o2.UST = "20";
            o2.FK_Rechnung = "1";

            list.Add(o1);
            list.Add(o2);

            DALFactory.dataAccesObj = new MicroERP.FakeDAL();
            List<InvoiceLineObject> result = BusinessLayer.blSearchInvoiceLines("string");


            int count = 0;

            foreach (InvoiceLineObject obj in list)
            {
                Assert.IsTrue(obj.Equals(result[count]));
                count++;
            }
        }
    }
}
