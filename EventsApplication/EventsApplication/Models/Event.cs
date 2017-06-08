﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventsApplication.App_DAL;
using EventsApplication.Controllers.Repositorys;

namespace EventsApplication.Models
{
    public class Event
    {
        private int ID;
        private string naam;
        private DateTime datumstart;
        private DateTime datumeind;
        private int maxbezoekers;
        private Locatie locatie;

        public Event(int iD, string naam, DateTime datumstart, DateTime datumeind, int maxbezoekers, int locatieid)
        {
            ID = iD;
            this.naam = naam;
            this.datumstart = datumstart;
            this.datumeind = datumeind;
            this.maxbezoekers = maxbezoekers;
            LocatieRepository lrepo = new LocatieRepository(new LocatieContext());
            this.locatie = lrepo.GetByEvenement(this);
        }

        public Event(string naam, DateTime datumstart, DateTime datumeind, int maxbezoekers)
        {
            this.naam = naam;
            this.datumstart = datumstart;
            this.datumeind = datumeind;
            this.maxbezoekers = maxbezoekers;
        }

        public int ID1
        {
            get {return ID;}
            set {ID = value;}
        }

        public string Naam
        {
            get{ return naam;}

            set{ naam = value;}
        }

        public DateTime Datumstart
        {
            get { return datumstart;}
            set { datumstart = value;}
        }

        public DateTime Datumeind
        {
            get {return datumeind;}

            set {datumeind = value;}
        }

        public int Maxbezoekers
        {
            get { return maxbezoekers;}
            set { maxbezoekers = value;}
        }

        public Locatie Locatie
        {
            get { return locatie; }
            set { locatie = value; }
        }


    }
}