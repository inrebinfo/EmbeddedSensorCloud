using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Net;

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

            //SELECT Titel, vorname, nachname, suffix, geburtsdatum, firmenname, uid, strasse, plz, ort FROM MicroERP.dbo.Kontakte WHERE Vorname LIKE '%gruber%' OR Nachname LIKE '%gruber%' OR Firmenname LIKE '%gruber%'
            string searchQuery = @"SELECT titel, vorname, nachname, suffix, geburtsdatum, firmenname, uid, strasse, plz, ort FROM MicroERP.dbo.Kontakte WHERE Vorname LIKE '%" + encSearchString + "%' OR Nachname LIKE '%" + encSearchString + "%' OR Firmenname LIKE '%" + encSearchString + "%'";
            //cmd.Parameters.Add(new SqlParameter("FirstName", FirstName));

            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                /*if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }*/
                
                con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand(searchQuery, con);
                    //cmd.Parameters.Add(new SqlParameter("SearchString", searchstring));
                    

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ContactObject resultContact = new ContactObject();
                        resultContact.Titel = reader["titel"].ToString();
                        resultContact.Vorname = reader["vorname"].ToString();
                        resultContact.Nachname = reader["nachname"].ToString();
                        resultContact.Suffix = reader["suffix"].ToString();
                        resultContact.Geburtsdatum = reader["geburtsdatum"].ToString();
                        resultContact.Firmenname = reader["firmenname"].ToString();
                        resultContact.UID = reader["uid"].ToString();
                        resultContact.Strasse = reader["strasse"].ToString();
                        resultContact.PLZ = reader["plz"].ToString();
                        resultContact.Ort = reader["ort"].ToString();
                        
                        contacts.Add(resultContact);
                    }
                }
                catch { }
                /*catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }*/

                con.Close();
            }

            /*resultContact.Titel = "Mag";
            resultContact.Vorname = "Johannes";
            resultContact.Nachname = "Huber";*/
            

            return contacts;
        }
    }
}
