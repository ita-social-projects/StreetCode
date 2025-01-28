using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Toponyms.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Toponyms
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1413:UseTrailingCommasInMultiLineInitializers", Justification = "Reviewed.")]
    public class GetToponymsByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

        public GetToponymsByStreetcodeIdTest()
        {
            mockRepository = new Mock<IRepositoryWrapper>();
            mockMapper = new Mock<IMapper>();
            mockLogger = new Mock<ILoggerService>();
            mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ReturnsSuccessfully_NotEmpty()
        {
            // Arrange
            this.mockRepository.Setup(x => x.ToponymRepository.GetAllAsync(
                It.IsAny<Expression<Func<Toponym, bool>>>(),
                It.IsAny<Func<IQueryable<Toponym>,
                IIncludableQueryable<Toponym, object>>>()
                )).ReturnsAsync(GetExistingToponymList());

            this.mockMapper.Setup(x => x
            .Map<IEnumerable<ToponymDTO>>(It.IsAny<IEnumerable<Toponym>>()))
            .Returns(GetToponymDTOList());

            int streetcodeId = 1;
            var handler = new GetToponymsByStreetcodeIdHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetToponymsByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            // Assert
            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.NotEmpty(result.Value));
        }

        [Fact]
        public async Task ReturnsSuccessfully_Empty()
        {
            // Arrange
            this.mockRepository.Setup(x => x.ToponymRepository.GetAllAsync(
                It.IsAny<Expression<Func<Toponym, bool>>>(),
                It.IsAny<Func<IQueryable<Toponym>,
                IIncludableQueryable<Toponym, object>>>()
                )).ReturnsAsync(GetEmptyToponymList());

            int streetcodeId = 1;
            var handler = new GetToponymsByStreetcodeIdHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetToponymsByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            // Assert
            Assert.Multiple(
               () => Assert.IsType<Result<IEnumerable<ToponymDTO>>>(result),
               () => Assert.IsAssignableFrom<IEnumerable<ToponymDTO>>(result.Value),
               () => Assert.Empty(result.Value));
        }

        public static List<Toponym> GetExistingToponymList()
        {
            return new List<Toponym>() {
                new Toponym
                {
                    Id = 1,
                    Oblast = "Test oblast",
                    StreetName = "Test street"
                }
            };
        }

        public static List<ToponymDTO> GetToponymDTOList() {
            return new List<ToponymDTO> {
                new ToponymDTO
                {
                    Id = 1,
                    Oblast = "Test oblast",
                    StreetName = "Test street"
                }
            };
        }
        public static List<Toponym> GetEmptyToponymList()
        {
            return new List<Toponym>();
        }
    }
}
