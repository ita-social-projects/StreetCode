using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Delete;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class DeleteStreetcodeHandlerTests
    {   
        private readonly Mock<IRepositoryWrapper> _repository;
        public DeleteStreetcodeHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsSuccess(int id)
        {
            var testStreetcode = new StreetcodeContent();
            int testSaveChangesSuccess = 1;

            RepositorySetup(testStreetcode, testSaveChangesSuccess);

            var handler = new DeleteStreetcodeHandler(_repository.Object);

            var result = await handler.Handle(new DeleteStreetcodeCommand(id), CancellationToken.None);
            
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsNullError(int id)
        {
            string expectedErrorMessage = $"Cannot find a streetcode with corresponding categoryId: {id}";
            int testSaveChangesSuccess = 1;

            RepositorySetup(null, testSaveChangesSuccess);

            var handler = new DeleteStreetcodeHandler(_repository.Object);

            var result = await handler.Handle(new DeleteStreetcodeCommand(id), CancellationToken.None);

            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsSaveAsyncError(int id)
        {
            var testStreetcode = new StreetcodeContent();
            string expectedErrorMessage = "Failed to delete a streetcode";
            int testSaveChangesFailed = -1;

            RepositorySetup(testStreetcode, testSaveChangesFailed);

            var handler = new DeleteStreetcodeHandler(_repository.Object);

            var result = await handler.Handle(new DeleteStreetcodeCommand(id), CancellationToken.None);

            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        private void RepositorySetup(StreetcodeContent streetcodeContent, int saveChangesVariable)
        {
            _repository.Setup(x => x.StreetcodeRepository.Delete(streetcodeContent));
            _repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(saveChangesVariable);
            _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null)).ReturnsAsync(streetcodeContent);

        }

    }
}
