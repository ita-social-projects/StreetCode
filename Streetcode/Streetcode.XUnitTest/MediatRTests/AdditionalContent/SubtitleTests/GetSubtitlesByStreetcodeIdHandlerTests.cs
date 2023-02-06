using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetByStreetcodeId;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.SubtitleTests
{
    public class GetTagByStreetcodeIdRequestHandlerTests
    {
        [Theory]
        [InlineData(1)]
        public async Task GetSubtitlesByStreetcodeId_ReturnsList(int streetcode_id)
        {
            //Arrange
            var subtitles = new List<Subtitle>()
            {
                new Subtitle
                {
                    Id = 1,
                    Status = SubtitleStatus.Editor,
                    FirstName = "Anatoliy",
                    LastName = "Chupasalenko",
                    Description = "description",
                    Title = "title",
                    Url = "url",
                    StreetcodeId = 1
                },
                new Subtitle
                {
                    Id = 2,
                    Status = SubtitleStatus.Illustrator,
                    FirstName = "Regina",
                    LastName = "Phalange",
                    Url = "url",
                    StreetcodeId = 1
                }
            };

            var mockRepo = new Mock<IRepositoryWrapper>();

            mockRepo.Setup(repo => repo.SubtitleRepository.GetAllAsync(
                It.IsAny<Expression<Func<Subtitle, bool>>>(),
                It.IsAny<Func<IQueryable<Subtitle>,
                IIncludableQueryable<Subtitle, object>>>()))
                .ReturnsAsync(value: subtitles);

            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(x => x.Map<IEnumerable<SubtitleDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(() =>
                {
                    var dtolist = new List<SubtitleDTO>();

                    for (int i = 0; i < subtitles.Count; i++)
                        dtolist.Add(new SubtitleDTO { Id = subtitles[i].Id, StreetcodeId = subtitles[i].StreetcodeId });

                    return dtolist;
                });
            
            var handler = new GetSubtitlesByStreetcodeIdQueryHandler(mockRepo.Object, mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetSubtitlesByStreetcodeIdQuery(streetcode_id), CancellationToken.None);
            
            //Assert
            Assert.NotNull(result.Value);

            Assert.IsType<List<SubtitleDTO>>(result.Value);

            Assert.NotEmpty(result.Value);

            Assert.True(result.Value.All(x => x.StreetcodeId == streetcode_id));
        }

        [Theory]
        [InlineData(-1)]
        public async Task GetSubtitlesByStreetcodeId_ReturnsNoElements(int streetcode_id)
        {

            var mockRepo = new Mock<IRepositoryWrapper>();

            mockRepo.Setup(repo => repo.SubtitleRepository.GetAllAsync(
                It.IsAny<Expression<Func<Subtitle, bool>>>(),
                It.IsAny<Func<IQueryable<Subtitle>,
                IIncludableQueryable<Subtitle, object>>>()))
                .ReturnsAsync(new List<Subtitle>()); //default value

            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(x => x.Map<IEnumerable<SubtitleDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns((List<Subtitle> subtitles) =>
                {
                    var dtolist = new List<SubtitleDTO>();

                    for (int i = 0; i < subtitles.Count; i++)
                        dtolist.Add(new SubtitleDTO { Id = subtitles[i].Id, StreetcodeId = subtitles[i].StreetcodeId });

                    return dtolist;
                });

            var handler = new GetSubtitlesByStreetcodeIdQueryHandler(mockRepo.Object, mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetSubtitlesByStreetcodeIdQuery(streetcode_id), CancellationToken.None);

            //Assert
            Assert.NotNull(result.Value);

            Assert.IsType<List<SubtitleDTO>>(result.Value);

            Assert.Empty(result.Value);
        }
    }
}
