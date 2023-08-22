using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetById;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetStreetcodeByIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILoggerService> _mockLogger;
        public GetStreetcodeByIdHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsSuccess(int id) 
        {   
            // arrange
            var testContentDTO = new EventStreetcodeDTO();
            var testContent = new StreetcodeContent();

            RepositorySetup(testContent);
            MapperSetup(testContentDTO);

            var handler = new GetStreetcodeByIdHandler(_repository.Object, _mapper.Object, _mockLogger.Object);
            // act
            var result = await handler.Handle(new GetStreetcodeByIdQuery(id), CancellationToken.None);
            // assert
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsCorrectType(int id)
        {   
            // arrange
            var testContentDTO = new EventStreetcodeDTO();
            var testContent = new StreetcodeContent();

            RepositorySetup(testContent);
            MapperSetup(testContentDTO);

            var handler = new GetStreetcodeByIdHandler(_repository.Object, _mapper.Object, _mockLogger.Object);
            // act
            var result = await handler.Handle(new GetStreetcodeByIdQuery(id), CancellationToken.None);
            // assert
            Assert.IsAssignableFrom<StreetcodeDTO>(result.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsError(int id)
        {   
            // arrange
            string expectedErrorMessage = $"Cannot find any streetcode with corresponding id: {id}";

            RepositorySetup(null);
            MapperSetup(null);

            var handler = new GetStreetcodeByIdHandler(_repository.Object, _mapper.Object, _mockLogger.Object);
            // act
            var result = await handler.Handle(new GetStreetcodeByIdQuery(id), CancellationToken.None);
            // assert
            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        private void RepositorySetup(StreetcodeContent streetcode)
        {
            _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(streetcode);
            _repository.Setup(repo => repo.StreetcodeTagIndexRepository.GetAllAsync(
               It.IsAny<Expression<Func<StreetcodeTagIndex, bool>>>(),
               It.IsAny<Func<IQueryable<StreetcodeTagIndex>,
               IIncludableQueryable<StreetcodeTagIndex, object>>>()))
               .ReturnsAsync(new List<StreetcodeTagIndex>());

        }

        private void MapperSetup(EventStreetcodeDTO streetcodeDTO)
        {
            _mapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<object>()))
                .Returns(streetcodeDTO);
        }
    }
}
