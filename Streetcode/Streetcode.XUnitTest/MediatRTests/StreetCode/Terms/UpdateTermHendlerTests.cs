using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Term.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Terms
{
	public class UpdateTermHendlerTests
	{
		private Mock<IRepositoryWrapper> _mockRepository;
		private Mock<IMapper> _mockMapper;
        private readonly Mock<IStringLocalizer<FailedToUpdateSharedResource>> _mockLocalizerFailedToUpdate;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> _mockLocalizerCannotConvertNull;

        public UpdateTermHendlerTests()
		{
			_mockRepository = new();
			_mockMapper = new();
			_mockLocalizerFailedToUpdate = new Mock<IStringLocalizer<FailedToUpdateSharedResource>>();
			_mockLocalizerCannotConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
		}


		[Theory]
		[InlineData(1)]
		public async Task ShouldReturnSuccessfully_WhenUpdated(int returnNuber)
		{
			//Arrange
			_mockRepository.Setup(x => x.TermRepository.Update(GetTerm()));
			_mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);

			_mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDTO>()))
			.Returns(GetTerm());

			var handler = new UpdateTermHandler(_mockMapper.Object, _mockRepository.Object, _mockLocalizerFailedToUpdate.Object, _mockLocalizerCannotConvertNull.Object);

			//Act
			var result = await handler.Handle(new UpdateTermCommand(GetTermDTO()), CancellationToken.None);

			//Assert
			Assert.Multiple(
				() => Assert.True(result.IsSuccess),
				() => Assert.IsType<Unit>(result.Value)
			);

		}


		[Theory]
		[InlineData(1)]
		public async Task ShouldThrowExeption_TryMapNullRequest(int returnNuber)
		{
			//Arrange
			_mockRepository.Setup(x => x.TermRepository.Update(GetTermWithNotExistId()!));
			_mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);

			_mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDTO>()))
				.Returns(GetTermWithNotExistId()!);

			var expectedError = "Cannot convert null to Term";

			var handler = new UpdateTermHandler(_mockMapper.Object, _mockRepository.Object, _mockLocalizerFailedToUpdate.Object, _mockLocalizerCannotConvertNull.Object);

			//Act
			var result = await handler.Handle(new UpdateTermCommand(GetTermDTOWithNotExistId()!), CancellationToken.None);

			//Assert
			Assert.Multiple(
				() => Assert.False(result.IsSuccess),
				() => Assert.Equal(expectedError, result.Errors.First().Message)
			);
		}

		[Theory]
		[InlineData(-1)]
		public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful(int returnNuber)
		{
			//Arrange
			_mockRepository.Setup(x => x.TermRepository.Update(GetTerm()));
			_mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);

			_mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDTO>()))
				.Returns(GetTerm());

			var expectedError = "Failed to update a term";
			var handler = new UpdateTermHandler(_mockMapper.Object, _mockRepository.Object, _mockLocalizerFailedToUpdate.Object, _mockLocalizerCannotConvertNull.Object);

			//Act
			var result = await handler.Handle(new UpdateTermCommand(GetTermDTO()), CancellationToken.None);

			//Assert
			Assert.Multiple(
				() => Assert.True(result.IsFailed),
				() => Assert.Equal(expectedError, result.Errors.First().Message)
			); ;
		}

		[Theory]
		[InlineData(1)]
		public async Task ShouldReturnSuccessfully_TypeIsCorrect(int returnNuber)
		{
			//Arrange
			_mockRepository.Setup(x => x.TermRepository.Create(GetTerm()));
			_mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);

			_mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDTO>()))
				.Returns(GetTerm());

			var handler = new UpdateTermHandler(_mockMapper.Object, _mockRepository.Object, _mockLocalizerFailedToUpdate.Object, _mockLocalizerCannotConvertNull.Object);

			//Act
			var result = await handler.Handle(new UpdateTermCommand(GetTermDTO()), CancellationToken.None);

			//Assert
			Assert.IsType<Unit>(result.Value);
		}
		private static Term GetTerm() => new();
		private static TermDTO GetTermDTO() => new();
		private static Term? GetTermWithNotExistId() => null;
		private static TermDTO? GetTermDTOWithNotExistId() => null;
	}
}
