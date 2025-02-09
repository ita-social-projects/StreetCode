using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using FluentAssertions;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Update;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics.Update;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Partners.Update;
using Streetcode.BLL.DTO.Sources.Update;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Enums;
using Streetcode.BLL.Interfaces.Cache;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Repositories.Interfaces.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class UpdateStreetcodeHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateStreetcodeHandler _handler;

        public UpdateStreetcodeHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryWrapper>();
            _mapperMock = new Mock<IMapper>();
            Mock<ILoggerService> mockLogger = new ();
            MockAnErrorOccurredLocalizer mockLocalizerAnErrorOccurred = new ();
            MockFailedToUpdateLocalizer mockLocalizerFailedToUpdate = new ();
            MockFailedToValidateLocalizer mockStringLocalizerFailedToValidate = new ();
            MockFieldNamesLocalizer mockStringLocalizerFieldNames = new ();
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new ();
            Mock<ICacheService> mockCache = new ();
            mockCache
                .Setup(c => c.GetOrSetAsync(It.IsAny<string>(), It.IsAny<Func<Task<Result<IEnumerable<ImageDTO>>>>>(), It.IsAny<TimeSpan>()))
                .Returns<string, Func<Task<Result<IEnumerable<ImageDTO>>>>, TimeSpan>((_, func, _) => func());
            _handler = new UpdateStreetcodeHandler(
               _mapperMock.Object,
               _repositoryMock.Object,
               mockLogger.Object,
               mockLocalizerAnErrorOccurred,
               mockLocalizerFailedToUpdate,
               mockStringLocalizerFailedToValidate,
               mockStringLocalizerFieldNames,
               mockCache.Object,
               mockHttpContextAccessor.Object);
        }

        [Fact]
        public async Task UpdateEntitiesAsync_WhenUpdatingStatisticRecords_CallsRepositoryMethods()
        {
            // Arrange
            var statisticRecords = new List<StatisticRecordUpdateDTO>
            {
                new ()
                {
                    Id = 1, QrId = 101, Count = 5, Address = "Kyiv", StreetcodeId = 1,
                    StreetcodeCoordinate = new StreetcodeCoordinateUpdateDTO { Latitude = 50.4501m, Longtitude = 30.5234m },
                    ModelState = ModelState.Updated,
                },
                new ()
                {
                    Id = 2, QrId = 102, Count = 10, Address = "Lviv", StreetcodeId = 1,
                    StreetcodeCoordinate = new StreetcodeCoordinateUpdateDTO { Latitude = 49.8397m, Longtitude = 24.0297m },
                    ModelState = ModelState.Created,
                },
                new ()
                {
                    Id = 3, QrId = 103, Count = 7, Address = "Odesa", StreetcodeId = 1,
                    StreetcodeCoordinate = new StreetcodeCoordinateUpdateDTO { Latitude = 46.4825m, Longtitude = 30.7233m },
                    ModelState = ModelState.Deleted,
                },
            };

            var repositoryMock = new Mock<IRepositoryBase<StatisticRecord>>();
            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateEntitiesAsync", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.MakeGenericMethod(typeof(StatisticRecord), typeof(StatisticRecordUpdateDTO));
            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { statisticRecords, repositoryMock.Object }) !;

            // Assert
            repositoryMock.Verify(repo => repo.UpdateRange(It.IsAny<IEnumerable<StatisticRecord>>()), Times.Once);
            repositoryMock.Verify(repo => repo.CreateRangeAsync(It.IsAny<IEnumerable<StatisticRecord>>()), Times.Once);
            repositoryMock.Verify(repo => repo.DeleteRange(It.IsAny<IEnumerable<StatisticRecord>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateEntitiesAsync_WhenUpdatingStreetcodeCategoryContent_CallsRepositoryMethods()
        {
            // Arrange
            var categoryContents = new List<StreetcodeCategoryContentUpdateDTO>
            {
                new () { Text = "Updated text", ModelState = ModelState.Updated },
                new () { Text = "New content", ModelState = ModelState.Created },
                new () { Text = "Deleted content", ModelState = ModelState.Deleted },
            };

            var repositoryMock = new Mock<IRepositoryBase<StreetcodeCategoryContent>>();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateEntitiesAsync", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.MakeGenericMethod(typeof(StreetcodeCategoryContent), typeof(StreetcodeCategoryContentUpdateDTO));

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { categoryContents, repositoryMock.Object }) !;

            // Assert
            repositoryMock.Verify(repo => repo.UpdateRange(It.IsAny<IEnumerable<StreetcodeCategoryContent>>()), Times.Once);
            repositoryMock.Verify(repo => repo.CreateRangeAsync(It.IsAny<IEnumerable<StreetcodeCategoryContent>>()), Times.Once);
            repositoryMock.Verify(repo => repo.DeleteRange(It.IsAny<IEnumerable<StreetcodeCategoryContent>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateEntitiesAsync_WhenUpdatingRelatedFigures_CallsRepositoryMethods()
        {
            // Arrange
            var relatedFigures = new List<RelatedFigureUpdateDTO>
            {
                new () { ObserverId = 1, TargetId = 2, ModelState = ModelState.Updated },
                new () { ObserverId = 3, TargetId = 4, ModelState = ModelState.Created },
                new () { ObserverId = 5, TargetId = 6, ModelState = ModelState.Deleted },
            };

            var repositoryMock = new Mock<IRepositoryBase<RelatedFigure>>();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateEntitiesAsync", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.MakeGenericMethod(typeof(RelatedFigure), typeof(RelatedFigureUpdateDTO));

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { relatedFigures, repositoryMock.Object }) !;

            // Assert
            repositoryMock.Verify(repo => repo.UpdateRange(It.IsAny<IEnumerable<RelatedFigure>>()), Times.Once);
            repositoryMock.Verify(repo => repo.CreateRangeAsync(It.IsAny<IEnumerable<RelatedFigure>>()), Times.Once);
            repositoryMock.Verify(repo => repo.DeleteRange(It.IsAny<IEnumerable<RelatedFigure>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateEntitiesAsync_WhenUpdatingPartners_CallsRepositoryMethods()
        {
            // Arrange
            var partners = new List<PartnersUpdateDTO>
            {
                new () { StreetcodeId = 1, PartnerId = 10, ModelState = ModelState.Updated },
                new () { StreetcodeId = 2, PartnerId = 20, ModelState = ModelState.Created },
                new () { StreetcodeId = 3, PartnerId = 30, ModelState = ModelState.Deleted },
            };

            var repositoryMock = new Mock<IRepositoryBase<StreetcodePartner>>();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateEntitiesAsync", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.MakeGenericMethod(typeof(StreetcodePartner), typeof(PartnersUpdateDTO));

            Assert.NotNull(methodInfo);

            // Act
            await ((Task)methodInfo.Invoke(_handler, new object[] { partners, repositoryMock.Object }) !);

            // Assert
            repositoryMock.Verify(repo => repo.UpdateRange(It.IsAny<IEnumerable<StreetcodePartner>>()), Times.Once);
            repositoryMock.Verify(repo => repo.CreateRangeAsync(It.IsAny<IEnumerable<StreetcodePartner>>()), Times.Once);
            repositoryMock.Verify(repo => repo.DeleteRange(It.IsAny<IEnumerable<StreetcodePartner>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTags_WhenNewTags_CreatesSuccessfully()
        {
            // Arrange
            var tags = new List<StreetcodeTagUpdateDTO>
            {
                new () { Id = 0, Title = "NewTag", ModelState = ModelState.Created },
            };

            SetupMockUpdateTags(tagExists: false);

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateTags", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { tags }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.StreetcodeTagIndexRepository.CreateRangeAsync(It.IsAny<IEnumerable<StreetcodeTagIndex>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTags_WhenTagAlreadyExists_ThrowsException()
        {
            // Arrange
            var tags = new List<StreetcodeTagUpdateDTO>
            {
                new () { Id = 0, Title = "ExistingTag", ModelState = ModelState.Created },
            };

            SetupMockUpdateTags(tagExists: true);

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateTags", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            Func<Task> action = async () => await (Task)methodInfo.Invoke(_handler, new object[] { tags }) !;

            // Assert
            await action.Should().ThrowAsync<HttpRequestException>();
        }

        [Fact]
        public async Task UpdateTags_WhenTagsUpdated_CallsUpdateRange()
        {
            // Arrange
            var tags = new List<StreetcodeTagUpdateDTO>
            {
                new () { Id = 5, Title = "UpdatedTag", ModelState = ModelState.Updated },
            };

            SetupMockUpdateTags();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateTags", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { tags }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.StreetcodeTagIndexRepository.UpdateRange(It.IsAny<IEnumerable<StreetcodeTagIndex>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTags_WhenTagsDeleted_CallsDeleteRange()
        {
            // Arrange
            var tags = new List<StreetcodeTagUpdateDTO>
            {
                new () { Id = 5, Title = "DeletedTag", ModelState = ModelState.Deleted },
            };

            SetupMockUpdateTags();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateTags", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { tags }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.StreetcodeTagIndexRepository.DeleteRange(It.IsAny<IEnumerable<StreetcodeTagIndex>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateStreetcodeToponymAsync_WhenCreatingToponyms_AddsToStreetcodeContent()
        {
            // Arrange
            var streetcodeContent = new StreetcodeContent { Id = 10, Toponyms = new List<Toponym>() };
            var toponyms = new List<StreetcodeToponymCreateUpdateDTO>
            {
                new () { StreetName = "New Street", ModelState = ModelState.Created },
            };

            SetupMockUpdateToponyms(hasToponymsToCreate: true);

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateStreetcodeToponymAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { streetcodeContent, toponyms }) !;

            // Assert
            Assert.Single(streetcodeContent.Toponyms);
            Assert.Equal("New Street", streetcodeContent.Toponyms.First().StreetName);
        }

        [Fact]
        public async Task UpdateStreetcodeToponymAsync_WhenDeletingToponyms_CallsExecuteSqlRaw()
        {
            // Arrange
            var toponyms = new List<StreetcodeToponymCreateUpdateDTO>
            {
                new () { StreetName = "Old Street", ModelState = ModelState.Deleted },
            };
            var streetcodeContent = new StreetcodeContent { Id = 10, Toponyms = new List<Toponym>() };

            SetupMockUpdateToponyms();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateStreetcodeToponymAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { streetcodeContent, toponyms }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.ToponymRepository.ExecuteSqlRaw(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTimelineItemsAsync_WhenNewHistoricalContexts_CallsCreateRangeAsync()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1, TimelineItems = new List<TimelineItem>() };
            var timelineItems = new List<TimelineItemCreateUpdateDTO>
            {
                new ()
                {
                    Id = 1,
                    Title = "Event 1",
                    HistoricalContexts = new List<HistoricalContextCreateUpdateDTO>
                    {
                        new () { Id = 0, Title = "New Context" },
                    },
                    ModelState = ModelState.Created,
                },
            };

            SetupMockUpdateTimelineItems();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateTimelineItemsAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, timelineItems }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.HistoricalContextRepository.CreateRangeAsync(It.IsAny<IEnumerable<HistoricalContext>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTimelineItemsAsync_WhenDeletingTimelineItem_CallsDeleteRange()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1 };
            var timelineItems = new List<TimelineItemCreateUpdateDTO>
            {
                new () { Id = 3, Title = "Deleted Event", ModelState = ModelState.Deleted },
            };

            SetupMockUpdateTimelineItems();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateTimelineItemsAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, timelineItems }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.TimelineRepository.DeleteRange(It.IsAny<IEnumerable<TimelineItem>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTimelineItemsAsync_WhenUpdatingTimelineItem_CallsHistoricalContextCreateRangeAsynce()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1, TimelineItems = new List<TimelineItem>() };
            var timelineItems = new List<TimelineItemCreateUpdateDTO>
            {
                new ()
                {
                    Id = 4,
                    Title = "Updated Event",
                    ModelState = ModelState.Updated,
                    HistoricalContexts = new List<HistoricalContextCreateUpdateDTO>()
                    {
                        new () { Id = 1, TimelineId = 4, ModelState = ModelState.Created },
                        new () { Id = 2, TimelineId = 3, ModelState = ModelState.Deleted },
                    },
                },
            };

            SetupMockUpdateTimelineItems();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateTimelineItemsAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, timelineItems }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.HistoricalContextTimelineRepository.CreateRangeAsync(It.IsAny<List<HistoricalContextTimeline>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTimelineItemsAsync_WhenUpdatingTimelineItem_CallsHistoricalContextDeleteRangeAsynce()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1, TimelineItems = new List<TimelineItem>() };
            var timelineItems = new List<TimelineItemCreateUpdateDTO>
            {
                new ()
                {
                    Id = 4,
                    Title = "Updated Event",
                    ModelState = ModelState.Updated,
                    HistoricalContexts = new List<HistoricalContextCreateUpdateDTO>()
                    {
                        new () { Id = 2, TimelineId = 3, ModelState = ModelState.Deleted },
                    },
                },
            };

            SetupMockUpdateTimelineItems();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateTimelineItemsAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, timelineItems }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.HistoricalContextTimelineRepository.DeleteRange(It.IsAny<List<HistoricalContextTimeline>>()), Times.Once);
        }

        [Fact]
        public void UpdateAudio_WhenCreatingNewAudio_UpdatesAudioId()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1 };
            var audios = new List<AudioUpdateDTO>
            {
                new () { Id = 100, ModelState = ModelState.Created },
            };

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateAudio", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            methodInfo.Invoke(_handler, new object[] { audios, streetcode });

            // Assert
            Assert.Equal(100, streetcode.AudioId);
        }

        [Fact]
        public void UpdateAudio_WhenUpdatingAudio_UpdatesAudioId()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1 };
            var audios = new List<AudioUpdateDTO>
            {
                new () { Id = 200, ModelState = ModelState.Updated },
            };

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateAudio", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            methodInfo.Invoke(_handler, new object[] { audios, streetcode });

            // Assert
            Assert.Equal(200, streetcode.AudioId);
        }

        [Fact]
        public async Task UpdateArtGallery_WhenDeletingOldStreetcodeArt_CallsDeleteRange()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1, StreetcodeArtSlides = new List<StreetcodeArtSlide>() };
            var artSlides = new List<StreetcodeArtSlideCreateUpdateDTO>();
            var arts = new List<ArtCreateUpdateDTO>();

            SetupMockUpdateArtGallery();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateArtGallery", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, artSlides, arts }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.StreetcodeArtRepository.DeleteRange(It.IsAny<IEnumerable<StreetcodeArt>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateArtGallery_WhenCreatingNewArts_CallsCreateRange()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1, StreetcodeArtSlides = new List<StreetcodeArtSlide>() };
            var artSlides = new List<StreetcodeArtSlideCreateUpdateDTO>
            {
                new () { SlideId = 10, StreetcodeArts = new List<StreetcodeArtCreateUpdateDTO> { new () { ArtId = 101 } } },
            };
            var arts = new List<ArtCreateUpdateDTO>
            {
                new () { Id = 101, ModelState = ModelState.Created },
            };

            SetupMockUpdateArtGallery();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateArtGallery", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, artSlides, arts }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.ArtRepository.CreateRangeAsync(It.IsAny<IEnumerable<Art>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateArtGallery_WhenDeletingUnusedArts_CallsDeleteRange()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1, StreetcodeArtSlides = new List<StreetcodeArtSlide>() };
            var artSlides = new List<StreetcodeArtSlideCreateUpdateDTO>
            {
                new () { SlideId = 20, StreetcodeArts = new List<StreetcodeArtCreateUpdateDTO> { new () { ArtId = 200 } } },
            };
            var arts = new List<ArtCreateUpdateDTO>
            {
                new () { Id = 300, ModelState = ModelState.Updated }, // Цей не використовується, інший Id
            };

            SetupMockUpdateArtGallery();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateArtGallery", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, artSlides, arts }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.ArtRepository.DeleteRange(It.IsAny<IEnumerable<Art>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateArtGallery_WhenUpdatingExistingArts_CallsUpdateRange()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1, StreetcodeArtSlides = new List<StreetcodeArtSlide>() };
            var artSlides = new List<StreetcodeArtSlideCreateUpdateDTO>
            {
                new () { SlideId = 30, StreetcodeArts = new List<StreetcodeArtCreateUpdateDTO> { new () { ArtId = 301 } } },
            };
            var arts = new List<ArtCreateUpdateDTO>
            {
                new () { Id = 301, ModelState = ModelState.Updated },
            };

            SetupMockUpdateArtGallery();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateArtGallery", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, artSlides, arts }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.ArtRepository.UpdateRange(It.IsAny<IEnumerable<Art>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateArtGallery_WhenCreatingNewArtSlides_CallsCreateAsync()
        {
            // Arrange
            var streetcode = new StreetcodeContent { Id = 1, StreetcodeArtSlides = new List<StreetcodeArtSlide>() };
            var artSlides = new List<StreetcodeArtSlideCreateUpdateDTO>
            {
                new () { SlideId = 40, ModelState = ModelState.Created },
            };
            var arts = new List<ArtCreateUpdateDTO>();

            SetupMockUpdateArtGallery();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateArtGallery", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { streetcode, artSlides, arts }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.StreetcodeArtSlideRepository.CreateAsync(It.IsAny<StreetcodeArtSlide>()), Times.Once);
        }

        [Fact]
        public void GetStreetcodeArtsWithNewArtsId_WhenArtIdExistsInMap_UpdatesArtId()
        {
            // Arrange
            var streetcodeId = 1;
            var artIdMap = new Dictionary<int, int>
            {
                { 100, 200 }, // ArtId 100 буде змінено на 200
            };

            var streetcodeArtSlide = new StreetcodeArtSlideCreateUpdateDTO
            {
                StreetcodeArts = new List<StreetcodeArtCreateUpdateDTO>
                {
                    new () { ArtId = 100 },
                },
            };

            SetupMockGetStreetcodeArtsWithNewArtsId();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("GetStreetcodeArtsWithNewArtsId", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            var result = (List<StreetcodeArt>)methodInfo.Invoke(_handler, new object[] { streetcodeId, artIdMap, streetcodeArtSlide }) !;

            // Assert
            Assert.Single(result);
            Assert.Equal(200, result[0].ArtId);
        }

        [Fact]
        public void GetStreetcodeArtsWithNewArtsId_WhenArtIdNotInMap_KeepsOriginalArtId()
        {
            // Arrange
            var streetcodeId = 1;
            var artIdMap = new Dictionary<int, int>();

            var streetcodeArtSlide = new StreetcodeArtSlideCreateUpdateDTO
            {
                StreetcodeArts = new List<StreetcodeArtCreateUpdateDTO>
                {
                    new () { ArtId = 300 },
                },
            };

            SetupMockGetStreetcodeArtsWithNewArtsId();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("GetStreetcodeArtsWithNewArtsId", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            var result = (List<StreetcodeArt>)methodInfo.Invoke(_handler, new object[] { streetcodeId, artIdMap, streetcodeArtSlide }) !;

            // Assert
            Assert.Single(result);
            Assert.Equal(300, result[0].ArtId);
        }

        [Fact]
        public void GetStreetcodeArtsWithNewArtsId_CreatesStreetcodeArtWithCorrectStreetcodeId()
        {
            // Arrange
            var streetcodeId = 5;
            var artIdMap = new Dictionary<int, int>();

            var streetcodeArtSlide = new StreetcodeArtSlideCreateUpdateDTO
            {
                StreetcodeArts = new List<StreetcodeArtCreateUpdateDTO>
                {
                    new () { ArtId = 400 },
                },
            };

            SetupMockGetStreetcodeArtsWithNewArtsId();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("GetStreetcodeArtsWithNewArtsId", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            var result = (List<StreetcodeArt>)methodInfo.Invoke(_handler, new object[] { streetcodeId, artIdMap, streetcodeArtSlide }) !;

            // Assert
            Assert.Single(result);
            Assert.Equal(5, result[0].StreetcodeId);
        }

        [Fact]
        public void DistributeArtSlide_WhenDeleted_AddsToDeleteSlides()
        {
            // Arrange
            var artSlideDto = new StreetcodeArtSlideCreateUpdateDTO { ModelState = ModelState.Deleted };
            var artSlide = new StreetcodeArtSlide();
            var newStreetcodeArts = new List<StreetcodeArt>();

            StreetcodeArtSlide? toCreateSlide = null;
            var toUpdateSlides = new List<StreetcodeArtSlide>();
            var toDeleteSlides = new List<StreetcodeArtSlide>();
            var toCreateStreetcodeArts = new List<StreetcodeArt>();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("DistributeArtSlide", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(methodInfo);

            // Act
            methodInfo.Invoke(_handler, new object[] { artSlideDto, artSlide, newStreetcodeArts, toCreateSlide!, toUpdateSlides, toDeleteSlides, toCreateStreetcodeArts });

            // Assert
            Assert.Single(toDeleteSlides);
            Assert.Contains(artSlide, toDeleteSlides);
        }

        [Fact]
        public void DistributeArtSlide_WhenUpdated_AddsToUpdateSlidesAndCreatesStreetcodeArts()
        {
            // Arrange
            var artSlideDto = new StreetcodeArtSlideCreateUpdateDTO
            {
                SlideId = 10,
                ModelState = ModelState.Updated,
            };

            var artSlide = new StreetcodeArtSlide();
            var newStreetcodeArts = new List<StreetcodeArt>
            {
                new () { ArtId = 1 },
                new () { ArtId = 2 },
            };

            StreetcodeArtSlide? toCreateSlide = null;
            var toUpdateSlides = new List<StreetcodeArtSlide>();
            var toDeleteSlides = new List<StreetcodeArtSlide>();
            var toCreateStreetcodeArts = new List<StreetcodeArt>();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("DistributeArtSlide", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(methodInfo);

            // Act
            methodInfo.Invoke(_handler, new object[] { artSlideDto, artSlide, newStreetcodeArts, toCreateSlide!, toUpdateSlides, toDeleteSlides, toCreateStreetcodeArts });

            // Assert
            Assert.Single(toUpdateSlides);
            Assert.Contains(artSlide, toUpdateSlides);

            Assert.Equal(2, toCreateStreetcodeArts.Count);
            Assert.All(toCreateStreetcodeArts, art => Assert.Equal(artSlideDto.SlideId, art.StreetcodeArtSlideId));
        }

        [Fact]
        public void DistributeArtSlide_WhenCreated_SetsToCreateSlide()
        {
            // Arrange
            var artSlideDto = new StreetcodeArtSlideCreateUpdateDTO { ModelState = ModelState.Created };
            var artSlide = new StreetcodeArtSlide();
            var newStreetcodeArts = new List<StreetcodeArt>();

            StreetcodeArtSlide? toCreateSlide = null;
            var toUpdateSlides = new List<StreetcodeArtSlide>();
            var toDeleteSlides = new List<StreetcodeArtSlide>();
            var toCreateStreetcodeArts = new List<StreetcodeArt>();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("DistributeArtSlide", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(methodInfo);

            var parameters = new object?[] { artSlideDto, artSlide, newStreetcodeArts, toCreateSlide, toUpdateSlides, toDeleteSlides, toCreateStreetcodeArts };

            // Act
            methodInfo.Invoke(_handler, parameters);
            toCreateSlide = (StreetcodeArtSlide?)parameters[3];

            // Assert
            Assert.NotNull(toCreateSlide);
            Assert.Equal(artSlide, toCreateSlide);
        }

        [Fact]
        public async Task UpdateImagesAsync_WhenDeletingImages_CallsDeleteRange()
        {
            // Arrange
            var images = new List<ImageUpdateDTO>
            {
                new () { Id = 1, ModelState = ModelState.Deleted },
                new () { Id = 2, ModelState = ModelState.Deleted },
            };

            SetupMockUpdateImages();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateImagesAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { images }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.ImageRepository.DeleteRange(It.IsAny<IEnumerable<Image>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateImagesAsync_WhenCreatingNewImages_CallsCreateRangeAsync()
        {
            // Arrange
            var images = new List<ImageUpdateDTO>
            {
                new () { Id = 3, ModelState = ModelState.Created },
                new () { Id = 4, ModelState = ModelState.Created },
            };

            SetupMockUpdateImages();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateImagesAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { images }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.StreetcodeImageRepository.CreateRangeAsync(It.IsAny<IEnumerable<StreetcodeImage>>()), Times.Once);
        }

        [Fact]
        public void UpdateTransactionLink_WhenUrlIsNull_CreatesEmptyTransactionLink()
        {
            // Arrange
            var streetcode = new StreetcodeContent { TransactionLink = null };
            string? url = null;

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateTransactionLink", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(methodInfo);

            // Act
            methodInfo.Invoke(_handler, new object[] { streetcode, url! });

            // Assert
            Assert.NotNull(streetcode.TransactionLink);
            Assert.Equal(string.Empty, streetcode.TransactionLink.Url);
            Assert.Equal(string.Empty, streetcode.TransactionLink.UrlTitle);
        }

        [Fact]
        public void UpdateTransactionLink_WhenUrlIsNotNull_DoesNotChangeTransactionLink()
        {
            // Arrange
            var streetcode = new StreetcodeContent
            {
                TransactionLink = new TransactionLink { Url = "https://example.com", UrlTitle = "Example" },
            };
            string url = "https://new-url.com";

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateTransactionLink", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(methodInfo);

            // Act
            methodInfo.Invoke(_handler, new object[] { streetcode, url });

            // Assert
            Assert.NotNull(streetcode.TransactionLink);
            Assert.Equal("https://example.com", streetcode.TransactionLink.Url);
            Assert.Equal("Example", streetcode.TransactionLink.UrlTitle);
        }

        [Fact]
        public async Task UpdateFactsDescription_WhenAltIsEmpty_DeletesImageDetails()
        {
            // Arrange
            var imageDetails = new List<ImageDetailsDto>
            {
                new () { Id = 1, Alt = string.Empty },
                new () { Id = 2, Alt = string.Empty },
            };

            SetupMockUpdateFactsDescription();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateFactsDescription", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { imageDetails }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.ImageDetailsRepository.DeleteRange(It.IsAny<IEnumerable<ImageDetails>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateFactsDescription_WhenAltIsNotEmpty_UpdatesImageDetails()
        {
            // Arrange
            var imageDetails = new List<ImageDetailsDto>
            {
                new () { Id = 3, Alt = "1" },
                new () { Id = 4, Alt = "0" },
            };

            SetupMockUpdateFactsDescription();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateFactsDescription", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { imageDetails }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.ImageDetailsRepository.UpdateRange(It.IsAny<IEnumerable<ImageDetails>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateFactsDescription_WhenNull_NothingIsCalled()
        {
            // Arrange
            IEnumerable<ImageDetailsDto>? imageDetails = null;

            SetupMockUpdateFactsDescription();

            var methodInfo = typeof(UpdateStreetcodeHandler)
                .GetMethod("UpdateFactsDescription", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(methodInfo);

            // Act
            await (Task)methodInfo.Invoke(_handler, new object[] { imageDetails! }) !;

            // Assert
            _repositoryMock.Verify(repo => repo.ImageDetailsRepository.DeleteRange(It.IsAny<IEnumerable<ImageDetails>>()), Times.Never);
            _repositoryMock.Verify(repo => repo.ImageDetailsRepository.UpdateRange(It.IsAny<IEnumerable<ImageDetails>>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        private void SetupMockUpdateTags(bool tagExists = false)
        {
            _repositoryMock
                .Setup(repo => repo.TagRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Tag, bool>>>(), null))
                .ReturnsAsync(tagExists ? new Tag { Id = 100, Title = "ExistingTag" } : null);

            _repositoryMock.Setup(x => x.StreetcodeTagIndexRepository)
                .Returns(new Mock<IStreetcodeTagIndexRepository>().Object);
        }

        private void SetupMockUpdateToponyms(bool hasToponymsToCreate = false)
        {
            _repositoryMock
                .Setup(repo => repo.ToponymRepository.ExecuteSqlRaw(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _repositoryMock
                .Setup(repo => repo.ToponymRepository.GetAllAsync(It.IsAny<Expression<Func<Toponym, bool>>>(), null))
                .ReturnsAsync(hasToponymsToCreate ? new List<Toponym> { new () { Id = 1, StreetName = "New Street" } } : new List<Toponym>());
        }

        private void SetupMockUpdateTimelineItems()
        {
            _mapperMock
                .Setup(m => m.Map<IEnumerable<HistoricalContext>>(It.IsAny<IEnumerable<HistoricalContextCreateUpdateDTO>>()))
                .Returns((IEnumerable<HistoricalContextCreateUpdateDTO> src) =>
                    src.Select(s => new HistoricalContext()
                    {
                        Id = s.Id,
                        Title = s.Title,
                    }).ToList());

            _mapperMock
                .Setup(m => m.Map<TimelineItem>(It.IsAny<TimelineItemCreateUpdateDTO>()))
                .Returns((
                    TimelineItemCreateUpdateDTO src) => new TimelineItem()
                {
                    Id = src.Id,
                    HistoricalContextTimelines = new List<HistoricalContextTimeline>(),
                });

            _repositoryMock
                .Setup(repo => repo.HistoricalContextRepository.CreateRangeAsync(It.IsAny<IEnumerable<HistoricalContext>>()))
                .Returns(Task.CompletedTask);

            _repositoryMock
                .Setup(repo => repo.HistoricalContextTimelineRepository.CreateRangeAsync(It.IsAny<IEnumerable<HistoricalContextTimeline>>()))
                .Returns(Task.CompletedTask);

            _repositoryMock.Setup(repo => repo.TimelineRepository.DeleteRange(It.IsAny<IEnumerable<TimelineItem>>()));

            _repositoryMock
                .Setup(repo => repo.TimelineRepository.UpdateRange(It.IsAny<IEnumerable<TimelineItem>>()));
        }

        private void SetupMockUpdateArtGallery()
        {
            _mapperMock
                .Setup(m => m.Map<StreetcodeArtSlide>(It.IsAny<StreetcodeArtSlideCreateUpdateDTO>()))
                .Returns((StreetcodeArtSlideCreateUpdateDTO src) => new StreetcodeArtSlide
                    {
                        Id = src.SlideId,
                        StreetcodeId = src.StreetcodeId ?? 0,
                        Template = src.Template,
                        Index = src.Index,
                    });

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

            _mapperMock
                .Setup(m => m.Map<Art>(It.IsAny<ArtCreateUpdateDTO>()))
                .Returns((ArtCreateUpdateDTO src) => new Art()
                    {
                        Id = src.Id,
                        ImageId = src.ImageId,
                        Description = src.Description,
                        Title = src.Title,
                    });

            _mapperMock
                .Setup(m => m.Map<StreetcodeArt>(It.IsAny<StreetcodeArtCreateUpdateDTO>()))
                .Returns((StreetcodeArtCreateUpdateDTO src) => new StreetcodeArt()
                {
                    Id = src.ArtId,
                });

            _repositoryMock.Setup(repo => repo.StreetcodeArtSlideRepository.CreateRangeAsync(It.IsAny<List<StreetcodeArtSlide>>()))
                .Returns(Task.CompletedTask);

            _repositoryMock.Setup(repo => repo.ArtRepository.CreateRangeAsync(It.IsAny<List<Art>>()))
                .Returns(Task.CompletedTask);

            _repositoryMock.Setup(repo => repo.StreetcodeArtRepository.CreateRangeAsync(It.IsAny<List<StreetcodeArt>>()))
                .Returns(Task.CompletedTask);
        }

        private void SetupMockGetStreetcodeArtsWithNewArtsId()
        {
            _mapperMock
                .Setup(m => m.Map<StreetcodeArt>(It.IsAny<StreetcodeArtCreateUpdateDTO>()))
                .Returns((StreetcodeArtCreateUpdateDTO src) => new StreetcodeArt { ArtId = src.ArtId });
        }

        private void SetupMockUpdateImages()
        {
            _mapperMock
                .Setup(m => m.Map<IEnumerable<Image>>(It.IsAny<IEnumerable<ImageUpdateDTO>>()))
                .Returns((IEnumerable<ImageUpdateDTO> src) =>
                    src.Select(img => new Image { Id = img.Id }).ToList());

            _mapperMock
                .Setup(m => m.Map<IEnumerable<StreetcodeImage>>(It.IsAny<IEnumerable<ImageUpdateDTO>>()))
                .Returns((IEnumerable<ImageUpdateDTO> src) =>
                    src.Select(img => new StreetcodeImage { ImageId = img.Id }).ToList());

            _repositoryMock
                .Setup(repo => repo.ImageRepository.DeleteRange(It.IsAny<IEnumerable<Image>>()));

            _repositoryMock
                .Setup(repo => repo.StreetcodeImageRepository.CreateRangeAsync(It.IsAny<IEnumerable<StreetcodeImage>>()))
                .Returns(Task.CompletedTask);
        }

        private void SetupMockUpdateFactsDescription()
        {
            _mapperMock
                .Setup(m => m.Map<IEnumerable<ImageDetails>>(It.IsAny<IEnumerable<ImageDetailsDto>>()))
                .Returns((IEnumerable<ImageDetailsDto> src) =>
                    src.Select(d => new ImageDetails { Id = d.Id, Alt = d.Alt }).ToList());

            _repositoryMock.Setup(repo => repo.ImageDetailsRepository.DeleteRange(It.IsAny<IEnumerable<ImageDetails>>()));

            _repositoryMock.Setup(repo => repo.ImageDetailsRepository.UpdateRange(It.IsAny<IEnumerable<ImageDetails>>()));
        }
    }
}