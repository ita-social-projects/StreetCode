using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class GetTagsByStreetcodeIdHandlerTests
    {
        [Theory]
        [InlineData(1)]
        public async Task GetSTagsByStreetcodeId_ReturnsList(int streetcode_id)
        {
            //Arrange
            var tags = new List<Tag>()
            {
                new Tag
                {
                    Id = 1,
                    Title = "some title 1",
                    Streetcodes = new List<StreetcodeContent> {
                        new StreetcodeContent {
                            Id = 1
                        }
                    }
                },
                new Tag
                {
                    Id = 2,
                    Title = "some title 2",
                    Streetcodes = new List<StreetcodeContent> {
                        new StreetcodeContent {
                            Id = 1
                        }
                    }
                }
            };
            var mockRepo = new Mock<IRepositoryWrapper>();

            mockRepo.Setup(repo => repo.TagRepository.GetAllAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(tags);

            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(x => x.Map<IEnumerable<TagDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns((List<Tag> tags) =>
                {
                    var dtolist = new List<TagDTO>();

                    for (int i = 0; i < tags.Count; i++)
                        dtolist.Add(new TagDTO
                        {
                            Id = tags[i].Id,
                            Streetcodes = new List<StreetcodeDTO>() {
                                new EventStreetcodeDTO { Id =
                                tags[i].Streetcodes[0].Id
                            }}
                        });
                    return dtolist;
                });

            var handler = new GetTagByStreetcodeIdHandler(mockRepo.Object, mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetTagByStreetcodeIdQuery(streetcode_id), CancellationToken.None);
            
            //Assert
            Assert.NotNull(result.Value);

            Assert.IsType<List<TagDTO>>(result.Value);

            Assert.NotEmpty(result.Value);

            Assert.True(result.Value.All(x =>
                x.Streetcodes.All(y => y.Id == streetcode_id)));
        }

        [Theory]
        [InlineData(-1)]
        public async Task GetSubtitlesByStreetcodeId_ReturnsNoElements(int id)
        {

            var mockRepo = new Mock<IRepositoryWrapper>();

            mockRepo.Setup(repo => repo.TagRepository.GetAllAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(new List<Tag>()); //default

            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(x => x.Map<IEnumerable<TagDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns((List<Tag> tags) =>
                {
                    var dtolist = new List<TagDTO>();

                    for (int i = 0; i < tags.Count; i++)
                        dtolist.Add(new TagDTO
                        {
                            Id = tags[i].Id,
                            Streetcodes = new List<StreetcodeDTO>() {
                                new EventStreetcodeDTO { Id =
                                tags[i].Streetcodes[0].Id
                            }}
                        });
                    return dtolist;
                });

            var handler = new GetTagByStreetcodeIdHandler(mockRepo.Object, mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetTagByStreetcodeIdQuery(id), CancellationToken.None);

            //Assert
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }
    }
}
