using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroERP
{
    public interface IDatabase
    {
        object realSearchContact(string param);
        object realSearchCompany(string param);
        object realSearchInvoice(string param);
        object realSearchInvoiceLines(string param);
        void realInsertContact(string param);
        void realUpdateContact(string param);
        void realInsertInvoice(string param);
    }
}
