
using Repositories.Realizations;
using Streetcode.BLL.Interfaces.Streetcode.TextContent;
using StreetCode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Streetcode.BLL.Services.Streetcode.TextContent;

public class TermService : ITermService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public TermService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }


    public string GetTermAsync()
    {
        return _repositoryWrapper.TermRepository.GetTermAsync();
    }
}