using FluentResults;
using Moq;
using Streetcode.BLL.MediatR.Media.Art.StreetcodeArtSlide.GetAllCountByStreetcodeId;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;

namespace Streetcode.XUnitTest.MediatRTests.Media.StreetcodeArtSlide
{
    public class GetAllCountByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepo;

        public GetAllCountByStreetcodeIdTest()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
        }

        [Theory]
        [InlineData(1, 5)]  
        public async Task Handle_ReturnsCorrectCount(uint streetcodeId, int expectedCount)
        {
            this.MockRepositoryAndMapper(streetcodeId, expectedCount);

            var handler = new GetAllCountByStreetcodeIdHandler(this.mockRepo.Object);

            var result = await handler.Handle(new GetAllCountByStreetcodeIdQuerry(streetcodeId), CancellationToken.None);

            Assert.Equal(expectedCount, result.Value);
        }

        [Theory]
        [InlineData(0)]  
        public async Task ShouldReturnFailure_WhenStreetcodeIdNotFound(uint streetcodeId)
        {
            this.mockRepo
                .Setup(r => r.StreetcodeArtSlideRepository
                    .FindAll(It.IsAny<Expression<Func<DAL.Entities.Streetcode.StreetcodeArtSlide, bool>>>(),
                        It.IsAny<Func<IQueryable<DAL.Entities.Streetcode.StreetcodeArtSlide>, IIncludableQueryable<DAL.Entities.Streetcode.StreetcodeArtSlide, object>>>()))
                .Returns(Enumerable.Empty<DAL.Entities.Streetcode.StreetcodeArtSlide>().AsQueryable());

            var handler = new GetAllCountByStreetcodeIdHandler(this.mockRepo.Object);

            var result = await handler.Handle(new GetAllCountByStreetcodeIdQuerry(streetcodeId), CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Value);  
        }


        private void MockRepositoryAndMapper(uint streetcodeId, int count)
        {
            this.mockRepo
                .Setup(r => r.StreetcodeArtSlideRepository
                    .FindAll(It.IsAny<Expression<Func<DAL.Entities.Streetcode.StreetcodeArtSlide, bool>>>(),
                        It.IsAny<Func<IQueryable<DAL.Entities.Streetcode.StreetcodeArtSlide>, IIncludableQueryable<DAL.Entities.Streetcode.StreetcodeArtSlide, object>>>()))
                .Returns((Expression<Func<DAL.Entities.Streetcode.StreetcodeArtSlide, bool>> predicate,
                          Func<IQueryable<DAL.Entities.Streetcode.StreetcodeArtSlide>, IIncludableQueryable<DAL.Entities.Streetcode.StreetcodeArtSlide, object>> include) =>
                {
                    var slides = new List<DAL.Entities.Streetcode.StreetcodeArtSlide>();

                    for (int i = 0; i < count; i++)
                    {
                        slides.Add(new DAL.Entities.Streetcode.StreetcodeArtSlide
                        {
                            StreetcodeId = (int)streetcodeId,
                        });
                    }

                    return slides.AsQueryable();
                });
        }
    }
}
