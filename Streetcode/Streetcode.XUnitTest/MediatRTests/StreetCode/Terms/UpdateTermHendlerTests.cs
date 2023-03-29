using AutoMapper;
using MediatR;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Term.Update;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Terms
{
	public class UpdateTermHendlerTests
	{
		private Mock<IRepositoryWrapper> _mockRepository;
		private Mock<IMapper> _mockMapper;

		public UpdateTermHendlerTests()
		{
			_mockRepository = new();
			_mockMapper = new();
		}

		[Fact]
		public async Task ShouldReturnSuccessfully_WhenUpdated()
		{
			//Arrange
			_mockRepository.Setup(x => x.TermRepository.Update(GetTerm()));
			_mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

			_mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDTO>()))
			.Returns(GetTerm());

			var handler = new UpdateTermHandler(_mockMapper.Object, _mockRepository.Object);

			//Act
			var result = await handler.Handle(new UpdateTermCommand(GetTermDTO()), CancellationToken.None);

			//Assert
			Assert.Multiple(
				() => Assert.True(result.IsSuccess),
				() => Assert.IsType<Unit>(result.Value)
			);

		}

		[Fact]
		public async Task ShouldThrowExeption_TryMapNullRequest()
		{
			//Arrange
			_mockRepository.Setup(x => x.TermRepository.Update(GetTermWithNotExistId()!));
			_mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

			_mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDTO>()))
				.Returns(GetTermWithNotExistId()!);

			var expectedError = "Cannot convert null to Term";

			var handler = new UpdateTermHandler(_mockMapper.Object, _mockRepository.Object);

			//Act
			var result = await handler.Handle(new UpdateTermCommand(GetTermDTOWithNotExistId()!), CancellationToken.None);

			//Assert
			Assert.Multiple(
				() => Assert.False(result.IsSuccess),
				() => Assert.Equal(expectedError, result.Errors.First().Message)
			);
		}

		[Fact]
		public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful()
		{
			//Arrange
			_mockRepository.Setup(x => x.TermRepository.Update(GetTerm()));
			_mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(-1);

			_mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDTO>()))
				.Returns(GetTerm());

			var expectedError = "Failed to update a term";
			var handler = new UpdateTermHandler(_mockMapper.Object, _mockRepository.Object);

			//Act
			var result = await handler.Handle(new UpdateTermCommand(GetTermDTO()), CancellationToken.None);

			//Assert
			Assert.Multiple(
				() => Assert.True(result.IsFailed),
				() => Assert.Equal(expectedError, result.Errors.First().Message)
			); ;
		}

		[Fact]
		public async Task ShouldReturnSuccessfully_TypeIsCorrect()
		{
			//Arrange
			_mockRepository.Setup(x => x.TermRepository.Create(GetTerm()));
			_mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

			_mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDTO>()))
				.Returns(GetTerm());

			var handler = new UpdateTermHandler(_mockMapper.Object, _mockRepository.Object);

			//Act
			var result = await handler.Handle(new UpdateTermCommand(GetTermDTO()), CancellationToken.None);

			//Assert
			Assert.IsType<Unit>(result.Value);
		}
		private static Term GetTerm()
		{
			return new();
		}
		private static TermDTO GetTermDTO()
		{
			return new();
		}
		private static Term? GetTermWithNotExistId()
		{
			return null;
		}
		private static TermDTO? GetTermDTOWithNotExistId()
		{
			return null;
		}
	}
}
