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
            var testContentDTO = new EventStreetcodeDTO();
            var testContent = new StreetcodeContent();

            RepositorySetup(testContent);
            MapperSetup(testContentDTO);

            var handler = new GetStreetcodeByIdHandler(_repository.Object, _mapper.Object);

            var result = await handler.Handle(new GetStreetcodeByIdQuery(id), CancellationToken.None);

            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsCorrectType(int id)
        {
            var testContentDTO = new EventStreetcodeDTO();
            var testContent = new StreetcodeContent();

            RepositorySetup(testContent);
            MapperSetup(testContentDTO);

            var handler = new GetStreetcodeByIdHandler(_repository.Object, _mapper.Object);

            var result = await handler.Handle(new GetStreetcodeByIdQuery(id), CancellationToken.None);

            Assert.IsAssignableFrom<StreetcodeDTO>(result.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsError(int id)
        {
            string expectedErrorMessage = $"Cannot find a streetcode with corresponding id: {id}";

            RepositorySetup(null);
            MapperSetup(null);

            var handler = new GetStreetcodeByIdHandler(_repository.Object, _mapper.Object);

            var result = await handler.Handle(new GetStreetcodeByIdQuery(id), CancellationToken.None);

            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        private void RepositorySetup(StreetcodeContent streetcode)
        {
            _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(streetcode);
        }

        private void MapperSetup(EventStreetcodeDTO streetcodeDTO)
        {
            _mapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<object>()))
                .Returns(streetcodeDTO);
        }
    }
}
