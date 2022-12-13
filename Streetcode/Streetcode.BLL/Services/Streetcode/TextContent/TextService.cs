
using Repositories.Realizations;
using Streetcode.BLL.Interfaces.Streetcode.TextContent;
using StreetCode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Streetcode.BLL.Services.Streetcode.TextContent;

public class TextService : ITextService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public TextService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
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