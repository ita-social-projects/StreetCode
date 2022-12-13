
using Repositories.Realizations;
using Streetcode.BLL.Interfaces.AdditionalContent;
using StreetCode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Streetcode.BLL.Services.AdditionalContent;

public class SubtitleService : ISubtitleService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public SubtitleService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public string GetSubtitlesByStreetcode()
    {
        return _repositoryWrapper.SubtitleRepository.GetSubtitlesByStreetcode();
        // TODO implement here
    }

}