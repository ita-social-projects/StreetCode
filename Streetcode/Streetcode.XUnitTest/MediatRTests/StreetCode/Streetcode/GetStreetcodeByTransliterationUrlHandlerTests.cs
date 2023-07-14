using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByTransliterationUrl;
using Streetcode.BLL.MediatR.Transactions.TransactionLink.GetById;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetStreetcodeByTransliterationUrlHandlerTests
    {

        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly StreetcodeContent? nullValue = null;
        private readonly StreetcodeDTO? nullValueDTO = null;
        private readonly Mock<ILoggerService> _mockLogger;
        public GetStreetcodeByTransliterationUrlHandlerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        async Task SetupRepository(string url)
        {
            _mockRepo.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>>())).ReturnsAsync(new StreetcodeContent() { TransliterationUrl = url });
            _mockRepo.Setup(repo => repo.StreetcodeTagIndexRepository.GetAllAsync(
              It.IsAny<Expression<Func<StreetcodeTagIndex, bool>>>(),
              It.IsAny<Func<IQueryable<StreetcodeTagIndex>,
              IIncludableQueryable<StreetcodeTagIndex, object>>>()))
              .ReturnsAsync(new List<StreetcodeTagIndex>());
        }

        async Task SetupMapper(string url)
        {
            _mockMapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<StreetcodeContent>())).Returns(new StreetcodeDTO() { TransliterationUrl = url });
        }

        [Theory]
        [InlineData("some")]
        public async Task ExistingUrl(string url)
        {
            //Arrange
            await SetupMapper(url);
            await SetupRepository(url);

            var handler = new GetStreetcodeByTransliterationUrlHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);

            //Act
            var result = await handler.Handle(new GetStreetcodeByTransliterationUrlQuery(url), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(result.Value.TransliterationUrl, url)
            );
        }

        [Theory]
        [InlineData("some")]
        public async Task NotExistingId(string url)
        {
            //Arrange
            _mockRepo.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
              It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), It.IsAny<Func<IQueryable<StreetcodeContent>,
               IIncludableQueryable<StreetcodeContent, object>>>())).ReturnsAsync(nullValue);

            _mockMapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<StreetcodeContent>())).Returns(nullValueDTO);

            var expectedError = $"Cannot find streetcode by transliteration url: {url}";

            var handler = new GetStreetcodeByTransliterationUrlHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);

            //Act
            var result = await handler.Handle(new GetStreetcodeByTransliterationUrlQuery(url), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors.First().Message)
            );
        }

        [Theory]
        [InlineData("some")]
        public async Task CorrectType(string url)
        {
            //Arrange
            await SetupMapper(url);
            await SetupRepository(url);

            var handler = new GetStreetcodeByTransliterationUrlHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);

            //Act
            var result = await handler.Handle(new GetStreetcodeByTransliterationUrlQuery(url), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.NotNull(result.ValueOrDefault),
                () => Assert.IsType<StreetcodeDTO>(result.ValueOrDefault)
            );
        }
    }
}

