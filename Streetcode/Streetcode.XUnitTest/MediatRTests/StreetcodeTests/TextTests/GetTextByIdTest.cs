﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Text.GetById;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.StreetcodeTest.TextTest
{
    public class GetTextByIdTest
    {
        private Mock<IRepositoryWrapper> repository;
        private Mock<IMapper> mockMapper;

        public GetTextByIdTest()
        {
            repository = new Mock<IRepositoryWrapper>();
            mockMapper = new Mock<IMapper>();
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
            var handler = new GetTextByIdHandler(repository.Object, mockMapper.Object);

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

            var handler = new GetTextByIdHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetTextByIdQuery(2), CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.IsAssignableFrom<TextDTO>(result.Value);

        }
    }
}