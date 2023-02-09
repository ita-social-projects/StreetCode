using Moq;
using AutoMapper;
using Xunit;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByStreetcodeId;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.DTO.Streetcode;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

using Entities = Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.XUnitTest.MediatRTests.Streetcode.RelatedFigure
{

    public class GetRelatedFiguresByStreetcodeIdTests
    {
        [Theory]
        [InlineData(1)]
        public async Task GetRelatedFigureByStreetcodeId_Existing(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testRelatedList = new List<Entities.RelatedFigure>()
            {
                    new Entities.RelatedFigure()
                    {
                        ObserverId = id,
                        TargetId = id + 1,
                        Observer = new StreetcodeContent(){},
                        Target = new StreetcodeContent(){}
                    }
            };

            var testPersonStreetcodeList = new List<PersonStreetcode>()
            {
                new PersonStreetcode()
                {
                    FirstName= "Test",
                    LastName = "TestSurname",
                    Id = id,
                    Index = id,
                    Teaser = "test",
                    EventStartOrPersonBirthDate = DateTime.UtcNow,
                    EventEndOrPersonDeathDate = DateTime.UtcNow.AddDays(1),
                }   
            };

            var testRelatedDTOList = new List<RelatedFigureDTO>()
            {
                new RelatedFigureDTO() { Id = id }
            };



            repository.Setup(x => x.RelatedFigureRepository
                .FindAll(It.IsAny<Expression<Func<Entities.RelatedFigure, bool>>?>()))
                .Returns(testRelatedList.AsQueryable());

            repository.Setup(x => x.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync(testPersonStreetcodeList);

            mockMapper.Setup(x => x.Map<IEnumerable<RelatedFigureDTO>>(It.IsAny<IEnumerable<object>>()))
            .Returns(testRelatedDTOList);

            var handler = new GetRelatedFiguresByStreetcodeIdHandler(mockMapper.Object, repository.Object);

            var result = await handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(id), CancellationToken.None);

            Assert.NotNull(result.Value);
            Assert.NotEmpty(result.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetRelatedFigureByStreetcodeId_Type_Check(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testStreetcodeContentList = new List<StreetcodeContent>()
            {
                new StreetcodeContent()
                {
                    Id = 1,
                    Index = 1,
                    Teaser = "test",
                    EventStartOrPersonBirthDate = DateTime.UtcNow,
                    EventEndOrPersonDeathDate = DateTime.UtcNow.AddDays(1),
                    Images = new List<Image>{ new Image() { Id = 1} },
                    Tags = new List<Tag>{new Tag() { Id = 1} }
                }
            };

            var testRelatedFigureList = new List<Entities.RelatedFigure>()
            {
                new Entities.RelatedFigure()
                {
                    ObserverId = 1,
                    TargetId = 1
                }
            };

            repository.Setup(x => x.StreetcodeRepository
                .GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync(testStreetcodeContentList.AsQueryable());

            repository.Setup(x => x.RelatedFigureRepository
                .FindAll(It.IsAny<Expression<Func<Entities.RelatedFigure, bool>>?>()))
                .Returns(testRelatedFigureList.AsQueryable());

            mockMapper.Setup(x => x.Map<RelatedFigureDTO>(It.IsAny<StreetcodeContent>()))
                .Returns((StreetcodeContent sourceContent) =>
                {
                    return new RelatedFigureDTO { Id = sourceContent.Id };
                });

            var handler = new GetRelatedFiguresByStreetcodeIdHandler(mockMapper.Object, repository.Object);

            var result = await handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(id), CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.IsAssignableFrom<IEnumerable<RelatedFigureDTO>>(result.Value);
        }

        [Theory]
        [InlineData(2)]
        public async Task GetRelatedFigureByStreetcodeId_NonExisting_GetAllAsyncNull(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testRelatedFigureEmptyList = new List<Entities.RelatedFigure>();

            repository.Setup(x => x.StreetcodeRepository
                .GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync((List<StreetcodeContent>)null);


            repository.Setup(x => x.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync((List<StreetcodeContent>)null);

            mockMapper.Setup(x => x.Map<RelatedFigureDTO>(It.IsAny<StreetcodeContent>()))
                .Returns((StreetcodeContent sourceContent) =>
                {
                    return new RelatedFigureDTO { Id = sourceContent.Id };
                });

            repository.Setup(x => x.RelatedFigureRepository
                .FindAll(It.IsAny<Expression<Func<Entities.RelatedFigure, bool>>?>()))
                .Returns(testRelatedFigureEmptyList.AsQueryable());

            var handler = new GetRelatedFiguresByStreetcodeIdHandler(mockMapper.Object, repository.Object);

            var result = await handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(id), CancellationToken.None);

            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Errors);
            Assert.Equal($"Cannot find any related figures by a streetcode id: {id}", result.Errors.First().Message);
        }

        [Theory]
        [InlineData(2)]
        public async Task GetRelatedFigureByStreetcodeId_NonExisting_FindAll_Null(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            repository.Setup(x => x.RelatedFigureRepository
                .FindAll(It.IsAny<Expression<Func<Entities.RelatedFigure, bool>>?>()))
                .Returns((IQueryable<Entities.RelatedFigure>)null);

            repository.Setup(x => x.StreetcodeRepository
                .GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync((List<StreetcodeContent>)null);

            repository.Setup(x => x.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync((List<StreetcodeContent>)null);

            mockMapper.Setup(x => x.Map<RelatedFigureDTO>(It.IsAny<StreetcodeContent>()))
                .Returns((StreetcodeContent sourceContent) =>
                {
                    return new RelatedFigureDTO { Id = sourceContent.Id };
                });

            var handler = new GetRelatedFiguresByStreetcodeIdHandler(mockMapper.Object, repository.Object);

            var result = await handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(id), CancellationToken.None);

            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Errors);
            Assert.Equal($"Cannot find any related figures by a streetcode id: {id}", result.Errors.First().Message);
        }


    }
}
