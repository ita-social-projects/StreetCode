
using Repositories.Realizations;
using Services.Interfaces;
using StreetCode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services;

public class TextService : ITextService 
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public TextService(IRepositoryWrapper repositoryWrapper) 
    {
        _repositoryWrapper= repositoryWrapper;
    }

    public void GetNext() 
    {
        // TODO implement here
    }
    public string GetTextAsync()
    {
        return _repositoryWrapper.TextRepository.GetTextAsync();
    }

}