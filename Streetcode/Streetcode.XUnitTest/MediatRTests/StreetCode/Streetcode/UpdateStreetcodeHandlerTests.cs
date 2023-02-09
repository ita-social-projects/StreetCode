/*using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class UpdateStreetcodeHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        public UpdateStreetcodeHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Handle_ReturnsSuccess()
        {   
            // arrange
            var testStreetcode = new StreetcodeContent();
            var testStreetcodeDTO = new EventStreetcodeDTO();
            int testSaveChangesSuccess = 1;

            RepositorySetup(testStreetcode, testSaveChangesSuccess);
            MapperSetup(testStreetcode);

            var handler = new UpdateStreetcodeHandler(_repository.Object, _mapper.Object);
            // act
            var result = await handler.Handle(new UpdateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);
            // assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_ReturnsMapNullError()
        {   
            // arrange
            var testStreetcode = new StreetcodeContent();
            var testStreetcodeDTO = new EventStreetcodeDTO();
            string expectedErrorMessage = "Cannot convert null to Streetcode";
            int testSaveChangesSuccess = 1;

            RepositorySetup(testStreetcode, testSaveChangesSuccess);
            MapperSetup(null);

            var handler = new UpdateStreetcodeHandler(_repository.Object, _mapper.Object);
            // act
            var result = await handler.Handle(new UpdateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);
            // assert
            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        [Fact]
        public async Task Handle_ReturnsSaveError()
        {   
            // arrange
            var testStreetcode = new StreetcodeContent();
            var testStreetcodeDTO = new EventStreetcodeDTO();
            string expectedErrorMessage = "Failed to update a streetcode";
            int testSaveChangesFailed = -1;

            RepositorySetup(testStreetcode, testSaveChangesFailed);
            MapperSetup(testStreetcode);

            var handler = new UpdateStreetcodeHandler(_repository.Object, _mapper.Object);
            // act
            var result = await handler.Handle(new UpdateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);
            // assert
            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        private void RepositorySetup(StreetcodeContent testStreetcode, int saveChangesVariable)
        {
            _repository.Setup(x => x.StreetcodeRepository.Update(testStreetcode));
            _repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(saveChangesVariable);
        }
        private void MapperSetup(StreetcodeContent testStreetcode)
        {
            _mapper.Setup(x => x.Map<StreetcodeContent>(It.IsAny<object>())).Returns(testStreetcode);
        }
    }
}
*/