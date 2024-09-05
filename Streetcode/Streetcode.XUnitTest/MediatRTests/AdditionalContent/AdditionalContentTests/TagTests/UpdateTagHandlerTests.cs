using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Update;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class UpdateTagHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        public UpdateTagHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_IsCorrectAndSuccess()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.TagRepository.Update(new Tag()));
            _mockRepo.Setup(repo => repo.TagRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Tag, bool>>>(), default))
                .ReturnsAsync((Expression<Func<Tag, bool>> expr, IIncludableQueryable<Tag, bool> include) =>
                {
                    BinaryExpression eq = (BinaryExpression)expr.Body;
                    MemberExpression member = (MemberExpression)eq.Left;
                    return member.Member.Name == "Id" ? new Tag() : null;
                });

            _mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(x => x.Map<TagDTO>(It.IsAny<Tag>())).Returns(new TagDTO());


            var handler = new UpdateTagHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);

            //Act
            var result = await handler.Handle(new UpdateTagCommand(new UpdateTagDTO()), CancellationToken.None);

            //Assert
            Assert.Multiple(
               () => Assert.IsType<TagDTO>(result.Value),
               () => Assert.True(result.IsSuccess));
        }
    }
}
