
using Repositories.Realizations;
using Services.Interfaces;
using StreetCode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services;

public class TransactLinksService : ITransactLinksService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public TransactLinksService(IRepositoryWrapper repositoryWrapper) 
    {
        _repositoryWrapper= repositoryWrapper;
    }

    private RepositoryWrapper RepositoryWrapper;

    public string GetTransactLinkAsync()
    {
        return _repositoryWrapper.TransactLinksRepository.GetTransactLinkAsync();
        // TODO implement here
    }
}