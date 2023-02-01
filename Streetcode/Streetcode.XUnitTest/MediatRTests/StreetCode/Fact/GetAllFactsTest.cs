using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Mapping.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.MediatRTest.StreetCode.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Fact;

public class GetAllFactsTest
{
    private Mock<IRepositoryWrapper> _repository;
    private IMapper _mockMapper;

    public GetAllFactsTest()
    {
        _repository = MockFactRepository.GetRepositoryWrapper();

        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<FactProfile>();
        });

        _mockMapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task GetAllFacts_ShouldReturnSuccessfullyType()
    {
        var handler = new GetAllFactsHandler(_repository.Object, _mockMapper);

        var result = await handler.Handle(new GetAllFactsQuery(), CancellationToken.None);

        Assert.IsType<List<FactDTO>>(result.Value);
    }
}
