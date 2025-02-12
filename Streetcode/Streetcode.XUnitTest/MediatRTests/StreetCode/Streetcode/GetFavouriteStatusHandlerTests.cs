using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetFavouriteStatus;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetFavouriteStatusHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;

        public GetFavouriteStatusHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
        }

        [Fact]
        public async Task Handle_ReturnsTrue()
        {
            // Arrange
            int streetcodeId = 1;
            string userId = "mockId";

            this.RepositorySetup(
                new StreetcodeContent
                {
                    Id = streetcodeId,
                });

            var handler = new GetFavouriteStatusHandler(_repository.Object);

            // Act
            var result = await handler.Handle(new GetFavouriteStatusQuery(streetcodeId, userId), CancellationToken.None);

            // Assert
            Assert.True(result.Value);
        }

        [Fact]
        public async Task Handle_ReturnsFalse()
        {
            // Arrange
            int streetcodeId = 1;
            string userId = "mockId";

            this.RepositorySetup(null);

            var handler = new GetFavouriteStatusHandler(_repository.Object);

            // Act
            var result = await handler.Handle(new GetFavouriteStatusQuery(streetcodeId, userId), CancellationToken.None);

            // Assert
            Assert.False(result.Value);
        }

        private void RepositorySetup(StreetcodeContent? favourite)
        {
            _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(favourite);

        }
    }
}
