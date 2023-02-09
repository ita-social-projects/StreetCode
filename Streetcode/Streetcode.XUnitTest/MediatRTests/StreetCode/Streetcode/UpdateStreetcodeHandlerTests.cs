using AutoMapper;
using MediatR;
using Moq;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Delete;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update;
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
    public class UpdateStreetcodeHandlerTests
    {
        [Fact]
        public async Task UpdateStreetcodeHandlerTests_Success()
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testStreetcode = new StreetcodeContent();

            var testStreetcodeDTO = new EventStreetcodeDTO();

            repository.Setup(x => x.StreetcodeRepository.Update(testStreetcode));
            repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            
            mockMapper.Setup(x => x.Map<StreetcodeContent>(It.IsAny<object>())).Returns(testStreetcode);

            var handler = new UpdateStreetcodeHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new UpdateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateStreetcodeHandlerTests_MapNull()
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testStreetcode = new StreetcodeContent();

            var testStreetcodeDTO = new EventStreetcodeDTO();

            repository.Setup(x => x.StreetcodeRepository.Update(testStreetcode));
            repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            mockMapper.Setup(x => x.Map<StreetcodeContent>(It.IsAny<object>())).Returns((StreetcodeContent)null);

            var handler = new UpdateStreetcodeHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new UpdateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);

            Assert.True(result.IsFailed);
            Assert.Equal("Cannot convert null to Streetcode", result.Errors.Single().Message);
        }

        [Fact]
        public async Task UpdateStreetcodeHandlerTests_SaveError()
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testStreetcode = new StreetcodeContent();

            var testStreetcodeDTO = new EventStreetcodeDTO();

            repository.Setup(x => x.StreetcodeRepository.Update(testStreetcode));
            repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(-1);

            mockMapper.Setup(x => x.Map<StreetcodeContent>(It.IsAny<object>())).Returns(testStreetcode);

            var handler = new UpdateStreetcodeHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new UpdateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);

            Assert.True(result.IsFailed);
            Assert.Equal("Failed to update a streetcode", result.Errors.Single().Message);
        }
    }
}
