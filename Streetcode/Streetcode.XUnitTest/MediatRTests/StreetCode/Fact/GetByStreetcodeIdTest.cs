using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Mapping.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetByStreetcodeId;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Fact;

public class GetByStreetcodeIdTest
{
    private Mock<IRepositoryWrapper> _mockRepository;
    private Mock<IMapper> _mockMapper;

    public GetByStreetcodeIdTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
    }

    [Theory]
    [InlineData(1)]
    public async void GetFactById_ShouldReturnSuccessfullyExistingId(int streetCodeId)
    {
        _mockRepository.Setup(x => x.FactRepository
              .GetAllAsync(
                  It.IsAny<Expression<Func<DAL.Entities.Streetcode.TextContent.Fact, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Streetcode.TextContent.Fact>,
              IIncludableQueryable<DAL.Entities.Streetcode.TextContent.Fact, object>>>()))
              .ReturnsAsync(GetListFacts());

        _mockMapper
            .Setup(x => x
            .Map<IEnumerable<FactDTO>>(It.IsAny<IEnumerable<DAL.Entities.Streetcode.TextContent.Fact>>()))
            .Returns(GetListFactDTO());

        var handler = new GetByStreetcodeIdHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetFactByStreetcodeIdQuery(streetCodeId), CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Theory]
    [InlineData(2)]
    public async void GetFactById_ShouldReturnSuccessfullyType(int streetCodeId)
    {
        _mockRepository.Setup(x => x.FactRepository
              .GetAllAsync(
                  It.IsAny<Expression<Func<DAL.Entities.Streetcode.TextContent.Fact, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Streetcode.TextContent.Fact>,
              IIncludableQueryable<DAL.Entities.Streetcode.TextContent.Fact, object>>>()))
              .ReturnsAsync(GetListFacts());

        _mockMapper
            .Setup(x => x
            .Map<IEnumerable<FactDTO>>(It.IsAny<IEnumerable<DAL.Entities.Streetcode.TextContent.Fact>>()))
            .Returns(GetListFactDTO());

        var handler = new GetByStreetcodeIdHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetFactByStreetcodeIdQuery(streetCodeId), CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.IsType<List<FactDTO>>(result.ValueOrDefault);
    }

    private static IQueryable<DAL.Entities.Streetcode.TextContent.Fact> GetListFacts()
    {
        var facts = new List<DAL.Entities.Streetcode.TextContent.Fact>
        {
            new DAL.Entities.Streetcode.TextContent.Fact
            {
                Id = 1,
                Title = "Викуп з кріпацтва",
                ImageId = null,
                Streetcodes = GetStreetcodes(),
                FactContent = "Навесні 1838-го Карл Брюллов..."
            },
        };

        return facts.AsQueryable();
    }

    private static List<StreetcodeContent> GetStreetcodes()
    {
        var streetCodes = new List<StreetcodeContent>
        {
            new StreetcodeContent
            {
                Id = 1,
                Index = 1,
                Teaser = "TestTeser",
                ViewCount = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EventStartOrPersonBirthDate = DateTime.Now,
                EventEndOrPersonDeathDate = DateTime.Now,
                Text = null,
                Audio = null,
                TransactionLink = null
            },
        };

        return streetCodes;
    }

    private static List<FactDTO> GetListFactDTO()
    {
        var facts = new List<FactDTO>
        {
            new FactDTO
            {
                Id = 1,
                Title = "Викуп з кріпацтва",
                ImageId = null,
                FactContent = "Навесні 1838-го Карл Брюллов..."
            },
        };

        return facts;
    }
}