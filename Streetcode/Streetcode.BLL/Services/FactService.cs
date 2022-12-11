
using Repositories.Interfaces;
using Repositories.Realizations;
using Services.Interfaces;
using StreetCode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services
{
    public class FactService : IFactService 
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public FactService(IRepositoryWrapper repositoryWrapper) 
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public string GetFactsByStreetcode() 
        {
            return _repositoryWrapper.FactRepository.GetFactsByStreetcode();
        }

    }
}