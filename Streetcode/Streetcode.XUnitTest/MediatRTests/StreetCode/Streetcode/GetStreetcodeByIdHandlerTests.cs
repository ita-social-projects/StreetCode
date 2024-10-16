using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetStreetcodeByIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> repository;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

        public GetStreetcodeByIdHandlerTests()
        {
            this.repository = new Mock<IRepositoryWrapper>();
            this.mapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsSuccess(int id)
        {
            // Arrange
            var testContentDTO = new EventStreetcodeDTO();
            var testContent = new StreetcodeContent();

            this.RepositorySetup(testContent);
            this.MapperSetup(testContentDTO);

            var handler = new GetStreetcodeByIdHandler(this.repository.Object, this.mapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // act
            var result = await handler.Handle(new GetStreetcodeByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsCorrectType(int id)
        {
            // arrange
            var testContentDTO = new EventStreetcodeDTO();
            var testContent = new StreetcodeContent();

            this.RepositorySetup(testContent);
            this.MapperSetup(testContentDTO);

            var handler = new GetStreetcodeByIdHandler(this.repository.Object, this.mapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // act
            var result = await handler.Handle(new GetStreetcodeByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.IsAssignableFrom<StreetcodeDTO>(result.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsError(int id)
        {
            // arrange
            string expectedErrorMessage = $"Cannot find any streetcode with corresponding id: {id}";
            this.mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
               .Returns((string key, object[] args) =>
               {
                   if (args != null && args.Length > 0 && args[0] is int id)
                   {
                       return new LocalizedString(key, $"Cannot find any streetcode with corresponding id: {id}");
                   }

                   return new LocalizedString(key, "Cannot find any streetcode with unknown id");
               });

            this.RepositorySetup(null);
            this.MapperSetup(null);

            var handler = new GetStreetcodeByIdHandler(this.repository.Object, this.mapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // act
            var result = await handler.Handle(new GetStreetcodeByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        private void RepositorySetup(StreetcodeContent? streetcode)
        {
            this.repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(streetcode);
            this.repository.Setup(repo => repo.StreetcodeTagIndexRepository.GetAllAsync(
               It.IsAny<Expression<Func<StreetcodeTagIndex, bool>>>(),
               It.IsAny<Func<IQueryable<StreetcodeTagIndex>,
               IIncludableQueryable<StreetcodeTagIndex, object>>>()))
               .ReturnsAsync(new List<StreetcodeTagIndex>());
        }

        private void MapperSetup(EventStreetcodeDTO? streetcodeDTO)
        {
            this.mapper.Setup(x => x.Map<StreetcodeDTO?>(It.IsAny<object>()))
                .Returns(streetcodeDTO);
        }
    }
}
