using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByIndex;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetById;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class GetStreetcodeByIdTests
    {
        [Theory]
        [InlineData(1)]
        public async Task GetStreetcodeById_Success(int id) 
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testContentDTO = new EventStreetcodeDTO();
            var testContent = new StreetcodeContent();

            repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(testContent);

            mockMapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<object>()))
            .Returns(testContentDTO);

            var handler = new GetStreetcodeByIdHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetStreetcodeByIdQuery(id), CancellationToken.None);

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetStreetcodeById_TypeCheck(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testContentDTO = new EventStreetcodeDTO();
            var testContent = new StreetcodeContent();

            repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(testContent);

            mockMapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<object>()))
            .Returns(testContentDTO);

            var handler = new GetStreetcodeByIdHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetStreetcodeByIdQuery(id), CancellationToken.None);

            Assert.IsAssignableFrom<StreetcodeDTO>(result.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetStreetcodeById_Error(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync((StreetcodeContent)null);

            mockMapper.Setup(x => x.Map<StreetcodeDTO>(It.IsAny<object>()))
                .Returns((EventStreetcodeDTO)null);

            var handler = new GetStreetcodeByIdHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetStreetcodeByIdQuery(id), CancellationToken.None);

            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
        }
    }
}
