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

        public override bool Equals(object obj)
        {
            InvoiceLineObject other = obj as InvoiceLineObject;

            if (other == null)
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            return
            this.Menge == other.Menge &&
            this.Stkpreis == other.Stkpreis &&
            this.UST == other.UST &&
            this.FK_Rechnung == other.FK_Rechnung;
        }
    }
}
