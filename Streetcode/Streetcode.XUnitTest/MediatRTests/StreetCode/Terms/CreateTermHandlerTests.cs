using AutoMapper;
using FluentResults;
using MediatR;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Term.Create;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Terms
{
	public class CreateTermHandlerTests
	{
		private Mock<IRepositoryWrapper> _mockRepository;
		private Mock<IMapper> _mockMapper;

		public CreateTermHandlerTests() 
		{
			_mockMapper = new();
			_mockRepository = new();
		}

		[Theory]
		[InlineData(1)]
		public async Task ShouldReturnSuccessfully_WhenTermAdded(int returnNumber) 
		{
        // Arrange
        var createdTerm = GetTerm();
        _mockRepository.Setup(x => x.TermRepository.Create(It.IsAny<Term>())).Returns(createdTerm);
        _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);

        _mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDTO>())).Returns(createdTerm);
        _mockMapper.Setup(x => x.Map<TermDTO>(createdTerm)).Returns(GetTermDTO());

        var handler = new CreateTermHandler(_mockMapper.Object, _mockRepository.Object);

        // Act
        var result = await handler.Handle(new CreateTermCommand(GetTermDTO()), CancellationToken.None);

  //Assert
        Assert.True(result.IsSuccess);
    }

		[Theory]
		[InlineData(1)]
		public async Task ShouldThrowException_WhenTryToAddNull(int returnNumber) 
		{
        //Arrange
        _mockRepository.Setup(x => x.TermRepository.Create(GetTerm()));
        _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);

        _mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDTO>()))
          .Returns(GetNotExistingTerm()!);

        var expectedError = "Cannot convert null to Term";
        var hendler = new CreateTermHandler(_mockMapper.Object, _mockRepository.Object);

        //Act
        var result = await hendler.Handle(new CreateTermCommand(GetNotExistingTermDTO()!), CancellationToken.None);

        //Assert
        Assert.Multiple(
          ()=>Assert.Equal(expectedError, result.Errors.First().Message),
          ()=>Assert.False(result.IsSuccess)
          );
		}

		private static Term GetTerm() => new();
		private static TermDTO GetTermDTO() => new();
		private static Term? GetNotExistingTerm() => null;
		private static TermDTO? GetNotExistingTermDTO() => null;
	}
}
