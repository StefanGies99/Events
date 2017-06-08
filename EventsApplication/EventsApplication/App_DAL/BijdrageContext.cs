﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using EventsApplication.Controllers;
using EventsApplication.Models;
using EventsApplication.ViewModels;

namespace EventsApplication.App_DAL
{
    public class BijdrageContext : IBijdrageContext
    {
        //public Bijdrage GetById(int id)
        //{

        //}

        //public bool InsertBericht(Bijdrage bijdrage)
        //{

        //}

        //public bool Delete(int id)
        //{

        //}

        public Bijdrage GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Bijdrage> GetAllBijdrages()
        {
            List<Bijdrage> bijdrageList = new List<Bijdrage>();

            using (SqlConnection connection = Connection.SQLconnection)
            {
                string query =
                    "SELECT * FROM BIJDRAGE b " +
                    "LEFT JOIN CATEGORIE c on b.ID = c.bijdrage_id " +
                    "LEFT JOIN BESTAND be on b.ID = be.bijdrage_id " +
                    "LEFT JOIN BERICHT br on b.ID = br.bijdrage_id " +
                    "LEFT JOIN ACCOUNT a on b.account_id = a.ID " +
                    "LEFT JOIN ACCOUNT_BIJDRAGE ab on b.ID = ab.bijdrage_id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Bijdrage bijdrage = CreateBijdrageFromReader(reader);
                            bijdrageList.Add(bijdrage);
                        }
                    }
                }
            }
            return bijdrageList;
        }

        public List<Bijdrage> GetAllBijdragesByUserId(int userid)
        {
            List<Bijdrage> bijdrageList = new List<Bijdrage>();

            using (SqlConnection connection = Connection.SQLconnection)
            {
                string query =
                    "SELECT * FROM BIJDRAGE b " +
                    "LEFT JOIN CATEGORIE c on b.ID = c.bijdrage_id " +
                    "LEFT JOIN BESTAND be on b.ID = be.bijdrage_id " +
                    "LEFT JOIN BERICHT br on b.ID = br.bijdrage_id " +
                    "LEFT JOIN ACCOUNT acc on b.account_id = acc.ID " +
                    "LEFT JOIN ACCOUNT_BIJDRAGE ab on b.ID = ab.bijdrage_id WHERE acc.ID = @userid";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userid", userid);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Bijdrage bijdrage = CreateBijdrageFromReader(reader);
                            bijdrageList.Add(bijdrage);
                        }
                    }
                }
            }
            return bijdrageList;
        }

        public bool Insert(PostViewModel bericht)
        {
            try
            {
                using (SqlConnection connection = Connection.SQLconnection)
                {
                    string query = "INSERT INTO BERICHT (bijdrage_id, titel, inhoud) VALUES (@bijdrageID, @titel, @inhoud)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@accountID", bericht.account.Id);
                        command.Parameters.AddWithValue("@datum", bericht.bericht.Datum);
                        command.Parameters.AddWithValue("@soort", bericht.bericht.Soort);
                        command.Parameters.AddWithValue("@bijdrageID", bericht.bericht.Id);
                        command.Parameters.AddWithValue("@titel", bericht.bericht.Titel);
                        command.Parameters.AddWithValue("@inhoud", bericht.bericht.Inhoud);
                        command.Parameters.AddWithValue("@categorieID", bericht.categorie.Id);
                        command.Parameters.AddWithValue("@bestandsLocatieID", 5);
                        command.Parameters.AddWithValue("@grootte", bericht.account.Id);
                        command.Parameters.AddWithValue("@naam", bericht.account.Id);


                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private Bijdrage CreateBijdrageFromReader(SqlDataReader reader)
        {
            AccountRepository accountRepository = new AccountRepository(new AccountContext());
            
            
            if (reader["soort"].ToString() == "categorie")
            {
                Account account = accountRepository.GetById(Convert.ToInt32(reader["account_id"]));
                AccountBijdrage accountBijdrage;
                try
                {
                    accountBijdrage = new AccountBijdrage(Convert.ToInt32(reader["account_id"]), Convert.ToInt32(reader["bijdrage_id"]), Convert.ToInt32(reader["like"]), Convert.ToInt32("ongewenst"));
                }
                catch (Exception)
                {
                    accountBijdrage = new AccountBijdrage(0,0,0,0,0);
                }
                 return new Categorie(
                    Convert.ToInt32(reader["ID"]),
                    account,
                    Convert.ToDateTime(reader["datum"]),
                    Convert.ToString(reader["soort"]),
                    accountBijdrage,
                    Convert.ToInt32(reader["categorie_id"] != DBNull.Value ? Convert.ToInt32(reader["categorie_id"]) : 0),
                    Convert.ToString(reader["naam"])
                );
            }
            else if (reader["soort"].ToString() == "bestand")
            {
                {
                    Account account = accountRepository.GetById(Convert.ToInt32(reader["account_id"]));
                    AccountBijdrage accountBijdrage;
                    try
                    {
                        accountBijdrage = new AccountBijdrage(Convert.ToInt32(reader["account_id"]), Convert.ToInt32(reader["bijdrage_id"]), Convert.ToInt32(reader["like"]), Convert.ToInt32("ongewenst"));
                    }
                    catch (Exception)
                    {
                        accountBijdrage = new AccountBijdrage(0, 0, 0, 0, 0);
                    }
                    return new Bestand(
                        Convert.ToInt32(reader["ID"]),
                        account,
                        Convert.ToDateTime(reader["datum"]),
                        Convert.ToString(reader["soort"]),
                        accountBijdrage,
                        Convert.ToInt32(reader["categorie_id"] != DBNull.Value ? Convert.ToInt32(reader["categorie_id"]) : 0),
                        Convert.ToString(reader["bestandslocatie"] != DBNull.Value ? Convert.ToString(reader["bestandslocatie"]) : ""),
                        Convert.ToInt32(reader["grootte"] != DBNull.Value ? Convert.ToInt32(reader["grootte"]) : 0)
                    );
                }
            }

            else if (reader["soort"].ToString() == "bericht")
            {
                {
                    Account account = accountRepository.GetById(Convert.ToInt32(reader["account_id"]));
                    AccountBijdrage accountBijdrage;
                    try
                    {
                        accountBijdrage = new AccountBijdrage(Convert.ToInt32(reader["account_id"]), Convert.ToInt32(reader["bijdrage_id"]), Convert.ToInt32(reader["like"]), Convert.ToInt32("ongewenst"));
                    }
                    catch (Exception)
                    {
                        accountBijdrage = new AccountBijdrage(0, 0, 0, 0, 0);
                    }
                    return new Bericht(
                        Convert.ToInt32(reader["ID"]),
                        account,
                        Convert.ToDateTime(reader["datum"]),
                        Convert.ToString(reader["soort"]),
                        accountBijdrage,
                        Convert.ToString(reader["titel"]),
                        Convert.ToString(reader["inhoud"])
                    );
                }
            }
            else
            {
                return null;
            }
        }

    }
}