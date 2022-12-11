
using Repositories.Realizations;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services
{
    public class StreetcodeService : IStreetcodeService
    {

        public StreetcodeService() 
        {
        }

        private RepositoryWrapper RepositoryWrapper;

        public string GetStreetcodeByNameAsync() 
        {
            return "GetStreetcodeByNameAsync";
        }

        public void GetStreetcodesByTagAsync() 
        {
            // TODO implement here
        }

        public void GetByCodeAsync() 
        {
            // TODO implement here
        }

        public void GetTagsByStreecodeIdAsync() 
        {
            // TODO implement here
        }

        public void GetEventsAsync() 
        {
            // TODO implement here
        }

        public void GetPersonsAsync() 
        {
            // TODO implement here
        }

    }
}