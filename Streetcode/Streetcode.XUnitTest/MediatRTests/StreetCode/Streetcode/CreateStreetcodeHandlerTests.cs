using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode.Create;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1124:Do not use regions", Justification = "For better reading")]
    public class CreateStreetcodeHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryMock;
        private readonly Mock<ILoggerService> _loggerMock;
        private readonly Mock<IStringLocalizer<AnErrorOccurredSharedResource>> _localizerErrorMock;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> _localizerFailedToCreateMock;
        private readonly Mock<IStringLocalizer<FailedToValidateSharedResource>> _localizerValidationMock;
        private readonly Mock<IStringLocalizer<FieldNamesSharedResource>> _localizerFieldMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IHttpContextAccessor> _httpAccessorMock;
        private CreateStreetcodeHandler _handler;

        public CreateStreetcodeHandlerTests()
        {
            _localizerValidationMock = new Mock<IStringLocalizer<FailedToValidateSharedResource>>();
            _localizerFieldMock = new Mock<IStringLocalizer<FieldNamesSharedResource>>();
            _repositoryMock = new Mock<IRepositoryWrapper>();
            _loggerMock = new Mock<ILoggerService>();
            _mapperMock = new Mock<IMapper>();
            _localizerErrorMock = new Mock<IStringLocalizer<AnErrorOccurredSharedResource>>();
            _localizerFailedToCreateMock = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            _httpAccessorMock = new Mock<IHttpContextAccessor>();

            _handler = new CreateStreetcodeHandler(
                _mapperMock.Object,
                _repositoryMock.Object,
                _loggerMock.Object,
                _localizerErrorMock.Object,
                _localizerFailedToCreateMock.Object,
                _localizerValidationMock.Object,
                _localizerFieldMock.Object,
                _httpAccessorMock.Object);
        }

        #region AddImagesDetails
        [Fact]
        public async Task AddImagesDetails_WhenValidImageDetails_CreatesImageDetailsSuccessfully()
        {
            // Arrange
            var alt1 = ((int)ImageAssigment.Blackandwhite).ToString();
            var alt2 = ((int)ImageAssigment.Animation).ToString();

            var imageDetails = new List<ImageDetailsDto>
            {
                new () { Alt = alt1 },
                new () { Alt = alt2 },
            };
            SetupMockStreetcodeImageDetailsCreate();

            // Act
            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddImagesDetails", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)methodInfo?.Invoke(_handler, new object[] { imageDetails }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.ImageDetailsRepository.CreateRangeAsync(It.IsAny<IEnumerable<ImageDetails>>()), Times.Once);
        }

        [Fact]
        public async Task AddImagesDetails_WhenImageDetailsIsNull_ThrowException()
        {
            // Arrange
            List<ImageDetailsDto>? imageDetails = null;

            SetupMockStreetcodeImageDetailsCreate();

            // Act
            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddImagesDetails", BindingFlags.NonPublic | BindingFlags.Instance);
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { imageDetails! });

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();

            _repositoryMock.Verify(
                repo => repo.ImageDetailsRepository.CreateRangeAsync(It.IsAny<IEnumerable<ImageDetails>>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_WhenImageDetailsIsNull_ReturnsErrorResult()
        {
            var streetcodeCreateDto = Create();
            streetcodeCreateDto.ImagesDetails = null;
            var request = new CreateStreetcodeCommand(streetcodeCreateDto);
            SetupMockAnErrorOccurredWhileCreatingLocalizer();
            SetupMocksForCreateStreetcode();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("Field ImagesDetails cannot be empty", result.Errors.First().Message);
        }

        #endregion

        #region AddImages
        [Fact]
        public async Task AddImagesAsync_WhenValidImagesIds_CreatesImagesSuccessfully()
        {
            // Arrange
            StreetcodeContent streetcode = new StreetcodeContent();
            IEnumerable<int> imagesIds = new List<int> { 1, 2, 3 };
            SetupMockStreetcodeImageCreate();

            // Act
            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddImagesAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, imagesIds });

            // Assert
            _repositoryMock.Verify(repo => repo.StreetcodeImageRepository.CreateRangeAsync(It.IsAny<IEnumerable<StreetcodeImage>>()), Times.Once);
        }

        [Fact]
        public async Task AddImagesAsync_WhenImagesIdsIsNull_ThrowException()
        {
            // Arrange
            StreetcodeContent streetcode = new StreetcodeContent();
            IEnumerable<int>? imagesIds = null;
            SetupMockStreetcodeImageCreate();

            // Act
            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddImagesAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, imagesIds });

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();

            _repositoryMock.Verify(
                repo => repo.StreetcodeImageRepository.CreateRangeAsync(It.IsAny<IEnumerable<StreetcodeImage>>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_WhenImagesIsNull_ReturnsErrorResult()
        {
            var streetcodeCreateDto = Create();
            streetcodeCreateDto.ImagesIds = null!;
            var request = new CreateStreetcodeCommand(streetcodeCreateDto);
            SetupMockAnErrorOccurredWhileCreatingLocalizer();
            SetupMocksForCreateStreetcode();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("Field Images cannot be empty", result.Errors.First().Message);
        }

        #endregion

        #region AddTimelineItems
        [Fact]
        public async Task AddTimelineItems_WhenValidTimelineItems_CreatesAndSavesSuccessfully()
        {
            // Arrange
            StreetcodeContent streetcode = new StreetcodeContent();
            IEnumerable<TimelineItemCreateUpdateDTO> timelineItems = new List<TimelineItemCreateUpdateDTO>()
            {
                new ()
                {
                    Id = 2,
                    Title = "TimeLine",
                    Date = new DateTime(2021, 1, 1),
                    DateViewPattern = DateViewPattern.Year,
                    HistoricalContexts = new List<HistoricalContextCreateUpdateDTO>()
                    {
                        new () { Id = 0, Title = "Historical Context 1" },
                        new () { Id = 2, Title = "Historical Context 2" },
                    },
                },
            };
            SetupMockAddTimeLineItems();

            var methodInfo = typeof(CreateStreetcodeHandler)
                .GetMethod("AddTimelineItems", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, timelineItems! });

            // Assert
            _repositoryMock.Verify(repo => repo.HistoricalContextRepository.CreateRangeAsync(It.IsAny<IEnumerable<HistoricalContext>>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddTimelineItems_WhenTimelineItemsIsNull_DoesNotThrowException_And_DoesNotSaveAnything()
        {
            // Arrange
            StreetcodeContent streetcode = new StreetcodeContent();
            IEnumerable<TimelineItemCreateUpdateDTO>? timelineItems = null;

            var methodInfo = typeof(CreateStreetcodeHandler)
                .GetMethod("AddTimelineItems", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, timelineItems! });

            // Assert
            await action.Should().NotThrowAsync();

            _repositoryMock.Verify(
                repo => repo.HistoricalContextRepository.CreateRangeAsync(It.IsAny<IEnumerable<HistoricalContext>>()),
                Times.Never);

            _repositoryMock.Verify(
                repo => repo.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Handle_WhenTimelineItemsIsNull_DoesNotReturnsErrorResult()
        {
            // Arrange
            var streetcodeCreateDto = Create();
            streetcodeCreateDto.TimelineItems = null;
            var request = new CreateStreetcodeCommand(streetcodeCreateDto);

            SetupMocksForCreateStreetcode();
            SetupMockAnErrorOccurredWhileCreatingLocalizer();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _loggerMock.Verify(
                logger => logger.LogError(It.IsAny<object>(), It.Is<string>(msg => msg.Contains("AnErrorOccurredWhileCreating"))),
                Times.Never);

            _repositoryMock.Verify(
                repo => repo.HistoricalContextRepository.CreateRangeAsync(It.IsAny<IEnumerable<HistoricalContext>>()),
                Times.Never);
        }
        #endregion

        #region AddAudio
        [Fact]
        public void AddAudio_WhenValidAudioId_AddsSuccessfully()
        {
            // Arrange
            StreetcodeContent streetcode = new StreetcodeContent();
            var audioId = 1;

            var methodInfo = typeof(CreateStreetcodeHandler)
                .GetMethod("AddAudio", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            methodInfo?.Invoke(_handler, new object[] { streetcode, audioId });

            // Assert
            Assert.Equal(streetcode.AudioId, audioId);
        }
        #endregion

        #region AddArtGallery
        [Fact]
        public async Task AddArtGallery_WhenValidData_AddsSuccessfully()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1 };
            var artSlides = new List<StreetcodeArtSlideCreateUpdateDTO>
            {
                new ()
                {
                    SlideId = 1,
                    StreetcodeId = 1,
                    Template = StreetcodeArtSlideTemplate.OneToTwo,
                    Index = 0,
                    StreetcodeArts = new List<StreetcodeArtCreateUpdateDTO>
                    {
                        new () { ArtId = 10 },
                    },
                },
            };

            var arts = new List<ArtCreateUpdateDTO>
            {
                new () { Id = 10, ImageId = 100, Description = "ArtDesc", Title = "ArtTitle" },
            };

            SetupMockAddArtGallery();

            // Act
            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddArtGallery", BindingFlags.NonPublic | BindingFlags.Instance);
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, artSlides, arts });

            // Assert
            await action.Should().NotThrowAsync();

            _repositoryMock.Verify(repo => repo.StreetcodeArtSlideRepository.CreateRangeAsync(It.IsAny<List<StreetcodeArtSlide>>()), Times.Once);
            _repositoryMock.Verify(repo => repo.ArtRepository.CreateRangeAsync(It.IsAny<List<Art>>()), Times.Once);
            _repositoryMock.Verify(repo => repo.StreetcodeArtRepository.CreateRangeAsync(It.IsAny<List<StreetcodeArt>>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Exactly(3));
        }

        [Fact]
        public async Task AddArtGallery_WhenArtSlidesEmpty_AddsSuccessfully()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1 };
            var artSlides = new List<StreetcodeArtSlideCreateUpdateDTO>();

            var arts = new List<ArtCreateUpdateDTO>
            {
                new () { Id = 10, ImageId = 100, Description = "ArtDesc", Title = "ArtTitle" },
            };

            SetupMockAddArtGallery();

            // Act
            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddArtGallery", BindingFlags.NonPublic | BindingFlags.Instance);
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, artSlides, arts });

            // Assert
            await action.Should().NotThrowAsync();

            _repositoryMock.Verify(repo => repo.StreetcodeArtSlideRepository.CreateRangeAsync(It.IsAny<List<StreetcodeArtSlide>>()), Times.Never);
            _repositoryMock.Verify(repo => repo.ArtRepository.CreateRangeAsync(It.IsAny<List<Art>>()), Times.Never);
            _repositoryMock.Verify(repo => repo.StreetcodeArtRepository.CreateRangeAsync(It.IsAny<List<StreetcodeArt>>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task AddArtGallery_WhenArtSlideAndArtsAreEmpty_DoesNotThrowException()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1 };
            var artSlides = new List<StreetcodeArtSlideCreateUpdateDTO>();
            var arts = new List<ArtCreateUpdateDTO>();

            SetupMockAddArtGallery();

            // Act
            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddArtGallery", BindingFlags.NonPublic | BindingFlags.Instance);
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, artSlides, arts });

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task AddArtGallery_WhenArtSlideAndArtsAreNull_DoesNotThrowException()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1 };
            List<StreetcodeArtSlideCreateUpdateDTO> artSlides = null!;
            List<ArtCreateUpdateDTO> arts = null!;

            SetupMockAddArtGallery();

            // Act
            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddArtGallery", BindingFlags.NonPublic | BindingFlags.Instance);
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, artSlides, arts });

            // Assert
            await action.Should().NotThrowAsync();

            _repositoryMock.Verify(repo => repo.StreetcodeArtSlideRepository.CreateRangeAsync(It.IsAny<List<StreetcodeArtSlide>>()), Times.Never);
            _repositoryMock.Verify(repo => repo.ArtRepository.CreateRangeAsync(It.IsAny<List<Art>>()), Times.Never);
            _repositoryMock.Verify(repo => repo.StreetcodeArtRepository.CreateRangeAsync(It.IsAny<List<StreetcodeArt>>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Exactly(0));
        }

        [Fact]
        public async Task Handle_WhenArtSlideAndArtsAreNull_DoesNotReturnErrorResult()
        {
            var streetcodeCreateDto = Create();
            streetcodeCreateDto.StreetcodeArtSlides = null!;
            streetcodeCreateDto.Arts = null!;
            var request = new CreateStreetcodeCommand(streetcodeCreateDto);
            SetupMockAnErrorOccurredWhileCreatingLocalizer();
            SetupMocksForCreateStreetcode();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _loggerMock.Verify(
                logger => logger.LogError(It.IsAny<object>(), It.Is<string>(msg => msg.Contains("AnErrorOccurredWhileCreating"))),
                Times.Never);

            _repositoryMock.Verify(repo => repo.StreetcodeArtSlideRepository.CreateRangeAsync(It.IsAny<List<StreetcodeArtSlide>>()), Times.Never);
            _repositoryMock.Verify(repo => repo.ArtRepository.CreateRangeAsync(It.IsAny<List<Art>>()), Times.Never);
            _repositoryMock.Verify(repo => repo.StreetcodeArtRepository.CreateRangeAsync(It.IsAny<List<StreetcodeArt>>()), Times.Never);
        }

        #endregion

        #region AddTags
        [Fact]
        public async Task AddTags_WhenValidTags_AddsSuccessfully()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1 };
            var tags = new List<StreetcodeTagDTO>
            {
                new () { Id = 5, Title = "History", IsVisible = true },
                new () { Id = 10, Title = "Culture", IsVisible = false },
            };
            SetupMockAddTags();

            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddTags", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, tags });

            // Assert
            await action.Should().NotThrowAsync();
            _repositoryMock.Verify(repo => repo.StreetcodeTagIndexRepository.CreateRangeAsync(It.IsAny<List<StreetcodeTagIndex>>()), Times.Once);
        }

        [Fact]
        public async Task AddTags_WhenTagExist_ThrowException()
        {
            // Arrange
            var streetcode = new StreetcodeContent();
            var tags = new List<StreetcodeTagDTO>
            {
                new () { Id = -1, Title = "ExistingTag", IsVisible = false },
            };
            SetupMockAddTags(true);

            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddTags", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, tags });

            // Assert
            await action.Should().ThrowAsync<HttpRequestException>();
            _repositoryMock.Verify(repo => repo.TagRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Tag, bool>>>(), null), Times.Once);
            _repositoryMock.Verify(repo => repo.StreetcodeTagIndexRepository.CreateRangeAsync(It.IsAny<List<StreetcodeTagIndex>>()), Times.Never);
        }

        [Fact]
        public async Task AddTags_WhenTagsIsNull_DoesNotThrowException()
        {
            // Arrange
            var streetcode = new StreetcodeContent();
            List<StreetcodeTagDTO> tags = null;
            SetupMockAddTags();

            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddTags", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, tags });

            // Assert
            await action.Should().NotThrowAsync();
            _repositoryMock.Verify(repo => repo.StreetcodeTagIndexRepository.CreateRangeAsync(It.IsAny<List<StreetcodeTagIndex>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenTagsIsNull_DoesNotReturnsErrorResult()
        {
            var streetcodeCreateDto = Create();
            streetcodeCreateDto.Tags = null;
            var request = new CreateStreetcodeCommand(streetcodeCreateDto);
            SetupMockAnErrorOccurredWhileCreatingLocalizer();
            SetupMocksForCreateStreetcode();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _loggerMock.Verify(
                logger => logger.LogError(It.IsAny<object>(), It.Is<string>(msg => msg.Contains("AnErrorOccurredWhileCreating"))),
                Times.Never);

            _repositoryMock.Verify(repo => repo.StreetcodeTagIndexRepository.CreateRangeAsync(It.IsAny<List<StreetcodeTagIndex>>()), Times.Never);
        }
        #endregion

        #region AddRelatedFigures
        [Fact]
        public async Task AddRelatedFigures_WhenValidFigures_AddsSuccessfully()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1 };
            var relatedFigures = new List<RelatedFigureShortDTO>
            {
                new () { Id = 101 },
                new () { Id = 102 },
            };

            SetupMockAddRelaredFigures();

            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddRelatedFigures", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, relatedFigures });

            // Assert
            await action.Should().NotThrowAsync();
            _repositoryMock.Verify(repo => repo.RelatedFigureRepository.CreateRangeAsync(It.IsAny<List<RelatedFigure>>()), Times.Once);
        }

        [Fact]
        public async Task AddRelatedFigures_WhenFiguresIsNull_DoesNotThrowException()
        {
            // Arrange
            var streetcode = new StreetcodeContent();
            List<RelatedFigureShortDTO> relatedFigures = null;

            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddRelatedFigures", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, relatedFigures });

            // Assert
            await action.Should().NotThrowAsync();
            _repositoryMock.Verify(repo => repo.RelatedFigureRepository.CreateRangeAsync(It.IsAny<List<RelatedFigure>>()), Times.Never);
        }

        [Fact]
        public async Task AddRelatedFigures_WhenFiguresIsEmpty_DoesNotCallRepository()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1 };
            var relatedFigures = new List<RelatedFigureShortDTO>();

            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddRelatedFigures", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, relatedFigures });

            // Assert
            await action.Should().NotThrowAsync();
            _repositoryMock.Verify(repo => repo.RelatedFigureRepository.CreateRangeAsync(It.IsAny<List<RelatedFigure>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenRelatedFiguresIsNull_DoesNotReturnsErrorResult()
        {
            var streetcodeCreateDto = Create();
            streetcodeCreateDto.RelatedFigures = null;
            var request = new CreateStreetcodeCommand(streetcodeCreateDto);
            SetupMockAnErrorOccurredWhileCreatingLocalizer();
            SetupMocksForCreateStreetcode();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _loggerMock.Verify(
                logger => logger.LogError(It.IsAny<object>(), It.Is<string>(msg => msg.Contains("AnErrorOccurredWhileCreating"))),
                Times.Never);

            _repositoryMock.Verify(repo => repo.RelatedFigureRepository.CreateRangeAsync(It.IsAny<List<RelatedFigure>>()), Times.Never);
        }
        #endregion

        #region AddPartners
        [Fact]
        public async Task AddPartners_WhenValidPartners_AddsSuccessfully()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1 };
            var partners = new List<int> { 101, 102, 103 };

            SetupMockAddPartners();

            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddPartners", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, partners });

            // Assert
            await action.Should().NotThrowAsync();
            _repositoryMock.Verify(repo => repo.PartnerStreetcodeRepository.CreateRangeAsync(It.IsAny<List<StreetcodePartner>>()), Times.Once);
        }

        [Fact]
        public async Task AddPartners_WhenPartnersIsNull_DoesNotThrowException()
        {
            // Arrange
            var streetcode = new StreetcodeContent();
            List<int> partners = null;

            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddPartners", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, partners });

            // Assert
            await action.Should().NotThrowAsync();
            _repositoryMock.Verify(repo => repo.PartnerStreetcodeRepository.CreateRangeAsync(It.IsAny<List<StreetcodePartner>>()), Times.Never);
        }

        [Fact]
        public async Task AddPartners_WhenPartnersIsEmpty_DoesNotCallRepository()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1 };
            var partners = new List<int>();

            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddPartners", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, partners });

            // Assert
            await action.Should().NotThrowAsync();
            _repositoryMock.Verify(repo => repo.PartnerStreetcodeRepository.CreateRangeAsync(It.IsAny<List<StreetcodePartner>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenPartnersIsNull_DoesNotReturnsErrorResult()
        {
            // Arrange
            var streetcodeCreateDto = Create();
            streetcodeCreateDto.Partners = null;

            var request = new CreateStreetcodeCommand(streetcodeCreateDto);
            SetupMocksForCreateStreetcode();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _repositoryMock.Verify(repo => repo.PartnerStreetcodeRepository.CreateRangeAsync(It.IsAny<List<StreetcodePartner>>()), Times.Never);
        }

        #endregion

        #region AddToponyms
        [Fact]
        public async Task AddToponyms_WhenValidToponyms_AddsSuccessfully()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1, Toponyms = new List<Toponym>() };
            var toponyms = new List<StreetcodeToponymCreateUpdateDTO>
            {
                new () { StreetName = "Main Street" },
                new () { StreetName = "Broadway" },
            };

            SetupMockAddToponyms();

            MethodInfo methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddToponyms", BindingFlags.NonPublic | BindingFlags.Instance) !;

            Assert.NotNull(methodInfo);

            // Act
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, toponyms }) !;

            // Assert
            await action.Should().NotThrowAsync();
            _repositoryMock.Verify(repo => repo.ToponymRepository.GetAllAsync(It.IsAny<Expression<Func<Toponym, bool>>>(), null), Times.Once);
        }

        [Fact]
        public async Task AddToponyms_WhenToponymsIsNull_DoesNotThrowException()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Toponyms = new List<Toponym>() };
            List<StreetcodeToponymCreateUpdateDTO> toponyms = null!;

            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddToponyms", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, toponyms }) !;

            // Assert
            await action.Should().NotThrowAsync();
            _repositoryMock.Verify(repo => repo.ToponymRepository.GetAllAsync(It.IsAny<Expression<Func<Toponym, bool>>>(), null), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenToponymsIsNull_DoesNotReturnsErrorResult()
        {
            // Arrange
            var streetcodeCreateDto = Create();
            streetcodeCreateDto.Toponyms = null;

            var request = new CreateStreetcodeCommand(streetcodeCreateDto);
            SetupMocksForCreateStreetcode();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _repositoryMock.Verify(repo => repo.ToponymRepository.GetAllAsync(It.IsAny<Expression<Func<Toponym, bool>>>(), null), Times.Never);
        }

        #endregion

        #region AddStatisticRecords
        [Fact]
        public void AddStatisticRecords_WhenValidRecords_AddsSuccessfully()
        {
            // Arrange
            var streetcode = new StreetcodeContent
            {
                Id = 1,
                Coordinates = new List<StreetcodeCoordinate>
                {
                    new () { Latitude = 50.41m, Longtitude = 30.5234m },
                },
                StatisticRecords = new List<StatisticRecord>(),
            };

            var statisticRecords = new List<StatisticRecordDTO>
            {
                new ()
                {
                    StreetcodeCoordinate = new StreetcodeCoordinateDTO { Latitude = 50.41m, Longtitude = 30.5234m },
                    QrId = 1,
                    Count = 5,
                    Address = "Kyiv",
                },
            };

            SetupMockAddStatisticRecords();

            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddStatisticRecords", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            Action action = () => methodInfo.Invoke(_handler, new object[] { streetcode, statisticRecords });

            // Assert
            action.Should().NotThrow();
            Assert.Single(streetcode.StatisticRecords);
        }

        [Fact]
        public void AddStatisticRecords_WhenRecordsIsNull_DoesNotThrowException()
        {
            // Arrange
            var streetcode = new StreetcodeContent { StatisticRecords = new List<StatisticRecord>() };
            List<StatisticRecordDTO> statisticRecords = null;

            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddStatisticRecords", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            Action action = () => methodInfo.Invoke(_handler, new object[] { streetcode, statisticRecords });

            // Assert
            action.Should().NotThrow();
            Assert.Empty(streetcode.StatisticRecords);
        }

        [Fact]
        public void AddStatisticRecords_WhenNoMatchingCoordinates_ThrowsException()
        {
            // Arrange
            var streetcode = new StreetcodeContent
            {
                Id = 1,
                Coordinates = new List<StreetcodeCoordinate> // Інші координати
                {
                    new () { Latitude = 40.7128m, Longtitude = -74.0060m },
                },
                StatisticRecords = new List<StatisticRecord>(),
            };

            var statisticRecords = new List<StatisticRecordDTO>
            {
                new ()
                {
                    StreetcodeCoordinate = new StreetcodeCoordinateDTO { Latitude = 50.4501m, Longtitude = 30.5234m },
                    QrId = 1,
                    Count = 5,
                    Address = "Kyiv",
                },
            };

            SetupMockAddStatisticRecords();

            var methodInfo = typeof(CreateStreetcodeHandler).GetMethod("AddStatisticRecords", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            Action action = () => methodInfo.Invoke(_handler, new object[] { streetcode, statisticRecords });

            // Assert
            action.Should().Throw<TargetInvocationException>()
                .WithInnerException<InvalidOperationException>();
        }

        [Fact]
        public async Task Handle_WhenStatisticRecordsIsNull_DoesNotReturnsErrorResult()
        {
            // Arrange
            var streetcodeCreateDto = Create();
            streetcodeCreateDto.StatisticRecords = null;

            var request = new CreateStreetcodeCommand(streetcodeCreateDto);
            SetupMocksForCreateStreetcode();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        #endregion

        #region AddTransactionLink
        [Fact]
        public void AddTransactionLink_WhenValidUrl_AddsTransactionLink()
        {
            // Arrange
            var streetcode = new StreetcodeContent();
            string url = "https://example.com";

            var methodInfo = typeof(CreateStreetcodeHandler)
                .GetMethod("AddTransactionLink", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            methodInfo.Invoke(_handler, new object[] { streetcode, url });

            // Assert
            Assert.NotNull(streetcode.TransactionLink);
            Assert.Equal(url, streetcode.TransactionLink.Url);
            Assert.Equal(url, streetcode.TransactionLink.UrlTitle);
        }

        [Fact]
        public void AddTransactionLink_WhenUrlIsNull_DoesNotModifyTransactionLink()
        {
            // Arrange
            var streetcode = new StreetcodeContent { TransactionLink = null };
            string? url = null;

            var methodInfo = typeof(CreateStreetcodeHandler)
                .GetMethod("AddTransactionLink", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            methodInfo.Invoke(_handler, new object[] { streetcode, url });

            // Assert
            Assert.Null(streetcode.TransactionLink);
        }

        #endregion

        #region AddFactImageDescription
        [Fact]
        public void AddFactImageDescription_WhenFactsListIsEmpty_DoesNotCallRepository()
        {
            // Arrange
            var facts = new List<FactUpdateCreateDto>();

            _repositoryMock.Setup(repo => repo.ImageDetailsRepository.Create(It.IsAny<ImageDetails>()));

            var methodInfo = typeof(CreateStreetcodeHandler)
                .GetMethod("AddFactImageDescription", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            methodInfo.Invoke(_handler, new object[] { facts });

            // Assert
            _repositoryMock.Verify(repo => repo.ImageDetailsRepository.Create(It.IsAny<ImageDetails>()), Times.Never);
        }

        [Fact]
        public void AddFactImageDescription_WhenFactsIsNull_DoesNotThrowException()
        {
            // Arrange
            List<FactUpdateCreateDto> facts = null!;

            _repositoryMock.Setup(repo => repo.ImageDetailsRepository.Create(It.IsAny<ImageDetails>()));

            var methodInfo = typeof(CreateStreetcodeHandler)
                .GetMethod("AddFactImageDescription", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            Action action = () => methodInfo.Invoke(_handler, new object[] { facts });

            // Assert
            action.Should().NotThrow();

            _repositoryMock.Verify(repo => repo.ImageDetailsRepository.Create(It.IsAny<ImageDetails>()), Times.Never);
        }

        #endregion
        [Fact]
        public async Task Handle_ReturnsSuccess_WhenStreetcodeCreated()
        {
            // Arrange
            var createdStreetcode = Create();
            var request = new CreateStreetcodeCommand(createdStreetcode);

            SetupMocksForCreateStreetcode();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        private void SetupMockAddArtGallery()
        {
            _mapperMock
                .Setup(m => m.Map<List<StreetcodeArtSlide>>(It.IsAny<List<StreetcodeArtSlideCreateUpdateDTO>>()))
                .Returns((List<StreetcodeArtSlideCreateUpdateDTO> src) =>
                    src.Select(s => new StreetcodeArtSlide
                    {
                        Id = s.SlideId,
                        StreetcodeId = s.StreetcodeId ?? 0,
                        Template = s.Template,
                        Index = s.Index,
                    }).ToList());

            _mapperMock
                .Setup(m => m.Map<List<Art>>(It.IsAny<List<ArtCreateUpdateDTO>>()))
                .Returns((List<ArtCreateUpdateDTO> src) =>
                    src.Select(s => new Art()
                    {
                        Id = s.Id,
                        ImageId = s.ImageId,
                        Description = s.Description,
                        Title = s.Title,
                    }).ToList());

            _repositoryMock.Setup(repo => repo.StreetcodeArtSlideRepository.CreateRangeAsync(It.IsAny<List<StreetcodeArtSlide>>()))
                .Returns(Task.CompletedTask);

            _repositoryMock.Setup(repo => repo.ArtRepository.CreateRangeAsync(It.IsAny<List<Art>>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<StreetcodeArt>(It.IsAny<StreetcodeArtCreateUpdateDTO>()))
                .Returns((
                    StreetcodeArtCreateUpdateDTO src) => new StreetcodeArt()
                {
                    Id = src.ArtId,
                });

            _repositoryMock.Setup(repo => repo.StreetcodeArtRepository.CreateRangeAsync(It.IsAny<List<StreetcodeArt>>()))
                .Returns(Task.CompletedTask);
        }

        private void SetupMockAddTimeLineItems()
        {
            _mapperMock
                .Setup(m => m.Map<List<HistoricalContext>>(It.IsAny<List<HistoricalContextCreateUpdateDTO>>()))
                .Returns((List<HistoricalContextCreateUpdateDTO> src) =>
                    src.Select(s => new HistoricalContext()
                    {
                        Id = s.Id,
                        Title = s.Title,
                    }).ToList());

            _repositoryMock.Setup(repo => repo.HistoricalContextRepository.CreateRangeAsync(It.IsAny<IEnumerable<HistoricalContext>>()))
                .Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            _mapperMock
                .Setup(m => m.Map<TimelineItem>(It.IsAny<TimelineItemCreateUpdateDTO>()))
                .Returns((
                    TimelineItemCreateUpdateDTO src) => new TimelineItem()
                {
                    Id = src.Id,
                    HistoricalContextTimelines = new List<HistoricalContextTimeline>(),
                });
        }

        private void SetupMockAddTags(bool tagExist = false)
        {
            _mapperMock
                .Setup(m => m.Map<Tag>(It.IsAny<StreetcodeTagDTO>()))
                .Returns((StreetcodeTagDTO src) => new Tag { Id = 0, Title = src.Title });

            _repositoryMock
                .Setup(repo => repo.TagRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Tag, bool>>>(), null))
                .ReturnsAsync(tagExist ? new Tag { Id = 10, Title = "ExistingTag" } : null);

            _repositoryMock.Setup(repo => repo.StreetcodeTagIndexRepository.CreateRangeAsync(It.IsAny<List<StreetcodeTagIndex>>()))
                .Returns(Task.CompletedTask);
        }

        private void SetupMockAddRelaredFigures()
        {
            _repositoryMock.Setup(repo => repo.RelatedFigureRepository.CreateRangeAsync(It.IsAny<List<DAL.Entities.Streetcode.RelatedFigure>>()))
                .Returns(Task.CompletedTask);
        }

        private void SetupMockAddPartners()
        {
            _repositoryMock.Setup(repo => repo.PartnerStreetcodeRepository.CreateRangeAsync(It.IsAny<List<StreetcodePartner>>()))
                .Returns(Task.CompletedTask);
        }

        private void SetupMockAddToponyms()
        {
            _repositoryMock.Setup(repo => repo.ToponymRepository.GetAllAsync(It.IsAny<Expression<Func<Toponym, bool>>>(), null))
                .ReturnsAsync(new List<Toponym>());
        }

        private void SetupMockStreetcodeImageCreate()
        {
            _repositoryMock.Setup(repo => repo.StreetcodeImageRepository.CreateRangeAsync(It.IsAny<IEnumerable<StreetcodeImage>>()))
                .Returns(Task.CompletedTask);

            _localizerFieldMock
                .Setup(l => l["Images"])
                .Returns(new LocalizedString("Images", "Images"));
        }

        private void SetupMockStreetcodeImageDetailsCreate()
        {
           _localizerFieldMock
                .Setup(l => l["ImagesDetails"])
                .Returns(new LocalizedString("ImagesDetails", "ImagesDetails"));

           _mapperMock
                .Setup(m => m.Map<List<ImageDetails>>(It.IsAny<List<ImageDetailsDto>>()))
                .Returns((List<ImageDetailsDto> src) =>
                    src.Select(s => new ImageDetails()
                    {
                        Id = s.Id,
                    }).ToList());
           _repositoryMock.Setup(repo => repo.ImageDetailsRepository.CreateRangeAsync(It.IsAny<IEnumerable<ImageDetails>>()))
                .Returns(Task.CompletedTask);
        }

        private void SetupMockAddStatisticRecords()
        {
            _mapperMock
                .Setup(m => m.Map<StatisticRecord>(It.IsAny<StatisticRecordDTO>()))
                .Returns((StatisticRecordDTO src) => new StatisticRecord
                {
                    StreetcodeCoordinate = new StreetcodeCoordinate { Latitude = src.StreetcodeCoordinate.Latitude, Longtitude = src.StreetcodeCoordinate.Longtitude },
                    QrId = src.QrId,
                    Count = src.Count,
                    Address = src.Address,
                });
        }

        private void SetupMockAnErrorOccurredWhileCreatingLocalizer()
        {
            _localizerErrorMock
                .Setup(x => x["AnErrorOccurredWhileCreating", It.IsAny<object>()])
                .Returns((string key, object[] args) =>
                    new LocalizedString(key, $"AnErrorOccurredWhileCreating: {args[0]}"));
        }

        private static StreetcodeCreateDTO Create()
        {
            return new StreetcodeCreateDTO()
            {
                Index = 1,
                Teaser = "Test_Teaser",
                DateString = "Test_Date",
                Alias = "Test_Alias",
                Status = StreetcodeStatus.Published,
                Title = "Test_Title",
                TransliterationUrl = "Url",
                ViewCount = 1,
                StreetcodeType = StreetcodeType.Person,
                TimelineItems = new List<TimelineItemCreateUpdateDTO>() { },
                ImagesIds = new List<int>() { 1, 2 },
                StreetcodeArtSlides = new List<StreetcodeArtSlideCreateUpdateDTO>(),
                Arts = new List<ArtCreateUpdateDTO>(),
                Tags = new List<StreetcodeTagDTO>(),
                RelatedFigures = new List<RelatedFigureShortDTO>(),
                Partners = new List<int>(),
                Toponyms = new List<StreetcodeToponymCreateUpdateDTO>(),
                StatisticRecords = new List<StatisticRecordDTO>(),
                Coordinates = new List<StreetcodeCoordinateDTO>(),
                Facts = new List<StreetcodeFactCreateDTO>(),
                ImagesDetails = new List<ImageDetailsDto>()
                {
                    new () { Alt = "1", Title = "TestImage", Id = 1 },
                },
            };
        }

        private void SetupMocksForCreateStreetcode()
        {
            _mapperMock
                .Setup(x => x.Map<StreetcodeContent>(It.IsAny<StreetcodeCreateDTO>()))
                .Returns((StreetcodeCreateDTO src) => new StreetcodeContent()
                {
                    Index = src.Index,
                    Teaser = src.Teaser,
                    DateString = src.DateString,
                    Alias = src.Alias,
                    Status = src.Status,
                    Title = src.Title,
                    TransliterationUrl = src.TransliterationUrl,
                    EventStartOrPersonBirthDate = src.EventStartOrPersonBirthDate,
                });

            _repositoryMock.Setup(r => r.StreetcodeRepository.Create(It.IsAny<StreetcodeContent>()));
            _repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            SetupMockAddTimeLineItems();
            SetupMockStreetcodeImageCreate();
            SetupMockAddArtGallery();
            SetupMockAddTags();
            SetupMockAddRelaredFigures();
            SetupMockAddPartners();
            SetupMockAddToponyms();
            SetupMockStreetcodeImageDetailsCreate();

            _repositoryMock
                .Setup(x => x.BeginTransaction())
                .Returns(new TransactionScope(TransactionScopeOption.Suppress));

            _localizerErrorMock
                .Setup(x => x["AnErrorOccurredWhileCreating", It.IsAny<object>()])
                .Returns((string key, object[] args) =>
                    new LocalizedString(key, $"AnErrorOccurredWhileCreating: {args[0]}"));

            _localizerValidationMock
                .Setup(l => l["CannotBeEmpty", It.IsAny<object[]>()])
                .Returns((string key, object[] args) => new LocalizedString(key, $"Field {args[0]} cannot be empty"));
        }
    }
}
