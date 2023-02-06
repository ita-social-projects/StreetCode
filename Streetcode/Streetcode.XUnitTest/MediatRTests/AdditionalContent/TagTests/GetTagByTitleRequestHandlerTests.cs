using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetById;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetTagByTitle;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class GetTagByTitleRequestHandlerTests
    {
        [Theory]
        [InlineData("test_title")]
        public async Task GetTagByTitle_Exists(string title)
        {
            //Arrange
            var mockRepo = new Mock<IRepositoryWrapper>();

            mockRepo.Setup(repo => repo.TagRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(new Tag { Id = 1, Title = title});

            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(x => x.Map<TagDTO>(It.IsAny<Tag>()))
                .Returns((Tag source) =>
                {
                    return new TagDTO
                    {
                        Id = source.Id,
                        Title = source.Title
                    };
                });

            var handler = new GetTagByTitleHandler(mockRepo.Object, mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetTagByTitleQuery(title), CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);

            Assert.NotNull(result.Value);

            Assert.IsType<TagDTO>(result.Value);

            Assert.True(result.Value.Title.Equals(title));
        }

        [Theory]
        [InlineData("doesn't_existing_title")]
        public async Task GetTagByTitle_NotExists(string title)
        {
            //Arrange

            var mockRepo = new Mock<IRepositoryWrapper>();

            mockRepo.Setup(repo => repo.TagRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(new Tag()); //default

            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(x => x.Map<TagDTO>(It.IsAny<Tag>()))
                .Returns((Tag source) =>
                {
                    return new TagDTO
                    {
                        Id = source.Id,
                        Title = source.Title
                    };
                });

            var handler = new GetTagByTitleHandler(mockRepo.Object, mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetTagByTitleQuery(title), CancellationToken.None);

            //Assert

            Assert.NotNull(result.Value);

            Assert.IsType<TagDTO>(result.Value);

            Assert.True(result.Value.Title is null);
        }
    }
}
