using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Delete;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Entity = Streetcode.DAL.Entities.Streetcode.TextContent.RelatedTerm;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.RelatedTerm.Delete
{
    public class DeleteRelatedTermHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
        private readonly Mock<IMapper> _mapperMock;

        public DeleteRelatedTermHandlerTests()
        {
            _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            _mapperMock = new Mock<IMapper>();
        }

        [Theory]
        [InlineData(1, "test", 1)]
        public async void Handle_WhenRelatedTermExists_DeletesItAndReturnsSuccessResult(int id, string word, int termId)
        {
            // Arrange
            int _id = id;
            var relatedTerm = new RelatedTermDTO { Id = _id, Word = word, TermId = termId };

            _repositoryWrapperMock.Setup(r => r.RelatedTermRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Entity, bool>>>(),
                It.IsAny<Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>>>())).ReturnsAsync(relatedTerm);
               
            var handler = new DeleteRelatedTermHandler(repositoryMock.Object, mapperMock.Object);
            var command = new DeleteRelatedTermCommand(_id);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            repositoryMock.Verify(r => r.RelatedTermRepository.Delete(relatedTerm), Times.Once);
            repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            Assert.True(result.IsSuccess);
        }

        
    }
}
