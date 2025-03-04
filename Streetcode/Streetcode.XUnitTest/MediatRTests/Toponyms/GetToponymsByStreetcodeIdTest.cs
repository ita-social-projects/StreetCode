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
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<NoSharedResource>> _mockLocalizerNo;

        public GetToponymsByStreetcodeIdTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerNo = new Mock<IStringLocalizer<NoSharedResource>>();
        }

        [Fact]
        public async Task ReturnsSuccessfully_NotEmpty()
        {
            // Arrange
            this._mockRepository.Setup(x => x.ToponymRepository.GetAllAsync(
                It.IsAny<Expression<Func<Toponym, bool>>>(),
                It.IsAny<Func<IQueryable<Toponym>,
                IIncludableQueryable<Toponym, object>>>()))
                .ReturnsAsync(GetExistingToponymList());

            this._mockMapper.Setup(x => x
            .Map<IEnumerable<ToponymDTO>>(It.IsAny<IEnumerable<Toponym>>()))
            .Returns(GetToponymDTOList());

            int streetcodeId = 1;
            var handler = new GetToponymsByStreetcodeIdHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerNo.Object);

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
            this._mockRepository.Setup(x => x.ToponymRepository.GetAllAsync(
                It.IsAny<Expression<Func<Toponym, bool>>>(),
                It.IsAny<Func<IQueryable<Toponym>,
                IIncludableQueryable<Toponym, object>>>()))
                .ReturnsAsync(GetEmptyToponymList());

            int streetcodeId = 1;

            this._mockLocalizerNo.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
               .Returns((string key, object[] args) =>
               {
                   if (args != null && args.Length > 0 && args[0] is int id)
                   {
                       return new LocalizedString(key, $"No toponym with such streetcode id: {id}");
                   }

                   return new LocalizedString(key, "Cannot find any toponym with unknown id");
               });

            var handler = new GetToponymsByStreetcodeIdHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerNo.Object);

            // Act
            var result = await handler.Handle(new GetToponymsByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            // Assert
            Assert.Multiple(
               () => Assert.IsType<Result<IEnumerable<ToponymDTO>>>(result),
               () => Assert.IsAssignableFrom<IEnumerable<ToponymDTO>>(result.Value),
               () => Assert.Empty(result.Value));
        }

        private static List<Toponym> GetExistingToponymList()
        {
            return new List<Toponym>()
            {
                new ()
                {
                    Id = 1,
                    Oblast = "Test oblast",
                    StreetName = "Test street"
                }
            };
        }

        private static List<ToponymDTO> GetToponymDTOList()
        {
            return new List<ToponymDTO>
            {
                new ()
                {
                    Id = 1,
                    Oblast = "Test oblast",
                    StreetName = "Test street"
                }
            };
        }

        private static List<Toponym> GetEmptyToponymList()
        {
            return new List<Toponym>();
        }
    }
}
