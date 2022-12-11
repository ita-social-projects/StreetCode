
using Repositories.Realizations;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services
{
    public class TermService : ITermService 
    {

        public TermService() 
        {
        }

        private RepositoryWrapper RepositoryWrapper;
        public string GetTermAsync()
        {
            return "GetTermAsync";
        }
    }
}