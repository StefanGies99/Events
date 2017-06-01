﻿using EventsApplication.App_DAL.Interfaces;
using EventsApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EventsApplication.App_DAL
{
    public class LocatieContext : ILocatieContext
    {
        public bool Insert(Locatie locatie)
        {
            SqlConnection conn = Connection.SQLconnection;
            try
            {
                string query = "INSERT INTO dbo.LOCATIE(naam, straat, nr, postcode, plaats) VALUES(@naam, @straat, @nr, @postcode, @plaats)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@naam", locatie.Naam);
                cmd.Parameters.AddWithValue("@straat", locatie.Straat);
                cmd.Parameters.AddWithValue("@nr", locatie.Nr);
                cmd.Parameters.AddWithValue("@postcode", locatie.Postcode);
                cmd.Parameters.AddWithValue("@plaats", locatie.Plaats);

                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch { return false; }
        }

        public List<Locatie> GetAll()
        {
            SqlConnection conn = Connection.SQLconnection;
            List<Locatie> locaties = new List<Locatie>();
            try
            {
                string query = "SELECT * FROM dbo.locatie";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string naam = reader["naam"].ToString();
                    string straat = reader["straat"].ToString();
                    int nr = Convert.ToInt32(reader["nr"].ToString());
                    int id = Convert.ToInt32(reader["ID"].ToString());
                    string postcode = reader["postcode"].ToString();
                    string plaats = reader["plaats"].ToString();

                    locaties.Add(new Locatie(naam, straat, nr, id, postcode, plaats));
                }
                conn.Close();

            }
            catch { }
            return locaties;
        }

        public Locatie GetByEvenement(Evenement evenement)
        {
            try
            {
                Locatie locatieReturn = null;
                using (SqlConnection connection = Connection.SQLconnection)
                {
                    string query =
                        "SELECT * FROM dbo.LOCATIE INNER JOIN [EVENT] ON [event].locatie_id = LOCATIE.ID WHERE [EVENT].ID = @id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", evenement.Id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            string naam = reader["naam"].ToString();
                            string straat = reader["straat"].ToString();
                            int nr = Convert.ToInt32(reader["nr"].ToString());
                            int id = Convert.ToInt32(reader["ID"].ToString());
                            string postcode = reader["postcode"].ToString();
                            string plaats = reader["plaats"].ToString();
                            locatieReturn = new Locatie(naam, straat, nr, id, postcode, plaats);
                        }
                        connection.Close();
                    }
                }
                return locatieReturn;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Delete(Locatie locatie)
        {
            SqlConnection conn = Connection.SQLconnection;
            try
            {
                string query = "DELETE FROM dbo.LOCATIE WHERE dbo.LOCATIE.ID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", locatie.Id);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch
            {
                conn.Close();
                return false;
            }
        }

        public bool Update(Locatie locatie)
        {
            SqlConnection conn = Connection.SQLconnection;
            try
            {
                string query = "UPDATE LOCATIE SET naam = @naam, straat = @straat, nr = @nr, postcode = @postcode, plaats = @plaats WHERE ID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", locatie.Id);
                cmd.Parameters.AddWithValue("@naam", locatie.Naam);
                cmd.Parameters.AddWithValue("@straat", locatie.Straat);
                cmd.Parameters.AddWithValue("@nr", locatie.Nr);
                cmd.Parameters.AddWithValue("@postcode", locatie.Postcode);
                cmd.Parameters.AddWithValue("@plaats", locatie.Plaats);

                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch { return false; }
        }
    }
}