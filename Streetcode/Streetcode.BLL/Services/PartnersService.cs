
using Repositories.Realizations;
using Services.Interfaces;
using StreetCode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services;

public class PartnersService : IPartnersService 
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public PartnersService(IRepositoryWrapper repositoryWrapper) 
    {
        _repositoryWrapper= repositoryWrapper;
    }

    public string GetSponsorsAsync() 
    {
        return _repositoryWrapper.PartnersRepository.GetSponsorsAsync();
        // TODO implement here
    }

}