using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByFilter;
using Streetcode.DAL.Repositories.Realizations.Base;
using BenchmarkDotNet.Attributes;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.DTO.AdditionalContent.Filter;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Validators;

namespace YourNamespace
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var config = new ManualConfig()
              .WithOptions(ConfigOptions.DisableOptimizationsValidator)
              .AddValidator(JitOptimizationsValidator.DontFailOnError)
              .AddLogger(ConsoleLogger.Default)
              .AddColumnProvider(DefaultColumnProviders.Instance);

            BenchmarkRunner.Run<GetStreetcodeByFilterHandlerBenchmark>(config);
        }
    }
    
    [MemoryDiagnoser]
    public class GetStreetcodeByFilterHandlerBenchmark
    {
        private GetStreetcodeByFilterHandler _handler;
        private GetStreetcodeByFilterQuery _query;

        [GlobalSetup]
        public void GlobalSetup()
        {
            // set up dependencies and configuration
            var options = new DbContextOptionsBuilder<StreetcodeDbContext>()
                .UseInMemoryDatabase(databaseName: "StreetcodeDb")
                .Options;
            var dbContext = new StreetcodeDbContext(options);
            var repositoryWrapper = new RepositoryWrapper(dbContext);
            _handler = new GetStreetcodeByFilterHandler(repositoryWrapper);

            // set up test data
            _query = new GetStreetcodeByFilterQuery(new StreetcodeFilterRequestDTO
            {
                SearchQuery = "та"
            });
        }

        [Benchmark]
        public async Task<List<StreetcodeFilterResultDTO>> Handle()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            if (result.IsSuccess)
            {
                return result.Value;
            }
            // handle error case
            return null;
        }
    }

    public class BenchConfig
    {
      public static readonly ManualConfig Instance = new ManualConfig()
          .WithOptions(ConfigOptions.DisableOptimizationsValidator);
    }
}
