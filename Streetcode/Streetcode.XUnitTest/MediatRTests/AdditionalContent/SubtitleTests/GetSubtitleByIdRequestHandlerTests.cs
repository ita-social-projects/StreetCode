using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.MediatR.AdditionalContent.GetById;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetById;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.SubtitleTests
{
    public class GetTagByIdRequestHandlerTests
    {
        [Theory]
        [InlineData(1)]
        public async Task GetSubtitlesById_ReturnElement(int id)
        {
            //Arrange
            var mockRepo = new Mock<IRepositoryWrapper>();

            mockRepo.Setup(repo => repo.SubtitleRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Subtitle, bool>>>(),
                It.IsAny<Func<IQueryable<Subtitle>,
                IIncludableQueryable<Subtitle, object>>>()))
                .ReturnsAsync(new Subtitle { Id = 1 });

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<SubtitleDTO>(It.IsAny<Subtitle>()))
                .Returns((Subtitle source) =>
                {
                    return new SubtitleDTO
                    {
                        Id = source.Id
                    };
                });

            var handler = new GetSubtitleByIdHandler(mockRepo.Object, mockMapper.Object);
            
            //Act
            var result = await handler.Handle(new GetSubtitleByIdQuery(id), CancellationToken.None);

            //Assert

            Assert.NotNull(result.Value);

            Assert.IsType<SubtitleDTO>(result.Value);

            Assert.True(result.Value.Id.Equals(id));
        }

        [Theory]
        [InlineData(-1)]
        public async Task GetSubtitlesById_ReturnsNoElements(int id)
        {
            //Arrange
            var mockRepo = new Mock<IRepositoryWrapper>();

            mockRepo.Setup(repo => repo.SubtitleRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<Subtitle, bool>>>(),
            It.IsAny<Func<IQueryable<Subtitle>,
            IIncludableQueryable<Subtitle, object>>>()))
            .ReturnsAsync(new Subtitle()); //returns default

            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(x => x.Map<SubtitleDTO>(It.IsAny<Subtitle>()))
                .Returns((Subtitle source) =>
                {
                    return new SubtitleDTO
                    {
                        Id = source.Id
                    };
                });

            var handler = new GetSubtitleByIdHandler(mockRepo.Object, mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetSubtitleByIdQuery(id), CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.Value.Id.Equals(id));
        }
    }
}
