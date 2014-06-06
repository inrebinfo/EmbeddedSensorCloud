using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroERP
{
    public class FakeDAL : IDatabase
    {
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

            //o1.ErstellungsDatum = "06-06-2014";

            list.Add(o1);


            return list;
        }


        public object realSearchInvoiceLines(string searchstring)
        {
            return SearchInvoiceLines(searchstring);
        } 

        public List<InvoiceLineObject> SearchInvoiceLines(string searchstring)
        {
            return null;
        }
    }
}
