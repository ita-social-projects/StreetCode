using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetAll;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Streetcode.BLL.Mapping.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Types;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class GetAllTagsRequestHandlerTests
    {
        [Fact]
        public async Task GetAllTags_ReturnsList()
        {
            //arrange
            var tags = new List<Tag>()
            {
                new Tag
                {
                    Id = 1,
                    Title = "some title 1",
                    Streetcodes = new List<StreetcodeContent> {
                        new StreetcodeContent {
                            Id = 2
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
                .ReturnsAsync(value: tags);

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

            var handler = new GetAllTagsHandler(mockRepo.Object, mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetAllTagsQuery(), CancellationToken.None);

            //Assert
            Assert.IsType<List<TagDTO>>(result.Value);

            Assert.True(result.Value.Count() == tags.Count);
        }

        [Fact]
        public async Task GetAllTags_ReturnsNoElements()
        {
            List<Tag> tags = new List<Tag>();

            var mockRepo = new Mock<IRepositoryWrapper>();

            mockRepo.Setup(repo => repo.TagRepository.GetAllAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(value: tags);

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

            var handler = new GetAllTagsHandler(mockRepo.Object, mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetAllTagsQuery(), CancellationToken.None);

            //Assert
            Assert.IsType<List<TagDTO>>(result.Value);

            Assert.Empty(result.Value);
        }
    }
}
