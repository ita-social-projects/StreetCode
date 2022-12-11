
using Repositories.Realizations;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services{
    public class TransactLinksService : ITransactLinksService
    {

        public TransactLinksService() 
        {
        }

        private RepositoryWrapper RepositoryWrapper;

        public string GetTransactLinkAsync()
        {
            return "GetTransactLinkAsync";
            // TODO implement here
        }
    }
}