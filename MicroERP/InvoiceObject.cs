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
    public class InvoiceObject
    {
        //Person
        [XmlElement(ElementName = "ID")]
        public string ID { get; set; }
        [XmlElement(ElementName = "ErstellungsDatum")]
        public DateTime ErstellungsDatum { get; set; }
        [XmlElement(ElementName = "FaelligkeitsDatum")]
        public DateTime FaelligkeitsDatum { get; set; }
        [XmlElement(ElementName = "Kommentar")]
        public string Kommentar { get; set; }
        [XmlElement(ElementName = "Nachricht")]
        public string Nachricht { get; set; }
        [XmlElement(ElementName = "FK_Kontakt")]
        public string FK_Kontakt { get; set; }
        [XmlElement(ElementName = "Summe")]
        public string Summe { get; set; }
        [XmlElement(ElementName = "InvoiceLines")]
        public List<InvoiceLineObject> InvoiceLines { get; set; }
    }
}
