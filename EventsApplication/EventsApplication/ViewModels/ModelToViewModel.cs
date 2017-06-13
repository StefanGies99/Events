﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventsApplication.App_DAL;
using EventsApplication.App_DAL.Interfaces;
using EventsApplication.Controllers;
using EventsApplication.Controllers.Repositorys;
using EventsApplication.Models;

namespace EventsApplication.ViewModels
{
    public static class ModelToViewModel
    {
        public static AccountRepository accountRepository = new AccountRepository(new AccountContext());
        public static PolsbandjeRepository polsbandjeRepository = new PolsbandjeRepository(new PolsbandjeContext());
        public static ReserveringRepository reserveringRepository = new ReserveringRepository(new ReserveringContext());

        public static EventViewModel EventToEventViewModel(Event evenement)
        {
            return new EventViewModel
            {
                Naam = evenement.Naam,
                DatumStart = evenement.Datumstart,
                DatumEind = evenement.Datumeind,
                MaxBezoekers = evenement.Maxbezoekers,
                Locatie = evenement.Locatie
            };
        }

//Make a "Account" view Model
        public static AccountViewModel ConvertAccounttoViewModel(Account account)
        {
            // convert an result to a viewmodel
            AccountViewModel accountViewModel = new AccountViewModel();

            accountViewModel.Account = accountRepository.GetById(account.Id);
            accountViewModel.Polsbandje = polsbandjeRepository.GetByAccountId(account);
            int id = accountViewModel.Polsbandje.ReserveringsId;
            accountViewModel.Reservering = reserveringRepository.GetById(id);
            return accountViewModel;
        }

        public static List<AccountViewModel> ConvertAccounttoViewModel(List<Account> accounts)
        {
            // convert a list of results to viewmodel results
            List<AccountViewModel> berichtenViewModelList = new List<AccountViewModel>();

            foreach (Account account in accounts)
            {
                berichtenViewModelList.Add(ConvertAccounttoViewModel(account));
            }

            return berichtenViewModelList;
        }

    }
}