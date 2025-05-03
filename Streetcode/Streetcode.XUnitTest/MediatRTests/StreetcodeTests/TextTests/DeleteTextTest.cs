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
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<FailedToDeleteSharedResource>> _mockLocalizerFailedToDelete;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public DeleteTextTest()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            _mockLocalizerFailedToDelete = new Mock<IStringLocalizer<FailedToDeleteSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteTextSuccessfull(int id)
        {
            _repository
                .Setup(x => x.TextRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Text, bool>>>(),
                        It.IsAny<Func<IQueryable<Text>,
                        IIncludableQueryable<Text, object>>>()))
                .ReturnsAsync(GetText(id));

            _repository.Setup(x => x.TextRepository.Delete(GetText(id)));

            _repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var handler = new DeleteTextHandler(_repository.Object, _mockLogger.Object, _mockLocalizerFailedToDelete.Object, _mockLocalizerCannotFind.Object);

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
