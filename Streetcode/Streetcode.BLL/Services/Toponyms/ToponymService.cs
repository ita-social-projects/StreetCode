
using Repositories.Realizations;
using Streetcode.BLL.Interfaces.Toponyms;
using StreetCode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Streetcode.BLL.Services.Toponyms;

public class ToponymService : IToponymService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public ToponymService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    private RepositoryWrapper RepositoryWrapper;

    public string GetToponymByNameAsync()
    {
        // TODO implement here
        return _repositoryWrapper.ToponymRepository.GetToponymByNameAsync();
    }

    public void GetStreetcodesByToponymAsync()
    {
        // TODO implement here
    }

}