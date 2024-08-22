using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByTransliterationUrl;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetStreetcodeByTransliterationUrlHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly StreetcodeContent? nullValue = null;
        private readonly StreetcodeDTO? nullValueDTO = null;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

        public GetStreetcodeByTransliterationUrlHandlerTests()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData("some")]
        public async Task ExistingUrl(string url)
        {
            // Arrange
            this.SetupMapper(url);
            this.SetupRepository(url);

            var handler = new GetStreetcodeByTransliterationUrlHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetStreetcodeByTransliterationUrlQuery(url), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(result.Value.TransliterationUrl, url));
        }

        [Theory]
        [InlineData("some")]
        public async Task NotExistingId(string url)
        {
            // Arrange
            this.mockRepo.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
              It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), It.IsAny<Func<IQueryable<StreetcodeContent>,
               IIncludableQueryable<StreetcodeContent, object>>>())).ReturnsAsync(this.nullValue);

            this.mockMapper.Setup(x => x.Map<StreetcodeDTO?>(It.IsAny<StreetcodeContent>())).Returns(this.nullValueDTO);

            var expectedError = $"Cannot find streetcode by transliteration url: {url}";
            this.mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
               .Returns((string key, object[] args) =>
               {
                   if (args != null && args.Length > 0 && args[0] is string url)
                   {
                       return new LocalizedString(key, $"Cannot find streetcode by transliteration url: {url}");
                   }

                   return new LocalizedString(key, "Cannot find any streetcode with unknown transliteration url");
               });

            var handler = new GetStreetcodeByTransliterationUrlHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetStreetcodeByTransliterationUrlQuery(url), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors[0].Message));
        }

        [Theory]
        [InlineData("some")]
        public async Task CorrectType(string url)
        {
            // Arrange
            this.SetupMapper(url);
            this.SetupRepository(url);

            var handler = new GetStreetcodeByTransliterationUrlHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetStreetcodeByTransliterationUrlQuery(url), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result.ValueOrDefault),
                () => Assert.IsType<StreetcodeDTO>(result.ValueOrDefault));
        }

        private void SetupRepository(string url)
        {
            this.mockRepo
                .Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                    It.IsAny<Func<IQueryable<StreetcodeContent>,
                    IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(new StreetcodeContent() { TransliterationUrl = url });
            this.mockRepo
                .Setup(repo => repo.StreetcodeTagIndexRepository.GetAllAsync(
                    It.IsAny<Expression<Func<StreetcodeTagIndex, bool>>>(),
                    It.IsAny<Func<IQueryable<StreetcodeTagIndex>,
                    IIncludableQueryable<StreetcodeTagIndex, object>>>()))
                .ReturnsAsync(new List<StreetcodeTagIndex>());
        }

        private void SetupMapper(string url)
        {
            this.mockMapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<StreetcodeContent>())).Returns(new StreetcodeDTO() { TransliterationUrl = url });
        }
    }
}
