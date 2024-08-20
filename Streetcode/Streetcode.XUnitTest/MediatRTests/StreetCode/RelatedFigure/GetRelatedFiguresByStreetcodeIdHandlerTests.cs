using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using Entities = Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.XUnitTest.MediatRTests.Streetcode.RelatedFigure
{
    public class GetRelatedFiguresByStreetcodeIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public GetRelatedFiguresByStreetcodeIdHandlerTests()
        {
            this._repository = new Mock<IRepositoryWrapper>();
            this._mapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ExistingId_ReturnsSuccess(int id)
        {
            // Arrange
            var testRelatedList = new List<Entities.RelatedFigure>()
            {
                new Entities.RelatedFigure(),
            };

            var testPersonStreetcodeList = new List<PersonStreetcode>()
            {
                new PersonStreetcode(),
            };

            var testRelatedDTOList = new List<RelatedFigureDTO>()
            {
                new RelatedFigureDTO(),
            };

            this.RepositorySetup(testRelatedList.AsQueryable(), new List<StreetcodeContent>());

            this._repository.Setup(x => x.StreetcodeRepository.GetAllAsync(
                    It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(),
                    It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync(testPersonStreetcodeList);

            this._mapper.Setup(x => x.Map<IEnumerable<RelatedFigureDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(testRelatedDTOList);

            var handler = new GetRelatedFiguresByStreetcodeIdHandler(this._mapper.Object, this._repository.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.NotEmpty(result.Value));
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsCorrectType(int id)
        {
            // Arrange
            var testStreetcodeContentList = new List<StreetcodeContent>()
            {
                new StreetcodeContent(),
            };

            var testRelatedFigureList = new List<Entities.RelatedFigure>()
            {
                new Entities.RelatedFigure(),
            };

            var testRelatedDTO = new RelatedFigureDTO() { Id = id };

            this._repository.Setup(x => x.StreetcodeRepository
                .GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync(testStreetcodeContentList.AsQueryable());

            this.RepositorySetup(testRelatedFigureList.AsQueryable(), testStreetcodeContentList);
            this.MapperSetup(testRelatedDTO);

            var handler = new GetRelatedFiguresByStreetcodeIdHandler(this._mapper.Object, this._repository.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(id), CancellationToken.None);

            // Assert
            Assert.IsAssignableFrom<IEnumerable<RelatedFigureDTO>>(result.Value);
        }

        [Theory]
        [InlineData(2)]
        public async Task Handle_NonExisting_ReturnsGetAllNull(int id)
        {
            // Arrange
            var testRelatedFigureEmptyList = new List<Entities.RelatedFigure>();
            var testRelatedDTO = new RelatedFigureDTO() { Id = id };
            string expectedErrorMessage = $"Cannot find any related figures by a streetcode id: {id}";
            this._mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
            {
                if (args != null && args.Length > 0 && args[0] is int id)
                {
                    return new LocalizedString(key, $"Cannot find any related figures by a streetcode id: {id}");
                }

                return new LocalizedString(key, "Cannot find any related figures with unknown id");
            });

            this.RepositorySetup(testRelatedFigureEmptyList.AsQueryable(), new List<StreetcodeContent>());
            this.MapperSetup(testRelatedDTO);

            var handler = new GetRelatedFiguresByStreetcodeIdHandler(this._mapper.Object, this._repository.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        [Theory]
        [InlineData(2)]
        public async Task Handle_NonExisting_ReturnsFindAllNull(int id)
        {
            // Arrange
            string expectedErrorMessage = $"Cannot find any related figures by a streetcode id: {id}";
            this._mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
            {
                if (args != null && args.Length > 0 && args[0] is int id)
                {
                    return new LocalizedString(key, $"Cannot find any related figures by a streetcode id: {id}");
                }

                return new LocalizedString(key, "Cannot find any related figures with unknown id");
            });
            var testRelatedDTO = new RelatedFigureDTO() { Id = id };

            this.RepositorySetup(Enumerable.Empty<Entities.RelatedFigure>().AsQueryable(), new List<StreetcodeContent>());
            this.MapperSetup(testRelatedDTO);

            var handler = new GetRelatedFiguresByStreetcodeIdHandler(this._mapper.Object, this._repository.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
        }

        [Fact]
        public void Handle_OrdersImagesByAlt()
        {
            // Arrange
            var relatedFigures = new List<StreetcodeContent>
            {
                new StreetcodeContent
                {
                    Images = new List<Image>
                    {
                        new Image { ImageDetails = new ImageDetails { Alt = "B" } },
                        new Image { ImageDetails = new ImageDetails { Alt = "A" } },
                    },
                },
            };

            // Act
            foreach (StreetcodeContent streetcode in relatedFigures)
            {
                if (streetcode.Images != null)
                {
                    streetcode.Images = streetcode.Images.OrderBy(img => img.ImageDetails?.Alt).ToList();
                }
            }

            // Assert
            Assert.Equal("A", relatedFigures[0].Images[0].ImageDetails?.Alt);
            Assert.Equal("B", relatedFigures[0].Images[1].ImageDetails?.Alt);
        }

        private void MapperSetup(RelatedFigureDTO relatedFigureDTO)
        {
            this._mapper.Setup(x => x.Map<RelatedFigureDTO>(It.IsAny<StreetcodeContent>()))
                .Returns(relatedFigureDTO);
        }

        private void RepositorySetup(IQueryable<Entities.RelatedFigure> relatedFigures, List<StreetcodeContent> streetcodeContents)
        {
            this._repository.Setup(x => x.RelatedFigureRepository
                .FindAll(It.IsAny<Expression<Func<Entities.RelatedFigure, bool>>?>(), null))
                .Returns(relatedFigures);

            this._repository.Setup(x => x.StreetcodeRepository
                .GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync(streetcodeContents);

            this._repository.Setup(x => x.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync(streetcodeContents);
        }
    }
}
