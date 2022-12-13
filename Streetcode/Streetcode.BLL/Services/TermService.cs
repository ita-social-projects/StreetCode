
using Repositories.Realizations;
using Services.Interfaces;
using StreetCode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services;

public class TermService : ITermService 
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public TermService(IRepositoryWrapper repositoryWrapper) 
    {
        _repositoryWrapper= repositoryWrapper;
    }


    public string GetTermAsync()
    {
        return _repositoryWrapper.TermRepository.GetTermAsync();
    }
}