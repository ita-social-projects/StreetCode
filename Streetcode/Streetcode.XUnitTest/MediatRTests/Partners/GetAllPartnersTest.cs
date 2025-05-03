using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners
{
    public class GetAllPartnersTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

        public GetAllPartnersTest()
        {
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorrectType()
        {
            // Arrange
            this.SetupGetAllAsync(GetPartnerList());
            this.SetupMapper(GetListPartnerDTO());

            var handler = new GetAllPartnersHandler(
                this.mockRepository.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllPartnersQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsType<List<PartnerDTO>>(result.Value.Partners)
            );
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CountMatch()
        {
            // Arrange
            this.SetupGetAllAsync(GetPartnerList());
            this.SetupMapper(GetListPartnerDTO());

            var handler = new GetAllPartnersHandler(
                this.mockRepository.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllPartnersQuery(), CancellationToken.None);

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
            this.SetupGetAllAsync(GetPartnerList().Take(pageSize));
            this.SetupMapper(GetListPartnerDTO().Take(pageSize).ToList());

            var handler = new GetAllPartnersHandler(
                this.mockRepository.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllPartnersQuery(page: 1, pageSize: pageSize), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<PartnerDTO>>(result.Value.Partners),
                () => Assert.Equal(pageSize, result.Value.Partners.Count())
            );
        }

        private static IEnumerable<Partner> GetPartnerList()
        {
            var partners = new List<Partner>
            {
                new Partner { Id = 1 },
                new Partner { Id = 2 },
                new Partner { Id = 3 },
                new Partner { Id = 4 },
                new Partner { Id = 5 }
            };

            return partners;
        }

        private static List<PartnerDTO> GetListPartnerDTO()
        {
            var partnersDTO = new List<PartnerDTO>
            {
                new PartnerDTO { Id = 1 },
                new PartnerDTO { Id = 2 },
                new PartnerDTO { Id = 3 },
                new PartnerDTO { Id = 4 },
                new PartnerDTO { Id = 5 }
            };

            return partnersDTO;
        }

        private void SetupGetAllAsync(IEnumerable<Partner> returnList)
        {
            this.mockRepository.Setup(repo => repo.PartnersRepository.GetAllAsync(
                It.IsAny<Expression<Func<Partner, bool>>>(),
                It.IsAny<Func<IQueryable<Partner>, IIncludableQueryable<Partner, object>>>()
            ))
            .ReturnsAsync(returnList);
        }

        private void SetupMapper(IEnumerable<PartnerDTO> returnList)
        {
            this.mockMapper
                .Setup(x => x.Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()))
                .Returns(returnList);
        }
    }
}