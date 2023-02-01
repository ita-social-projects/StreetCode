using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Mapping.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetByStreetcodeId;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.MediatRTest.StreetCode.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTest.StreetCode.Fact;

public class GetByStreetcodeIdTest
{
    private Mock<IRepositoryWrapper> _repository;
    private IMapper _mockMapper;

    public GetByStreetcodeIdTest()
    {
        _repository = MockFactRepository.GetRepositoryWrapper();

        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<FactProfile>();
        });

        _mockMapper = mapperConfig.CreateMapper();
    }

    [Theory]
    [InlineData(1)]
    public async void GetFactById_ShouldReturnSuccessfullyExistingId(int id)
    {
        var handler = new GetByStreetcodeIdHandler(_repository.Object, _mockMapper);

        var result = await handler.Handle(new GetFactByStreetcodeIdQuery(id), CancellationToken.None);

        Assert.True(result.Value.Any(x => x.Id == id));
    }

    [Theory]
    [InlineData(3)]
    public async Task GetFactById_ShouldReturnSuccessfullyNotExistingId(int id)
    {
        var handler = new GetByStreetcodeIdHandler(_repository.Object, _mockMapper);

        var result = await handler.Handle(new GetFactByStreetcodeIdQuery(id), CancellationToken.None);

        Assert.False(result.Value.Any(x => x.Id == id));
    }

    [Theory]
    [InlineData(2)]
    public async void GetFactById_ShouldReturnSuccessfullyType(int id)
    {
        var handler = new GetByStreetcodeIdHandler(_repository.Object, _mockMapper);

        var result = await handler.Handle(new GetFactByStreetcodeIdQuery(id), CancellationToken.None);

        Assert.IsType<List<FactDTO>>(result.ValueOrDefault);
    }
}