using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
using MediatR;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class CreateStreetcodeHandlerTests
    {
        [Fact]
        public async Task CreateStreetcodeHandler_Success()
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testStreetcode = new StreetcodeContent();

            var testStreetcodeDTO = new EventStreetcodeDTO();

            repository.Setup(x => x.StreetcodeRepository.Create(testStreetcode));
            repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            mockMapper.Setup(x => x.Map<StreetcodeContent>(It.IsAny<object>())).Returns(testStreetcode);

            var handler = new CreateStreetcodeHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new CreateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);
            
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

        }
        [Fact]
        public async Task CreateStreetcodeHandler_ErrorNull()
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testStreetcode = new StreetcodeContent();

            var testStreetcodeDTO = new EventStreetcodeDTO();

            repository.Setup(x => x.StreetcodeRepository.Create(testStreetcode));
            repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            mockMapper.Setup(x => x.Map<StreetcodeContent>(It.IsAny<object>())).Returns((StreetcodeContent)null);

            var handler = new CreateStreetcodeHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new CreateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);

            Assert.Single(result.Errors);
            Assert.Equal("Cannot convert null to Streetcode", result.Errors.First().Message);

        }

        [Fact]
        public async Task CreateStreetcodeHandler_SaveError()
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testStreetcode = new StreetcodeContent();

            var testStreetcodeDTO = new EventStreetcodeDTO();

            repository.Setup(x => x.StreetcodeRepository.Create(testStreetcode));
            repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(-1);

            mockMapper.Setup(x => x.Map<StreetcodeContent>(It.IsAny<object>())).Returns(testStreetcode);

            var handler = new CreateStreetcodeHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new CreateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);

            Assert.Single(result.Errors);
            Assert.Equal("Failed to create a streetcode", result.Errors.First().Message);
        }

        [Fact]
        public async Task CreateStreetcodeHandler_TypeCheck() 
        { 
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testStreetcode = new StreetcodeContent();

            var testStreetcodeDTO = new EventStreetcodeDTO();

            repository.Setup(x => x.StreetcodeRepository.Create(testStreetcode));
            repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            mockMapper.Setup(x => x.Map<StreetcodeContent>(It.IsAny<object>())).Returns(testStreetcode);

            var handler = new CreateStreetcodeHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new CreateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);

            Assert.IsAssignableFrom<Unit>(result.Value);
        }
    }
}
