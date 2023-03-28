using AutoMapper;
using AutoMapper.Internal;
using FluentAssertions;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Create;
using Streetcode.BLL.MediatR.Streetcode.Fact.Create;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Repositories.Interfaces.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;
using System;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class CreateTagHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;

        public CreateTagHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        async Task SetupRepository(Tag tag)
        {

            _mockRepo.Setup(repo => repo.TagRepository.CreateAsync(tag));
            _mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

        }

        async Task SetupMapper(Tag tag)
        {

            _mockMapper.Setup(x => x.Map<Tag>(It.IsAny<TagDTO>())).Returns(tag);

        }

        [Fact]
        public async Task ShouldReturnSuccessfully_TypeIsCorrect()
        {

            // Arrange

            await SetupRepository(GetTag());
            await SetupMapper(GetTag());

            var exception = new Exception("Database error.");

            _mockRepo.Setup(r => r.TagRepository.CreateAsync(GetTag())).ThrowsAsync(exception);

            var handler = new CreateTagHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new CreateTagQuery(CreateTagDTO()), CancellationToken.None);

            //Assert
            Assert.IsType<Unit>(result.Value);
        }

        private static Tag GetTag()
        {
            return new Tag();
        }

        private static TagDTO GetTagDTO()
        {
            return new TagDTO();
        }

        private static CreateTagDTO CreateTagDTO()
        {
            return new CreateTagDTO
            {
                Title = GetTag().Title
            };
        }


    }
}




















