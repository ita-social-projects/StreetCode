
using Repositories.Realizations;
using Streetcode.BLL.Interfaces.AdditionalContent;
using StreetCode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Streetcode.BLL.Services.AdditionalContent;

public class TagService : ITagService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public TagService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public string GetTagByNameAsync()
    {
        return _repositoryWrapper.TagRepository.GetTagByNameAsync();
    }
}