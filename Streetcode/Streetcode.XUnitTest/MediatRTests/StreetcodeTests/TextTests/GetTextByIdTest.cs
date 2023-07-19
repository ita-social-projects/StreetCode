using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.MediatR.Streetcode.Text.GetById;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.Interfaces.Logging;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.StreetcodeTest.TextTest
{
  public class GetTextByIdTest
    {
        private readonly Mock<IRepositoryWrapper> repository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;


        public GetTextByIdTest()
        {
            repository = new Mock<IRepositoryWrapper>();
            mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();   
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(2)]
        public async Task GetTextByIdSuccess(int id)
        {

            var testText = new Text() { Id = id };

            repository.Setup(repo => repo.TextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Text, bool>>>(),
                It.IsAny<Func<IQueryable<Text>, IIncludableQueryable<Text, Text>>?>()))
            .ReturnsAsync(testText);

            mockMapper.Setup(x => x.Map<TextDTO>(It.IsAny<Text>())).Returns((Text sourceText) =>
            {
                return new TextDTO { Id = sourceText.Id };
            });
            var handler = new GetTextByIdHandler(repository.Object, mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            var result = await handler.Handle(new GetTextByIdQuery(id), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(id, result.Value.Id);

        }

        [Theory]
        [InlineData(1)]
        public async Task GetTextByIdTypeCheck(int id)
        {

            var testText = new Text() { Id = id };

            repository.Setup(repo => repo.TextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Text, bool>>>(),
                It.IsAny<Func<IQueryable<Text>, IIncludableQueryable<Text, Text>>?>()))
            .ReturnsAsync(testText);

            mockMapper.Setup(x => x.Map<TextDTO>(It.IsAny<Text>())).Returns((Text sourceText) =>
            {
                return new TextDTO { Id = sourceText.Id };
            });

            var handler = new GetTextByIdHandler(repository.Object, mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            var result = await handler.Handle(new GetTextByIdQuery(2), CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.IsAssignableFrom<TextDTO>(result.Value);

        }
    }
}