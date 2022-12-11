
using Repositories.Realizations;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services
{
    public class TextService : ITextService 
    {

        public TextService() 
        {
        }

        private RepositoryWrapper RepositoryWrapper;

        public void GetNext() 
        {
            // TODO implement here
        }
        public string GetTextAsync()
        {
            return "GetTextAsync";
        }

    }
}