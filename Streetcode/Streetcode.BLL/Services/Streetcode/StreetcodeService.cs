
using Repositories.Realizations;
using Streetcode.BLL.Interfaces.Streetcode;
using StreetCode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Streetcode.BLL.Services.Streetcode;

public class StreetcodeService : IStreetcodeService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public StreetcodeService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public string GetStreetcodeByNameAsync()
    {
        return _repositoryWrapper.StreetcodeRepository.GetStreetcodeByNameAsync();
    }

    public void GetStreetcodesByTagAsync()
    {
        // TODO implement here
    }

    public void GetByCodeAsync()
    {
        // TODO implement here
    }

    public void GetTagsByStreecodeIdAsync()
    {
        // TODO implement here
    }

    public void GetEventsAsync()
    {
        // TODO implement here
    }

    public void GetPersonsAsync()
    {
        // TODO implement here
    }

}