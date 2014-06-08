using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroERP
{
    public static class BusinessLayer
    {
        //BUSINESSLAYER

        //search
        public static List<ContactObject> blSearchContacts(string param)
        {
            List<ContactObject> res = (List<ContactObject>)DALFactory.dalFacSearchContact(param);

            return res;
        }

        public static List<ContactObject> blSearchCompany(string param)
        {
            List<ContactObject> res = (List<ContactObject>)DALFactory.dalFacSearchCompany(param);

            return res;
        }

        public static List<InvoiceObject> blSearchInvoice(string param)
        {
            List<InvoiceObject> res = (List<InvoiceObject>)DALFactory.dalFacSearchInvoice(param);

            return res;
        }

        public static List<InvoiceLineObject> blSearchInvoiceLines(string param)
        {
            List<InvoiceLineObject> res = (List<InvoiceLineObject>)DALFactory.dalFacSearchInvoiceLines(param);

            return res;
        }

        public static void blInsertContact(string param)
        {
            DALFactory.dalFacInsertContact(param);
        }

        public static void blUpdateContact(string param)
        {
            DALFactory.dalFacUpdateContact(param);
        }

        public static void blInsertInvoice(string param)
        {
            DALFactory.dalFacInsertInvoice(param);
        }
    }
}
