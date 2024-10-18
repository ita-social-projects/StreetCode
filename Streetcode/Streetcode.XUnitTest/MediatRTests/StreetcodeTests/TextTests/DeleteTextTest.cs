﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Text.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.StreetcodeTest.TextTest
{
    public class DeleteTextTest
    {
        private readonly Mock<IRepositoryWrapper> repository;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<FailedToDeleteSharedResource>> mockLocalizerFailedToDelete;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

        public DeleteTextTest()
        {
            this.repository = new Mock<IRepositoryWrapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            this.mockLocalizerFailedToDelete = new Mock<IStringLocalizer<FailedToDeleteSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteTextSuccessfull(int id)
        {
            this.repository
                .Setup(x => x.TextRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Text, bool>>>(),
                        It.IsAny<Func<IQueryable<Text>,
                        IIncludableQueryable<Text, object>>>()))
                .ReturnsAsync(GetText(id));

            this.repository.Setup(x => x.TextRepository.Delete(GetText(id)));

            this.repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var handler = new DeleteTextHandler(this.repository.Object, this.mockLogger.Object, this.mockLocalizerFailedToDelete.Object, this.mockLocalizerCannotFind.Object);

            var result = await handler.Handle(new DeleteTextCommand(id), CancellationToken.None);

            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess));
        }

        private static Text GetText(int id)
        {
            return new Text { Id = id };
        }
    }
}
