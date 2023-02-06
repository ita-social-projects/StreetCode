using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Delete;
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
    public class DeleteStreetcodeHandlerTests
    {
        [Theory]
        [InlineData(1)]
        public async Task DeleteStreetcodeHandler_Success(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testStreetcode = new StreetcodeContent();

            var testStreetcodeDTO = new EventStreetcodeDTO();

            repository.Setup(x => x.StreetcodeRepository.Delete(testStreetcode));
            repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null)).ReturnsAsync(testStreetcode);

            mockMapper.Setup(x => x.Map<StreetcodeContent>(It.IsAny<object>())).Returns(testStreetcode);

            var handler = new DeleteStreetcodeHandler(repository.Object);

            var result = await handler.Handle(new DeleteStreetcodeCommand(id), CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteStreetcodeHandler_NullError(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();

            var testStreetcode = new StreetcodeContent();

            var testStreetcodeDTO = new EventStreetcodeDTO()
            {
                Id = id
            };

            repository.Setup(x => x.StreetcodeRepository.Delete(testStreetcode));
            repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null)).ReturnsAsync((StreetcodeContent)null);


            var handler = new DeleteStreetcodeHandler(repository.Object);

            var result = await handler.Handle(new DeleteStreetcodeCommand(id), CancellationToken.None);

            Assert.True(result.IsFailed);
            Assert.Equal($"Cannot find a streetcode with corresponding categoryId: {id}", result.Errors.Single().Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteStreetcodeHandler_SaveAsyncError(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();

            var testStreetcode = new StreetcodeContent();

            var testStreetcodeDTO = new EventStreetcodeDTO()
            {
                Id = id
            };

            repository.Setup(x => x.StreetcodeRepository.Delete(testStreetcode));
            repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(-1);
            repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null)).ReturnsAsync(testStreetcode);


            var handler = new DeleteStreetcodeHandler(repository.Object);

            var result = await handler.Handle(new DeleteStreetcodeCommand(id), CancellationToken.None);

            Assert.True(result.IsFailed);
            Assert.Equal("Failed to delete a streetcode", result.Errors.Single().Message);
        }
    }
}
