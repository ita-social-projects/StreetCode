using Moq;
using AutoMapper;
using Xunit;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByStreetcodeId;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.DTO.Streetcode;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Entities = Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.XUnitTest.MediatRTests.Streetcode.RelatedFigure
{
    public class GetRelatedFiguresByStreetcodeIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        public GetRelatedFiguresByStreetcodeIdHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }
        [Theory]
        [InlineData(1)]
        public async Task Handle_ExistingId_ReturnsSuccess(int id)
        {
            var testRelatedList = new List<Entities.RelatedFigure>()
            {
                new Entities.RelatedFigure()
            };

            var testPersonStreetcodeList = new List<PersonStreetcode>()
            {
                new PersonStreetcode()
            };

            var testRelatedDTOList = new List<RelatedFigureDTO>()
            {
                new RelatedFigureDTO()
            };

            RepositorySetup(testRelatedList.AsQueryable(), null);

            _repository.Setup(x => x.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync(testPersonStreetcodeList);

            _mapper.Setup(x => x.Map<IEnumerable<RelatedFigureDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(testRelatedDTOList);

            var handler = new GetRelatedFiguresByStreetcodeIdHandler(_mapper.Object, _repository.Object);

            var result = await handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(id), CancellationToken.None);

            Assert.Multiple(
                () => Assert.NotNull(result), 
                () => Assert.NotEmpty(result.Value));         
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsCorrectType(int id)
        {
            var testStreetcodeContentList = new List<StreetcodeContent>()
            {
                new StreetcodeContent()
            };

            var testRelatedFigureList = new List<Entities.RelatedFigure>() 
            { 
                new Entities.RelatedFigure() 
            };

            var testRelatedDTO = new RelatedFigureDTO() { Id = id };

            _repository.Setup(x => x.StreetcodeRepository
                .GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync(testStreetcodeContentList.AsQueryable());

            RepositorySetup(testRelatedFigureList.AsQueryable(), testStreetcodeContentList);
            MapperSetup(testRelatedDTO);

            var handler = new GetRelatedFiguresByStreetcodeIdHandler(_mapper.Object, _repository.Object);

            var result = await handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(id), CancellationToken.None);

            Assert.IsAssignableFrom<IEnumerable<RelatedFigureDTO>>(result.Value);
        }

        [Theory]
        [InlineData(2)]
        public async Task Handle_NonExisting_ReturnsGetAllNull(int id)
        {
            var testRelatedFigureEmptyList = new List<Entities.RelatedFigure>();
            var testRelatedDTO = new RelatedFigureDTO() { Id = id };
            string expectedErrorMessage = $"Cannot find any related figures by a streetcode id: {id}";

            RepositorySetup(testRelatedFigureEmptyList.AsQueryable(), null);
            MapperSetup(testRelatedDTO);

            var handler = new GetRelatedFiguresByStreetcodeIdHandler(_mapper.Object, _repository.Object);

            var result = await handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(id), CancellationToken.None);

            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        [Theory]
        [InlineData(2)]
        public async Task Handle_NonExisting_ReturnsFindAllNull(int id)
        {
            string expectedErrorMessage = $"Cannot find any related figures by a streetcode id: {id}";
            var testRelatedDTO = new RelatedFigureDTO() { Id = id };

            RepositorySetup(null, null);
            MapperSetup(testRelatedDTO);

            var handler = new GetRelatedFiguresByStreetcodeIdHandler(_mapper.Object, _repository.Object);

            var result = await handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(id), CancellationToken.None);

            Assert.Equal(expectedErrorMessage, result.Errors.First().Message);
        }

        private void RepositorySetup(IQueryable<Entities.RelatedFigure> relatedFigures, List<StreetcodeContent> streetcodeContents)
        {
            _repository.Setup(x => x.RelatedFigureRepository
                .FindAll(It.IsAny<Expression<Func<Entities.RelatedFigure, bool>>?>()))
                .Returns(relatedFigures);

            _repository.Setup(x => x.StreetcodeRepository
                .GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync(streetcodeContents);

            _repository.Setup(x => x.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync(streetcodeContents);
        }

        private void MapperSetup(RelatedFigureDTO relatedFigureDTO)
        {
            _mapper.Setup(x => x.Map<RelatedFigureDTO>(It.IsAny<StreetcodeContent>()))
                .Returns(relatedFigureDTO);
        }
    }
}
