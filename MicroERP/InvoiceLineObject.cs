using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmbeddedSensorCloud;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MicroERP
{
    [Serializable()]
    public class InvoiceLineObject
    {
        [XmlElement(ElementName = "Menge")]
        public string Menge { get; set; }
        [XmlElement(ElementName = "Stkpreis")]
        public string Stkpreis { get; set; }
        [XmlElement(ElementName = "UST")]
        public string UST { get; set; }
        [XmlElement(ElementName = "FK_Rechnung")]
        public string FK_Rechnung { get; set; }
    }
}
