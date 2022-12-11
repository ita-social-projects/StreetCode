
using Repositories.Realizations;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services
{
    public class TimelineService : ITimelineService 
    {

        public TimelineService() 
        {
        }

        private RepositoryWrapper RepositoryWrapper;

        public string GetTimelineItemsAsync() 
        {
            // TODO implement here
            return "GetTimeItemsAsync";
        }

    }
}