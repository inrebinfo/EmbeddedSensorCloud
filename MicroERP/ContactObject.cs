using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmbeddedSensorCloud;
using System.Xml.Serialization;

namespace MicroERP
{

    [Serializable()]
    public class ContactObject
    {
        //Allgemein
        public string Typ { get; set; }
        
        //Suchstring
        public string NameToSearch { get; set; }

        //Person
        [XmlElement(ElementName = "ID")]
        public string ID { get; set; }
        [XmlElement(ElementName = "Titel")]
        public string Titel { get; set; }
        [XmlElement(ElementName = "Vorname")]
        public string Vorname { get; set; }
        [XmlElement(ElementName = "Nachname")]
        public string Nachname { get; set; }
        [XmlElement(ElementName = "Suffix")]
        public string Suffix { get; set; }
        [XmlElement(ElementName = "Geburtsdatum")]
        public string Geburtsdatum { get; set; }
        [XmlElement(ElementName = "Strasse")]
        public string Strasse { get; set; }
        [XmlElement(ElementName = "PLZ")]
        public string PLZ { get; set; }
        [XmlElement(ElementName = "Ort")]
        public string Ort { get; set; }

        //Firma
        [XmlElement(ElementName = "Firmenname")]
        public string Firmenname { get; set; }
        [XmlElement(ElementName = "UID")]
        public string UID { get; set; }

        [XmlElement(ElementName = "FK_Kontakt")]
        public string FK_Kontakt { get; set; }



        public override bool Equals(object obj)
        {
            ContactObject other = obj as ContactObject;

            if (other == null)
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            return
            this.ID == other.ID &&
            this.Titel == other.Titel &&
            this.Vorname == other.Vorname &&
            this.Nachname == other.Nachname &&
            this.Suffix == other.Suffix &&
            this.Geburtsdatum == other.Geburtsdatum &&
            this.Strasse == other.Strasse &&
            this.PLZ == other.PLZ &&
            this.Ort == other.Ort &&
            this.Firmenname == other.Firmenname &&
            this.UID == other.UID &&
            this.FK_Kontakt == other.FK_Kontakt;
        }
    }
}
