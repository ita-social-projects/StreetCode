
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetAll;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.SubtitleTests
{
    public class GetAllSubtitlesRequestHandlerTests
    {
        [Fact]
        public async Task GetAllSubtitles_ReturnsList()
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
                    Description = "description",
                    Title = "title",
                    Url = "url",
                    StreetcodeId = 1
                }
            };

            var mockRepo = new Mock<IRepositoryWrapper>();

            mockRepo.Setup(repo => repo.SubtitleRepository.GetAllAsync(
                It.IsAny<Expression<Func<Subtitle, bool>>>(),
                It.IsAny<Func<IQueryable<Subtitle>,
                IIncludableQueryable<Subtitle, object>>>()))
                .ReturnsAsync(subtitles);

            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(x => x.Map<IEnumerable<SubtitleDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns((List<Subtitle> subtitles) =>
                {
                    var dtolist = new List<SubtitleDTO>();

                    for (int i = 0; i < subtitles.Count; i++)
                        dtolist.Add(new SubtitleDTO { Id = subtitles[i].Id, StreetcodeId = subtitles[i].StreetcodeId });

                    return dtolist;
                });

            var handler = new GetAllSubtitlesHandler(mockRepo.Object, mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetAllSubtitlesQuery(), CancellationToken.None);
            
            //Assert
            Assert.IsType<List<SubtitleDTO>>(result.Value);

            Assert.True(result.Value.Count() == subtitles.Count);
        }

        [Fact]
        public async Task GetAllSubtitles_ReturnsNoElements()
        {
            var mockRepo = new Mock<IRepositoryWrapper>();

            mockRepo.Setup(repo => repo.SubtitleRepository.GetAllAsync(
                It.IsAny<Expression<Func<Subtitle, bool>>>(),
                It.IsAny<Func<IQueryable<Subtitle>,
                IIncludableQueryable<Subtitle, object>>>()))
                .ReturnsAsync(new List<Subtitle>());

            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(x => x.Map<IEnumerable<SubtitleDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns((List<Subtitle> subtitles) =>
                {
                    var dtolist = new List<SubtitleDTO>();

                    for (int i = 0; i < subtitles.Count; i++)
                        dtolist.Add(new SubtitleDTO { Id = subtitles[i].Id, StreetcodeId = subtitles[i].StreetcodeId });

                    return dtolist;
                });

            var handler = new GetAllSubtitlesHandler(mockRepo.Object, mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetAllSubtitlesQuery(), CancellationToken.None);

            //Assert
            Assert.IsType<List<SubtitleDTO>>(result.Value);

            Assert.Empty(result.Value);
        }

    }
}
