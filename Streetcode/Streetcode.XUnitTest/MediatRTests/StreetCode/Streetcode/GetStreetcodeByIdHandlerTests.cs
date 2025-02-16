using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetById;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetStreetcodeByIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;

        public GetStreetcodeByIdHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsSuccess(int id)
        {
            // Arrange
            var testContentDto = new EventStreetcodeDTO();
            var testContent = new StreetcodeContent();

            RepositorySetup(testContent);
            MapperSetup(testContentDto);

            var handler = new GetStreetcodeByIdHandler(_repository.Object, _mapper.Object);

            // act
            var result = await handler.Handle(new GetStreetcodeByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsCorrectType(int id)
        {
            // arrange
            var testContentDto = new EventStreetcodeDTO();
            var testContent = new StreetcodeContent();

            RepositorySetup(testContent);
            MapperSetup(testContentDto);

            var handler = new GetStreetcodeByIdHandler(_repository.Object, _mapper.Object);

            // act
            var result = await handler.Handle(new GetStreetcodeByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.IsAssignableFrom<StreetcodeDTO>(result.Value);
        }

        private void RepositorySetup(StreetcodeContent? streetcode)
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

        private void MapperSetup(EventStreetcodeDTO? streetcodeDto)
        {
            _mapper.Setup(x => x.Map<StreetcodeDTO?>(It.IsAny<object>()))
                .Returns(streetcodeDto);
        }
    }
}
