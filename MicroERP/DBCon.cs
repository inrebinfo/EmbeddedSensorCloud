using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Net;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

using EmbeddedSensorCloud;

namespace MicroERP
{
    public class DBCon : IDatabase
    {
        private SqlConnection _SQLCon;
        private string _ConnectionString;

        public DBCon()
        {
            //_SQLCon = new SqlConnection(@"Data Source=.\SqlExpress;Initial Catalog=MicroERP;Integrated Security=true;");
            _ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=MicroERP;Integrated Security=true;";
        }

        public object realSearchContact(string searchstring)
        {
            return SearchContact(searchstring);
        }

        public List<ContactObject> SearchContact(string searchstring)
        {
            string encSearchString = WebUtility.UrlDecode(searchstring);
            List<ContactObject> contacts = new List<ContactObject>();

            //string searchQuery = @"SELECT id, titel, vorname, nachname, suffix, geburtsdatum, firmenname, uid, strasse, plz, ort, fk_kontakt FROM MicroERP.dbo.Kontakte WHERE Vorname LIKE '%" + encSearchString + "%' OR Nachname LIKE '%" + encSearchString + "%' OR Firmenname LIKE '%" + encSearchString + "%'";

            //select * from Kontakte a left join Kontakte b on a.fk_kontakt = b.id where a.vorname like '%grub%' or a.nachname like '%grub%' or a.firmenname like '%grub%'
            /*string searchQuery = @"SELECT a.id, a.titel, a.vorname, a.nachname, a.suffix, a.geburtsdatum, b.firmenname, b.uid, a.strasse, a.plz, a.ort, a.fk_kontakt FROM MicroERP.dbo.Kontakte a
            left join Kontakte b on a.fk_kontakt = b.id
            WHERE a.vorname LIKE '%" + encSearchString + "%' OR a.nachname LIKE '%" + encSearchString + "%' OR a.firmenname LIKE '%" + encSearchString + "%'";*/

            /*string searchQuery = @"SELECT a.id, a.titel, a.vorname, a.nachname, a.suffix, a.geburtsdatum, a.firmenname, a.uid, a.strasse, a.plz, a.ort, a.fk_kontakt FROM MicroERP.dbo.Kontakte a
            left join Kontakte b on a.fk_kontakt = b.id
            WHERE a.vorname LIKE '%" + encSearchString + "%' OR a.nachname LIKE '%" + encSearchString + "%' OR a.firmenname LIKE '%" + encSearchString + "%'";*/

            string searchQuery = "SELECT * FROM Kontakte WHERE vorname LIKE '%" + encSearchString + "%' OR nachname LIKE '%" + encSearchString + "%' OR firmenname LIKE '%" + encSearchString + "%'";

            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand(searchQuery, con);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ContactObject resultContact = new ContactObject();
                        resultContact.ID = Convert.ToString(reader["id"] as decimal? ?? default(decimal));
                        resultContact.Titel = reader["titel"] as string;
                        resultContact.Vorname = reader["vorname"] as string;
                        resultContact.Nachname = reader["nachname"] as string;
                        resultContact.Suffix = reader["suffix"] as string;
                        resultContact.Geburtsdatum = reader["geburtsdatum"] as string;
                        resultContact.Firmenname = reader["firmenname"] as string;
                        resultContact.UID = reader["uid"] as string;
                        resultContact.Strasse = reader["strasse"] as string;
                        resultContact.PLZ = reader["plz"] as string;
                        resultContact.Ort = reader["ort"] as string;

                        if (reader["fk_kontakt"] != DBNull.Value)
                        {
                            using (SqlConnection con2 = new SqlConnection(_ConnectionString))
                            {
                                con2.Open();
                                string searchQuery2 = "select * from Kontakte where id = '" + reader["fk_kontakt"] + "'";

                                SqlCommand cmd2 = new SqlCommand(searchQuery2, con2);

                                SqlDataReader reader2 = cmd2.ExecuteReader();

                                while (reader2.Read())
                                {
                                    resultContact.FK_Kontakt = Convert.ToString(reader2["id"] as decimal? ?? default(decimal));
                                    resultContact.Firmenname = reader2["firmenname"] as string;
                                    resultContact.UID = reader2["uid"] as string;
                                }

                                con2.Close();
                            }
                        }

                        contacts.Add(resultContact);
                    }
                }
                catch (Exception ex)
                { }

                con.Close();
            }

            return contacts;
        }

        public object realSearchCompany(string searchstring)
        {
            return SearchCompany(searchstring);
        }

        public List<ContactObject> SearchCompany(string searchstring)
        {
            string encSearchString = WebUtility.UrlDecode(searchstring);
            List<ContactObject> contacts = new List<ContactObject>();

            string searchQuery = @"SELECT id, firmenname, uid, strasse, plz, ort FROM MicroERP.dbo.Kontakte WHERE Firmenname LIKE '%" + encSearchString + "%'";

            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand(searchQuery, con);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ContactObject resultContact = new ContactObject();
                        resultContact.ID = reader["id"].ToString();
                        resultContact.Firmenname = reader["firmenname"].ToString();
                        resultContact.UID = reader["uid"].ToString();
                        resultContact.Strasse = reader["strasse"].ToString();
                        resultContact.PLZ = reader["plz"].ToString();
                        resultContact.Ort = reader["ort"].ToString();

                        contacts.Add(resultContact);
                    }
                }
                catch { }
                con.Close();
            }

            return contacts;
        }

        public object realSearchInvoice(string searchstring)
        {
            return SearchInvoice(searchstring);
        }

        public List<InvoiceObject> SearchInvoice(string searchstring)
        {
            string encSearchString = WebUtility.UrlDecode(searchstring);


            //kunde,datevon,datebis,preisvon,preisbis
            string[] parts = encSearchString.Split(',');

            List<InvoiceObject> invoices = new List<InvoiceObject>();



            string searchQuery = "select * from Rechnungen WHERE id in (Select id from Kontakte Where Vorname like '%" + parts[0] + "%' OR Nachname like '%" + parts[0] + "%' OR Firmenname like '%" + parts[0] + "%') OR datum BETWEEN '" + parts[1] + "' and '" + parts[2] + "' OR sum BETWEEN '" + parts[3] + "' and '" + parts[4] + "'";

            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand(searchQuery, con);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        InvoiceObject resultInvoice = new InvoiceObject();
                        /*resultContact.ID = Convert.ToString(reader["id"] as decimal? ?? default(decimal));
                        resultContact.Titel = reader["titel"] as string;
                        resultContact.Vorname = reader["vorname"] as string;
                        resultContact.Nachname = reader["nachname"] as string;
                        resultContact.Suffix = reader["suffix"] as string;
                        resultContact.Geburtsdatum = reader["geburtsdatum"] as string;
                        resultContact.Firmenname = reader["firmenname"] as string;
                        resultContact.UID = reader["uid"] as string;
                        resultContact.Strasse = reader["strasse"] as string;
                        resultContact.PLZ = reader["plz"] as string;
                        resultContact.Ort = reader["ort"] as string;*/

                        resultInvoice.ID = Convert.ToString(reader["id"] as decimal? ?? default(decimal));
                        resultInvoice.ErstellungsDatum = reader["datum"] as DateTime? ?? default(DateTime);
                        resultInvoice.FaelligkeitsDatum = reader["faellig"] as DateTime? ?? default(DateTime);
                        resultInvoice.Kommentar = Convert.ToString(reader["Kommentar"] as string);
                        resultInvoice.Nachricht = Convert.ToString(reader["Nachricht"] as string);

                        using (SqlConnection con3 = new SqlConnection(_ConnectionString))
                        {
                            con3.Open();
                            string searchQuery3 = "select * from Kontakte where id = '" + reader["fk_kontakt"] + "'";

                            SqlCommand cmd3 = new SqlCommand(searchQuery3, con3);

                            SqlDataReader reader3 = cmd3.ExecuteReader();

                            while (reader3.Read())
                            {
                                resultInvoice.FK_Kontakt = reader3["vorname"] + ";" + reader3["nachname"] + ";" + reader3["firmenname"];
                            }

                            con3.Close();
                        }

                        invoices.Add(resultInvoice);
                    }
                }
                catch (Exception ex)
                { }

                con.Close();
            }

            return invoices;
        }

        public object realSearchInvoiceLines(string searchstring)
        {
            return SearchInvoiceLines(searchstring);
        }

        public List<InvoiceLineObject> SearchInvoiceLines(string searchstring)
        {
            string encSearchString = WebUtility.UrlDecode(searchstring);

            List<InvoiceLineObject> invoiceLines = new List<InvoiceLineObject>();

            string searchQuery = "select * from Rechnungszeile WHERE fk_rechnung = '" + encSearchString + "'";

            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand(searchQuery, con);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        InvoiceLineObject resultInvoiceLine = new InvoiceLineObject();

                        resultInvoiceLine.Menge = Convert.ToString(reader["menge"] as decimal? ?? default(decimal));
                        resultInvoiceLine.Stkpreis = Convert.ToString(reader["stkpreis"] as double? ?? default(double));
                        resultInvoiceLine.UST = Convert.ToString(reader["ust"] as decimal? ?? default(decimal));
                        resultInvoiceLine.FK_Rechnung = Convert.ToString(reader["fk_rechnung"] as decimal? ?? default(decimal)); ;

                        invoiceLines.Add(resultInvoiceLine);
                    }
                }
                catch (Exception ex)
                { }

                con.Close();
            }

            return invoiceLines;
        }

        public void UpdateContact(string searchstring)
        {
            string encSearchString = searchstring;

            ContactObject resultContact = new ContactObject();

            var serializer = new XmlSerializer(typeof(ContactObject), new XmlRootAttribute("ContactObject"));
            using (var stringReader = new StringReader(encSearchString))
            using (var reader2 = XmlReader.Create(stringReader))
            {
                var result = (ContactObject)serializer.Deserialize(reader2);
                resultContact = result;
            }

            string searchQuery = @"UPDATE MicroERP.dbo.Kontakte SET titel = '" + resultContact.Titel + @"', 
            vorname = '" + resultContact.Vorname + @"', 
            nachname = '" + resultContact.Nachname + @"', 
            suffix = '" + resultContact.Suffix + @"', 
            geburtsdatum = '" + resultContact.Geburtsdatum + @"', 
            firmenname = '" + resultContact.Firmenname + @"', 
            uid = '" + resultContact.UID + @"', 
            strasse = '" + resultContact.Strasse + @"', 
            ort = '" + resultContact.Ort + @"', 
            plz = '" + resultContact.PLZ + @"', 
            fk_kontakt = '" + resultContact.FK_Kontakt + @"'
            WHERE id = '" + resultContact.ID + "'";


            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand(searchQuery, con);

                    cmd.ExecuteNonQuery();
                }
                catch { }

                con.Close();
            }
        }

        public void InsertContact(string searchstring)
        {
            string encSearchString = searchstring;

            ContactObject resultContact = new ContactObject();

            var serializer = new XmlSerializer(typeof(ContactObject), new XmlRootAttribute("ContactObject"));
            using (var stringReader = new StringReader(encSearchString))
            using (var reader2 = XmlReader.Create(stringReader))
            {
                var result = (ContactObject)serializer.Deserialize(reader2);
                resultContact = result;
            }

            string searchQuery = @"INSERT INTO MicroERP.dbo.Kontakte
            (titel, vorname, nachname, suffix, geburtsdatum, firmenname, uid, strasse, ort, plz, fk_kontakt) VALUES
            ('" + resultContact.Titel + "', '" + resultContact.Vorname + "', '" + resultContact.Nachname + "', '" + resultContact.Suffix + "', '" + resultContact.Geburtsdatum + "', '" + resultContact.Firmenname + "', '" + resultContact.UID + "', '" + resultContact.Strasse + "', '" + resultContact.Ort + "', '" + resultContact.PLZ + "', '" + resultContact.FK_Kontakt + "')";


            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand(searchQuery, con);

                    cmd.ExecuteNonQuery();
                }
                catch { }

                con.Close();
            }
        }

        public void InsertInvoice(string searchstring)
        {
            string encSearchString = searchstring;

            InvoiceObject resultInvoice = new InvoiceObject();

            var serializer = new XmlSerializer(typeof(InvoiceObject), new XmlRootAttribute("InvoiceObject"));
            using (var stringReader = new StringReader(encSearchString))
            using (var reader2 = XmlReader.Create(stringReader))
            {
                var result = (InvoiceObject)serializer.Deserialize(reader2);
                resultInvoice = result;
            }

            string searchQuery = @"INSERT INTO MicroERP.dbo.Rechnungen
            (datum, faellig, kommentar, nachricht, fk_kontakt, sum) VALUES
            ('" + resultInvoice.ErstellungsDatum + "', '" + resultInvoice.FaelligkeitsDatum + "', '" + resultInvoice.Kommentar + "', '" + resultInvoice.Nachricht + "', '" + resultInvoice.FK_Kontakt + "', '" + resultInvoice.Summe + "')";


            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand(searchQuery, con);

                    cmd.ExecuteNonQuery();
                }
                catch { }

                con.Close();
            }

            using (SqlConnection con2 = new SqlConnection(_ConnectionString))
            {
                con2.Open();

                try
                {
                    foreach (InvoiceLineObject obj in resultInvoice.InvoiceLines)
                    {
                        string insertString = @"INSERT INTO MicroERP.dbo.Rechnungszeile
                            (menge, stkpreis, fk_rechnung, ust) VALUES
                            ('" + obj.Menge + "', '" + obj.Stkpreis + "', (Select Max(id)+1 from Rechnungen), '" + obj.UST + "')";

                        SqlCommand cmd = new SqlCommand(insertString, con2);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch { }

                con2.Close();
            }
        }
    }
}
