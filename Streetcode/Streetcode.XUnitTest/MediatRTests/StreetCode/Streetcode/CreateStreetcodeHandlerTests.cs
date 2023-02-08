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
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        public CreateStreetcodeHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Handle_ReturnsSuccess()
        {
            var testStreetcode = new StreetcodeContent();
            var testStreetcodeDTO = new EventStreetcodeDTO();

            RepositorySetup(testStreetcode, 1);
            MapperSetup(testStreetcode);

            var handler = new CreateStreetcodeHandler(_repository.Object, _mapper.Object);

            var result = await handler.Handle(new CreateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);
            
            Assert.True(result.IsSuccess);

        }
        [Fact]
        public async Task Handle_ReturnErrorNull()
        {
            var testStreetcode = new StreetcodeContent();
            var testStreetcodeDTO = new EventStreetcodeDTO();
            string expectedErrorMessage = "Cannot convert null to Streetcode";

            RepositorySetup(testStreetcode, 1);
            MapperSetup(null);

            var handler = new CreateStreetcodeHandler(_repository.Object, _mapper.Object);

            var result = await handler.Handle(new CreateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);

            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        [Fact]
        public async Task Handle_ReturnsSaveError()
        {
            var testStreetcode = new StreetcodeContent();
            var testStreetcodeDTO = new EventStreetcodeDTO();
            string expectedErrorMessage = "Failed to create a streetcode";

            RepositorySetup(testStreetcode, -1);
            MapperSetup(testStreetcode);

            _mapper.Setup(x => x.Map<StreetcodeContent>(It.IsAny<object>())).Returns(testStreetcode);

            var handler = new CreateStreetcodeHandler(_repository.Object, _mapper.Object);

            var result = await handler.Handle(new CreateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);

            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        [Fact]
        public async Task Handle_ReturnsCorrectType() 
        { 
            var testStreetcode = new StreetcodeContent();
            var testStreetcodeDTO = new EventStreetcodeDTO();

            RepositorySetup(testStreetcode, 1);
            MapperSetup(testStreetcode);

            var handler = new CreateStreetcodeHandler(_repository.Object, _mapper.Object);

            var result = await handler.Handle(new CreateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);

            Assert.IsAssignableFrom<Unit>(result.Value);
        }

        private void RepositorySetup(StreetcodeContent streetcodeContent, int saveChangesVariable)
        {
            _repository.Setup(x => x.StreetcodeRepository.Create(streetcodeContent));
            _repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(saveChangesVariable);
        }

        private void MapperSetup(StreetcodeContent streetcodeContent)
        {
            _mapper.Setup(x => x.Map<StreetcodeContent>(It.IsAny<object>())).Returns(streetcodeContent);
        }
    }
}
