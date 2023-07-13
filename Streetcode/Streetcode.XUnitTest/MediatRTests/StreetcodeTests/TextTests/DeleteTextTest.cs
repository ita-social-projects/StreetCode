using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Text.Delete;
using Streetcode.BLL.MediatR.Streetcode.Text.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.StreetcodeTest.TextTest
{
    public class DeleteTextTest
    {
        private Mock<IRepositoryWrapper> repository;
        private readonly Mock<IStringLocalizer<FailedToDeleteSharedResource>> _mockLocalizerFailedToDelete;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public DeleteTextTest()
        {
            repository = new Mock<IRepositoryWrapper>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            _mockLocalizerFailedToDelete = new Mock<IStringLocalizer<FailedToDeleteSharedResource>>();
        }

        private static Text GetText(int id)
        {
            return new Text { Id = id };
        }

        private static Text? GetTextWithNotExitingId()
        {
            return null;
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteTextSuccessfull(int id)
        {
            repository.Setup(x => x.TextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Text, bool>>>(),
                It.IsAny<Func<IQueryable<Text>, IIncludableQueryable<Text, object>>>()))
                .ReturnsAsync(GetText(id));

            repository.Setup(x => x.TextRepository.Delete(GetText(id)));

            repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var handler = new DeleteTextHandler(repository.Object, _mockLocalizerFailedToDelete.Object, _mockLocalizerCannotFind.Object);

            var result = await handler.Handle(new DeleteTextCommand(id), CancellationToken.None);

            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess));
        }

    }
}
