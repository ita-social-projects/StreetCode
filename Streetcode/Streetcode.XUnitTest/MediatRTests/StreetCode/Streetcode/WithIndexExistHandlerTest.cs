using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.WithIndexExist;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Streetcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class WithIndexExistHandlerTest
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        public WithIndexExistHandlerTest()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }
        [Fact]
        public async Task ShouldReturnSuccesfully()
        {
            // arrange
            _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(GetStreetCodeContent());

            var handler = new StreetcodeWithIndexExistHandler(_repository.Object, _mapper.Object);

            // act

            var result = await handler.Handle(new StreetcodeWithIndexExistQuery(1), CancellationToken.None);

            // assert

            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsAssignableFrom<Result<bool>>(result),
                () => Assert.True(result.Value)
                );
        }
        [Fact]
        public async Task ShouldReturnFalse_NotExistingId()
        {
            // arrange
            _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(GetNull());

            var handler = new StreetcodeWithIndexExistHandler(_repository.Object, _mapper.Object);

            // act

            var result = await handler.Handle(new StreetcodeWithIndexExistQuery(2), CancellationToken.None);

            // assert

            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsAssignableFrom<Result<bool>>(result),
                () => Assert.False(result.Value));
        }

        private StreetcodeContent GetStreetCodeContent()
        {
            return new StreetcodeContent() { Id = 1 };
        }

        private StreetcodeContent? GetNull()
        {
            return null;
        }
    }
}
