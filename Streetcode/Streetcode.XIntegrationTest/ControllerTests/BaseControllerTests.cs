using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Streetcode.DAL.Persistence;
using System.Data.Common;
using Xunit;

using Streetcode.WebApi.Extensions;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.TestHost;
using Streetcode.XIntegrationTest.ControllerTests.Utils;

namespace Streetcode.XIntegrationTest.ControllerTests
{
    
    public class BaseControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        protected HttpClient _client;

        protected string baseUrl = "https://localhost:5001/";
        protected CustomWebApplicationFactory<Program> _factory;

        public BaseControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            
            _client = factory
                .CreateClient() ;
            
            this._factory = factory;

        }

       

    }
}
