
using Repositories.Realizations;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services
{
    public class PartnersService : IPartnersService 
    {
        public PartnersService() 
        {
        }

        private RepositoryWrapper RepositoryWrapper;

        public string GetSponsorsAsync() 
        {
            return "GetSponsorsAsync";
            // TODO implement here
        }

    }
}