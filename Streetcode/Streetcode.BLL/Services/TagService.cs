
using Repositories.Realizations;
using Services.Interfaces;
using StreetCode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services;

public class TagService : ITagService 
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public TagService(IRepositoryWrapper repositoryWrapper) 
    {
        _repositoryWrapper= repositoryWrapper;
    }

    public string GetTagByNameAsync()
    {
        return _repositoryWrapper.TagRepository.GetTagByNameAsync();
    }
}