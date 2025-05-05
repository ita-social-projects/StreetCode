using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore.Query;  // Add this for IIncludableQueryable
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Partners.GetAll;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Partners;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners
{
    public class GetAllPartnersTest
    {
        private const string _testBase64String = "rVhhWrnh72xHfKGHg6YTV2H4ywe7BorrYUdILaKz0lQ=";

        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _mockBlobService;
        private readonly GetAllPartnersHandler _handler;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public GetAllPartnersTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockBlobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            _handler = new GetAllPartnersHandler(
                _mockRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockLocalizerCannotFind.Object,
                _mockBlobService.Object);
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorrectType()
        {
            // Arrange
            this.SetupPaginatedRepository(GetPartnerList());
            this.SetupMapper(GetListPartnerDto());

            // Act
            var result = await _handler.Handle(new GetAllPartnersQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsType<List<PartnerDto>>(result.Value.Partners)
            );
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CountMatch()
        {
            // Arrange
            this.SetupPaginatedRepository(GetPartnerList());
            this.SetupMapper(GetListPartnerDto());

            // Act
            var result = await _handler.Handle(new GetAllPartnersQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Equal(GetPartnerList().Count(), result.Value.Partners.Count())
            );
        }

        [Fact]
        public async Task Handler_Returns_Correct_PageSize()
        {
            // Arrange
            ushort pageSize = 3;
            this.SetupPaginatedRepository(GetPartnerList().Take(pageSize));
            this.SetupMapper(GetListPartnerDto().Take(pageSize).ToList());

            // Act
            var result = await _handler.Handle(new GetAllPartnersQuery(page: 1, pageSize: pageSize), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<PartnerDto>>(result.Value.Partners),
                () => Assert.Equal(pageSize, result.Value.Partners.Count()));
        }

        private static IEnumerable<Partner> GetPartnerList()
        {
            var partners = new List<Partner>
            {
                new Partner { Id = 1 },
                new Partner { Id = 2 },
                new Partner { Id = 3 },
                new Partner { Id = 4 },
                new Partner { Id = 5 },
            };

            return partners;
        }

        private static List<PartnerDto> GetListPartnerDto()
        {
            var partnersDto = new List<PartnerDto>
            {
                new PartnerDto { Id = 1 },
                new PartnerDto { Id = 2 },
                new PartnerDto { Id = 3 },
                new PartnerDto { Id = 4 },
                new PartnerDto { Id = 5 },
            };

            return partnersDto;
        }

        private void SetupPaginatedRepository(IEnumerable<Partner> returnList)
        {
            // Mocking IQueryable for PartnersRepository's GetAllAsync method
            var mockIncludableQueryable = returnList.AsQueryable();

            // Adjusted mock setup to expect a filter and a function for includes
            _mockRepository.Setup(repo => repo.PartnersRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Partner, bool>>>(),  // Mock the filter expression (for Where)
                    It.IsAny<Func<IQueryable<Partner>, IIncludableQueryable<Partner, object>>>()))  // Mock includes
                .ReturnsAsync(mockIncludableQueryable);
        }

        private void SetupMapper(IEnumerable<PartnerDto> returnList)
        {
            _mockMapper
                .Setup(x => x.Map<IEnumerable<PartnerDto>>(It.IsAny<IEnumerable<Partner>>()))
                .Returns(returnList);
        }
    }
}