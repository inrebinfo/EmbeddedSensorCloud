using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroERP
{
    public class DALFactory
    {
        public static IDatabase dataAccesObj;

        public static object dalFacSearchContact(string param)
        {
            return dataAccesObj.realSearchContact(param);
        }

        public static object dalFacSearchCompany(string param)
        {
            return dataAccesObj.realSearchCompany(param);
        }

        public static object dalFacSearchInvoice(string param)
        {
            return dataAccesObj.realSearchInvoice(param);
        }

        public static object dalFacSearchInvoiceLines(string param)
        {
            return dataAccesObj.realSearchInvoiceLines(param);
        }
    }
}
