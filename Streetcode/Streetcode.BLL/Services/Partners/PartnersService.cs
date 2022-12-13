
using Repositories.Realizations;
using Streetcode.BLL.Interfaces.Partners;
using StreetCode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Streetcode.BLL.Services.Partners;

public class PartnersService : IPartnersService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public PartnersService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public string GetSponsorsAsync()
    {
        return _repositoryWrapper.PartnersRepository.GetSponsorsAsync();
        // TODO implement here
    }

}