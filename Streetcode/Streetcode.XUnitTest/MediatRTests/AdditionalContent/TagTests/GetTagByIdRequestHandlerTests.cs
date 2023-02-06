using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetById;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class GetTagByIdRequestHandlerTests
    {
        [Theory]
        [InlineData(1)]
        public async Task GetTagById_ReturnElement(int id)
        {
            //Arrange
            var mockRepo = new Mock<IRepositoryWrapper>();

            mockRepo.Setup(repo => repo.TagRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(new Tag { Id = 1 });

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<TagDTO>(It.IsAny<Tag>()))
                .Returns((Tag source) =>
                {
                    return new TagDTO
                    {
                        Id = source.Id
                    };
                });

            var handler = new GetTagByIdHandler(mockRepo.Object, mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetTagByIdQuery(id), CancellationToken.None);

            //Assert
            Assert.NotNull(result.Value);

            Assert.IsType<TagDTO>(result.Value);

            Assert.True(result.Value.Id.Equals(id));
        }

        [Theory]
        [InlineData(-1)]
        public async Task GetTagById_ReturnsNoElements(int id)
        {
            //Arrange
            var mockRepo = new Mock<IRepositoryWrapper>();

            mockRepo.Setup(repo => repo.TagRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<Tag, bool>>>(),
            It.IsAny<Func<IQueryable<Tag>,
            IIncludableQueryable<Tag, object>>>()))
            .ReturnsAsync(new Tag()); //returns default

            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(x => x.Map<TagDTO>(It.IsAny<Tag>()))
                .Returns((Tag source) =>
                {
                    return new TagDTO
                    {
                        Id = source.Id
                    };
                });

            var handler = new GetTagByIdHandler(mockRepo.Object, mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetTagByIdQuery(id), CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.Value.Id.Equals(id));
        }
    }
}
