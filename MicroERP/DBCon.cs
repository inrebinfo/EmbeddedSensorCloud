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
using System.Globalization;

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
            //_ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=MicroERP;Integrated Security=true;";
            _ConnectionString = Properties.Resources.connectionString;
        }

        public object realSearchContact(string searchstring)
        {
            return SearchContact(searchstring);
        }

        public List<ContactObject> SearchContact(string searchstring)
        {
            string encSearchString = WebUtility.UrlDecode(searchstring);
            List<ContactObject> contacts = new List<ContactObject>();
            
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

                        if (!string.IsNullOrWhiteSpace(reader["fk_kontakt"].ToString()))
                        {
                            using (SqlConnection con2 = new SqlConnection(_ConnectionString))
                            {
                                con2.Open();
                                string searchQuery2 = "select * from Kontakte where id = '" + reader["fk_kontakt"] + "'";

                                try
                                {

                                    SqlCommand cmd2 = new SqlCommand(searchQuery2, con2);

                                    SqlDataReader reader2 = cmd2.ExecuteReader();

                                    while (reader2.Read())
                                    {
                                        resultContact.FK_Kontakt = Convert.ToString(reader2["id"] as decimal? ?? default(decimal));
                                        resultContact.Firmenname = reader2["firmenname"] as string;
                                        resultContact.UID = reader2["uid"] as string;
                                    }
                                }
                                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                                con2.Close();
                            }
                        }

                        contacts.Add(resultContact);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

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

            string searchQuery;

            //name gesetzt, rest nicht, rechnung nach name suchen
            if((!string.IsNullOrWhiteSpace(parts[0]) && (string.IsNullOrWhiteSpace(parts[1]) && string.IsNullOrWhiteSpace(parts[2]) && string.IsNullOrWhiteSpace(parts[3]) && string.IsNullOrWhiteSpace(parts[4]))))
            {
                searchQuery = "select * from Rechnungen WHERE fk_kontakt in (Select id from Kontakte Where Vorname like '%" + parts[0] + "%' OR Nachname like '%" + parts[0] + "%' OR Firmenname like '%" + parts[0] + "%')";
            }
            //datum gesetzt, rest nicht, rechnung nach datum suchen
            else if((!string.IsNullOrWhiteSpace(parts[1]) && !string.IsNullOrWhiteSpace(parts[2])) && (string.IsNullOrWhiteSpace(parts[0]) && string.IsNullOrWhiteSpace(parts[3]) && string.IsNullOrWhiteSpace(parts[4])))
            {
                searchQuery = "select * from Rechnungen WHERE datum BETWEEN '" + parts[1] + "' and '" + parts[2] + "'";
            }
            //preis gesetzt, rest nicht, rechnung nach preis suchen
            else if ((!string.IsNullOrWhiteSpace(parts[3]) && !string.IsNullOrWhiteSpace(parts[4])) && (string.IsNullOrWhiteSpace(parts[0]) && string.IsNullOrWhiteSpace(parts[1]) && string.IsNullOrWhiteSpace(parts[2])))
            {
                searchQuery = "select * from Rechnungen WHERE sum BETWEEN '" + parts[3] + "' and '" + parts[4] + "'";
            }
            //name und preis gesetzt, rest nicht, rechnung nach name und preis suchen
            else if ((!string.IsNullOrWhiteSpace(parts[3]) && !string.IsNullOrWhiteSpace(parts[4]) && !string.IsNullOrWhiteSpace(parts[0])) && (string.IsNullOrWhiteSpace(parts[1]) && string.IsNullOrWhiteSpace(parts[2])))
            {
                searchQuery = "select * from Rechnungen WHERE fk_kontakt in (Select id from Kontakte Where Vorname like '%" + parts[0] + "%' OR Nachname like '%" + parts[0] + "%' OR Firmenname like '%" + parts[0] + "%') AND sum BETWEEN '" + parts[3] + "' and '" + parts[4] + "'";
            }
            //name und datum gesetzt, rest nicht, rechnung nach name und datum suchen
            else if ((!string.IsNullOrWhiteSpace(parts[1]) && !string.IsNullOrWhiteSpace(parts[2]) && !string.IsNullOrWhiteSpace(parts[0])) && (string.IsNullOrWhiteSpace(parts[3]) && string.IsNullOrWhiteSpace(parts[4])))
            {
                searchQuery = "select * from Rechnungen WHERE sum BETWEEN '" + parts[3] + "' and '" + parts[4] + "' AND datum BETWEEN '" + parts[1] + "' and '" + parts[2] + "'";
            }
            //preis und datum gesetzt, rest nicht, rechnung nach preis und datum suchen
            else if ((!string.IsNullOrWhiteSpace(parts[1]) && !string.IsNullOrWhiteSpace(parts[2]) && !string.IsNullOrWhiteSpace(parts[3]) && (!string.IsNullOrWhiteSpace(parts[4])) && string.IsNullOrWhiteSpace(parts[0])))
            {
                searchQuery = "select * from Rechnungen WHERE fk_kontakt in (Select id from Kontakte Where Vorname like '%" + parts[0] + "%' OR Nachname like '%" + parts[0] + "%' OR Firmenname like '%" + parts[0] + "%') AND datum BETWEEN '" + parts[1] + "' and '" + parts[2] + "'";
            }
            //alles gesetzt
            else if (!string.IsNullOrWhiteSpace(parts[0]) && !string.IsNullOrWhiteSpace(parts[1]) && !string.IsNullOrWhiteSpace(parts[2]) && !string.IsNullOrWhiteSpace(parts[3]) && !string.IsNullOrWhiteSpace(parts[4]))
            {
                searchQuery = "select * from Rechnungen WHERE fk_kontakt in (Select id from Kontakte Where Vorname like '%" + parts[0] + "%' OR Nachname like '%" + parts[0] + "%' OR Firmenname like '%" + parts[0] + "%') AND (datum BETWEEN '" + parts[1] + "' and '" + parts[2] + "') AND (sum BETWEEN '" + parts[3] + "' and '" + parts[4] + "')";
            }
            //nix gesetzt, einfach alle rechnungen suchen
            else
            {
                searchQuery = "select * from Rechnungen";
            }
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
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

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

                        resultInvoiceLine.Menge = Convert.ToString(reader["menge"]);
                        resultInvoiceLine.Stkpreis = Convert.ToString(reader["stkpreis"]);
                        resultInvoiceLine.UST = Convert.ToString(reader["ust"]);
                        resultInvoiceLine.FK_Rechnung = Convert.ToString(reader["fk_rechnung"]); ;

                        invoiceLines.Add(resultInvoiceLine);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                con.Close();
            }

            return invoiceLines;
        }

        public void realInsertContact(string searchstring)
        {
            InsertContact(searchstring);
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

            string plz;
            string uid;
            string fk;

            if (string.IsNullOrWhiteSpace(resultContact.PLZ))
                plz = "";
            else
                plz = resultContact.PLZ;

            if (string.IsNullOrWhiteSpace(resultContact.UID))
                uid = "";
            else
                uid =resultContact.UID;

            if (string.IsNullOrWhiteSpace(resultContact.FK_Kontakt))
                fk = "";
            else
                fk = resultContact.FK_Kontakt;

            string searchQuery = @"INSERT INTO MicroERP.dbo.Kontakte
            (titel, vorname, nachname, suffix, geburtsdatum, firmenname, uid, strasse, ort, plz, fk_kontakt) VALUES
            ('" + resultContact.Titel + "', '" + resultContact.Vorname + "', '" + resultContact.Nachname + "', '" + resultContact.Suffix + "', '" + resultContact.Geburtsdatum + "', '" + resultContact.Firmenname + "', '" + uid + "', '" + resultContact.Strasse + "', '" + resultContact.Ort + "', '" + plz + "', '" + fk + "')";


            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand(searchQuery, con);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                con.Close();
            }
        }

        public void realUpdateContact(string searchstring)
        {
            UpdateContact(searchstring);
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

            string plz;
            string uid;
            string fk;

            if (string.IsNullOrWhiteSpace(resultContact.PLZ))
                plz = "";
            else
                plz = resultContact.PLZ;

            if (string.IsNullOrWhiteSpace(resultContact.UID))
                uid = "";
            else
                uid = resultContact.UID;

            if (string.IsNullOrWhiteSpace(resultContact.FK_Kontakt))
                fk = "";
            else
                fk = resultContact.FK_Kontakt;

            string searchQuery = @"UPDATE MicroERP.dbo.Kontakte SET titel = '" + resultContact.Titel + @"', 
            vorname = '" + resultContact.Vorname + @"', 
            nachname = '" + resultContact.Nachname + @"', 
            suffix = '" + resultContact.Suffix + @"', 
            geburtsdatum = '" + resultContact.Geburtsdatum + @"', 
            firmenname = '" + resultContact.Firmenname + @"', 
            uid = '" + uid + @"', 
            strasse = '" + resultContact.Strasse + @"', 
            ort = '" + resultContact.Ort + @"', 
            plz = '" + plz + @"', 
            fk_kontakt = '" + fk + @"'
            WHERE id = '" + resultContact.ID + "'";


            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand(searchQuery, con);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                con.Close();
            }
        }

        public void realInsertInvoice(string searchstring)
        {
            InsertInvoice(searchstring);
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

            string searchQuery = @"INSERT INTO MicroERP.dbo.Rechnungen (datum, faellig, kommentar, nachricht, fk_kontakt, sum)
                                    VALUES (@erstellung, @faellig, @kommentar, @nachricht, @fk, @summe)";


            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand(searchQuery, con);

                    cmd.Parameters.Add("@erstellung", SqlDbType.DateTime2, 50).Value = resultInvoice.ErstellungsDatum;
                    cmd.Parameters.Add("@faellig", SqlDbType.DateTime2, 50).Value = resultInvoice.FaelligkeitsDatum;
                    cmd.Parameters.Add("@kommentar", SqlDbType.NVarChar, 50).Value = resultInvoice.Kommentar;
                    cmd.Parameters.Add("@nachricht", SqlDbType.NVarChar, 50).Value = resultInvoice.Nachricht;
                    cmd.Parameters.Add("@fk", SqlDbType.Int, 38).Value = resultInvoice.FK_Kontakt;
                    cmd.Parameters.Add("@summe", SqlDbType.Decimal, 38).Value = resultInvoice.Summe;
                    cmd.Parameters["@summe"].Precision = 38;
                    cmd.Parameters["@summe"].Scale = 2;

                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                con.Close();
            }

            using (SqlConnection con2 = new SqlConnection(_ConnectionString))
            {
                con2.Open();

                try
                {
                    foreach (InvoiceLineObject obj in resultInvoice.InvoiceLines)
                    {

                        string insertString = @"INSERT INTO MicroERP.dbo.Rechnungszeile (menge, stkpreis, fk_rechnung, ust) VALUES
                                                (@menge, @stkpreis, @fk, @ust)";

                        int max = 0;

                        using (SqlConnection con3 = new SqlConnection(_ConnectionString))
                        {
                            con3.Open();
                            
                            SqlCommand cmdmax = new SqlCommand("(Select Max(id) as maxid from Rechnungen)", con3);

                            SqlDataReader reader = cmdmax.ExecuteReader();

                            while (reader.Read())
                            {
                                max = Convert.ToInt32(reader["maxid"]);
                            }

                            con3.Close();
                        }

                        SqlCommand cmd2 = new SqlCommand(insertString, con2);

                        cmd2.Parameters.Add("@menge", SqlDbType.Int, 38).Value = obj.Menge;
                        cmd2.Parameters.Add("@stkpreis", SqlDbType.Decimal, 38).Value = obj.Stkpreis;
                        cmd2.Parameters["@stkpreis"].Precision = 38;
                        cmd2.Parameters["@stkpreis"].Scale = 2;
                        cmd2.Parameters.Add("@fk", SqlDbType.Int, 38).Value = max;
                        cmd2.Parameters.Add("@ust", SqlDbType.Int, 38).Value = obj.UST;

                        cmd2.Prepare();

                        cmd2.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                con2.Close();
            }
        }
    }
}
