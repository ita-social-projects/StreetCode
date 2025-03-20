using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using MockQueryable.Moq;
using Moq;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Toponyms.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
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
        private readonly MockCannotFindLocalizer _mockLocalizerCannotFind;
        private readonly GetToponymsByStreetcodeIdHandler _handler;

        public GetToponymsByStreetcodeIdTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerNo = new Mock<IStringLocalizer<NoSharedResource>>();
            _mockLocalizerCannotFind = new MockCannotFindLocalizer();
            _handler = new GetToponymsByStreetcodeIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerNo.Object, _mockLocalizerCannotFind);
        }

        [Fact]
        public async Task Handler_ToponymNotEmptyAndUserHasAccess_ReturnsSuccessfullyList()
        {
            // Arrange
            SetupRepositoryMock(GetExistingToponymList(), GetStreetcodeList());

            _mockMapper.Setup(x => x
            .Map<IEnumerable<ToponymDTO>>(It.IsAny<IEnumerable<Toponym>>()))
            .Returns(GetToponymDTOList());

            int streetcodeId = 1;

            // Act
            var result = await _handler.Handle(new GetToponymsByStreetcodeIdQuery(streetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.NotEmpty(result.Value));
        }

        [Fact]
        public async Task Handler_ToponymNotEmptyButUserDoesNotHaveAccess_ReturnsError()
        {
            // Arrange
            int streetcodeId = 1;

            SetupRepositoryMock(GetExistingToponymList(), new List<StreetcodeContent>());

            _mockMapper.Setup(x => x
                    .Map<IEnumerable<ToponymDTO>>(It.IsAny<IEnumerable<Toponym>>()))
                .Returns(GetToponymDTOList());

            var expectedError = _mockLocalizerCannotFind["CannotFindAnyStreetcodeWithCorrespondingId", streetcodeId].Value;

            // Act
            var result = await _handler.Handle(new GetToponymsByStreetcodeIdQuery(streetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        [Fact]
        public async Task Handler_ToponymEmptyAndUserHasAccess_ReturnsSuccessfullyEmptyList()
        {
            // Arrange
            SetupRepositoryMock(GetEmptyToponymList(), GetStreetcodeList());

            int streetcodeId = 1;

            _mockLocalizerNo.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
               .Returns((string key, object[] args) =>
               {
                   if (args != null && args.Length > 0 && args[0] is int id)
                   {
                       return new LocalizedString(key, $"No toponym with such streetcode id: {id}");
                   }

                   return new LocalizedString(key, "Cannot find any toponym with unknown id");
               });

            // Act
            var result = await _handler.Handle(new GetToponymsByStreetcodeIdQuery(streetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
               () => Assert.IsType<Result<IEnumerable<ToponymDTO>>>(result),
               () => Assert.IsAssignableFrom<IEnumerable<ToponymDTO>>(result.Value),
               () => Assert.Empty(result.Value));
        }

        private void SetupRepositoryMock(List<Toponym> toponyms, List<StreetcodeContent> streetcodeListUserCanAccess)
        {
            _mockRepository.Setup(x => x.ToponymRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Toponym, bool>>>(),
                    It.IsAny<Func<IQueryable<Toponym>,
                        IIncludableQueryable<Toponym, object>>>()))
                .ReturnsAsync(toponyms);

            _mockRepository.Setup(repo => repo.StreetcodeRepository
                    .FindAll(
                        It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                        It.IsAny<Func<IQueryable<StreetcodeContent>,
                            IIncludableQueryable<StreetcodeContent, object>>>()))
                .Returns(streetcodeListUserCanAccess.AsQueryable().BuildMockDbSet().Object);
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

        private static List<StreetcodeContent> GetStreetcodeList()
        {
            return new List<StreetcodeContent>
            {
                new StreetcodeContent
                {
                    Id = 1,
                },
            };
        }
    }
}