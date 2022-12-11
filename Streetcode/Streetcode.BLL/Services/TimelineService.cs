
using Repositories.Realizations;
using Services.Interfaces;
using StreetCode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services;

public class TimelineService : ITimelineService 
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public TimelineService(IRepositoryWrapper repositoryWrapper) 
    {
        _repositoryWrapper= repositoryWrapper;
    }

  
    public string GetTimelineItemsAsync() 
    {
        // TODO implement here
        return _repositoryWrapper.TimelineRepository.GetTimeItemsByStreetcode();
    }

}