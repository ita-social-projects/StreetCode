
using Repositories.Realizations;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services
{
    public class TagService : ITagService 
    {
        public TagService() 
        {
        }

        private RepositoryWrapper RepositoryWrapper;
        public string GetTagByNameAsync()
        {
            return "GetTagByNameAsync";
        }
    }
}