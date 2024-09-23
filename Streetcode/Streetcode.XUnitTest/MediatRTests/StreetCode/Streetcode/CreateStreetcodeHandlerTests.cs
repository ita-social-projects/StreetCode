﻿/*using AutoMapper;
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
            // arrange
            var testStreetcode = new StreetcodeContent();
            var testStreetcodeDTO = new EventStreetcodeDTO();
            int testSaveChangesSuccess = 1;

            RepositorySetup(testStreetcode, testSaveChangesSuccess);
            MapperSetup(testStreetcode);

            var handler = new CreateStreetcodeHandler(_repository.Object, _mapper.Object);
            // act
            var result = await handler.Handle(new CreateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);
            // assert
            Assert.True(result.IsSuccess);

        }
        [Fact]
        public async Task Handle_ReturnErrorNull()
        {   
            // arrange
            var testStreetcode = new StreetcodeContent();
            var testStreetcodeDTO = new EventStreetcodeDTO();
            string expectedErrorMessage = "Cannot convert null to Streetcode";
            int testSaveChangesSuccess = 1;

            RepositorySetup(testStreetcode, testSaveChangesSuccess);
            MapperSetup(null);

            var handler = new CreateStreetcodeHandler(_repository.Object, _mapper.Object);
            // act
            var result = await handler.Handle(new CreateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);
            // assert
            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        [Fact]
        public async Task Handle_ReturnsSaveError()
        {   
            // arrange
            var testStreetcode = new StreetcodeContent();
            var testStreetcodeDTO = new EventStreetcodeDTO();
            string expectedErrorMessage = "Failed to create a streetcode";
            int testSaveChangesFailed = -1;

            RepositorySetup(testStreetcode, testSaveChangesFailed);
            MapperSetup(testStreetcode);

            _mapper.Setup(x => x.Map<StreetcodeContent>(It.IsAny<object>())).Returns(testStreetcode);

            var handler = new CreateStreetcodeHandler(_repository.Object, _mapper.Object);
            // act
            var result = await handler.Handle(new CreateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);
            // assert
            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        [Fact]
        public async Task Handle_ReturnsCorrectType() 
        { 
            // arrange
            var testStreetcode = new StreetcodeContent();
            var testStreetcodeDTO = new EventStreetcodeDTO();
            int testSaveChangesSuccess = 1;

            RepositorySetup(testStreetcode, testSaveChangesSuccess);
            MapperSetup(testStreetcode);

            var handler = new CreateStreetcodeHandler(_repository.Object, _mapper.Object);
            // act
            var result = await handler.Handle(new CreateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);
            // assert
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
*/