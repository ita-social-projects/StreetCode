
using Repositories.Realizations;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services{
    public class ToponymService : IToponymService
    {

        public ToponymService() 
        {
        }

        private RepositoryWrapper RepositoryWrapper;

        public string GetToponymByNameAsync() 
        {
            // TODO implement here
            return "GetToponymByNameAsync";
        }

        public void GetStreetcodesByToponymAsync() 
        {
            // TODO implement here
        }

    }
}