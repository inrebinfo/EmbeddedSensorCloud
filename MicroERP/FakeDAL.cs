using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroERP
{
    public class FakeDAL : IDatabase
    {
        public List<ContactObject> contacts;
        public List<ContactObject> companies;
        public List<InvoiceObject> invoices;
        public List<InvoiceLineObject> invoicelines;

        public object realSearchContact(string searchstring)
        {
            return SearchContact(searchstring);
        }

        public List<ContactObject> SearchContact(string searchstring)
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

            return list;
        }

        public object realSearchCompany(string searchstring)
        {
            return SearchCompany(searchstring);
        }

        public List<ContactObject> SearchCompany(string searchstring)
        {
            List<ContactObject> list = new List<ContactObject>();
            ContactObject o1 = new ContactObject();
            o1.Firmenname = "Firma1";
            o1.UID = "12345678";

            list.Add(o1);


            return list;
        }

        public object realSearchInvoice(string searchstring)
        {
            return SearchInvoice(searchstring);
        }

        public List<InvoiceObject> SearchInvoice(string searchstring)
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


            return list;
        }

        public object realSearchInvoiceLines(string searchstring)
        {
            return SearchInvoiceLines(searchstring);
        } 

        public List<InvoiceLineObject> SearchInvoiceLines(string searchstring)
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

            return list;
        }

        public void realInsertContact(string searchstring)
        {
            InsertContact(searchstring);
        }

        public void InsertContact(string searchstring)
        {
        }

        public void realUpdateContact(string searchstring)
        {
            UpdateContact(searchstring);
        }

        public void UpdateContact(string searchstring)
        {
        }

        public void realInsertInvoice(string searchstring)
        {
            InsertInvoice(searchstring);
        }

        public void InsertInvoice(string searchstring)
        {
        }
    }
}
