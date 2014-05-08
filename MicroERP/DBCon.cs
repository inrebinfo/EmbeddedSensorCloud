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
    public class DBCon
    {
        private SqlConnection _SQLCon;
        private string _ConnectionString;

        public DBCon()
        {
            //_SQLCon = new SqlConnection(@"Data Source=.\SqlExpress;Initial Catalog=MicroERP;Integrated Security=true;");
            _ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=MicroERP;Integrated Security=true;";
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
                catch(Exception ex)
                { }

                con.Close();
            }

            return contacts;
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
    }
}
