using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Update;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class UpdateTagHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<FailedToValidateSharedResource>> mockStringLocalizerFailedToValidate;
        private readonly Mock<IStringLocalizer<FieldNamesSharedResource>> mockStringLocalizerFieldNames;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockStringLocalizerCannotFind;

        public UpdateTagHandlerTests()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockStringLocalizerFailedToValidate = new Mock<IStringLocalizer<FailedToValidateSharedResource>>();
            this.mockStringLocalizerFieldNames = new Mock<IStringLocalizer<FieldNamesSharedResource>>();
            this.mockStringLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_IsCorrectAndSuccess()
        {
            // Arrange
            this.mockRepo.Setup(repo => repo.TagRepository.Update(new Tag()));
            this.mockRepo.Setup(repo => repo.TagRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Tag, bool>>>(), default))
                .ReturnsAsync((Expression<Func<Tag, bool>> expr, IIncludableQueryable<Tag, bool> include) =>
                {
                    BinaryExpression eq = (BinaryExpression)expr.Body;
                    MemberExpression member = (MemberExpression)eq.Left;
                    return member.Member.Name == "Id" ? new Tag() : null;
                });

            this.mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
            this.mockMapper.Setup(x => x.Map<TagDTO>(It.IsAny<Tag>())).Returns(new TagDTO());

            var handler = new UpdateTagHandler(
                this.mockRepo.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockStringLocalizerFailedToValidate.Object,
                this.mockStringLocalizerFieldNames.Object,
                this.mockStringLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new UpdateTagCommand(new UpdateTagDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
               () => Assert.IsType<TagDTO>(result.Value),
               () => Assert.True(result.IsSuccess));
        }
    }
}
