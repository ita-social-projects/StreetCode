using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.MediatR.Streetcode.Term.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Terms
{
	public class DeleteTermHendlerTests
	{
		private Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IStringLocalizer<FailedToDeleteSharedResource>> _mockLocalizerFailedToCreate;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> _mockLocalizerCannotConvertNull;

        public DeleteTermHendlerTests()
		{
			_mockRepository = new();
			_mockLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToDeleteSharedResource>>();
			_mockLocalizerCannotConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
		}

		[Theory]
		[InlineData(-1,1)]
		public async Task ShouldDeleteSuccessfully(int id, int returnNuber) 
		{
			//Arrange
			MockRepoInitial_GetFirstOrDefault_Delete(_mockRepository, id, true);
			_mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);
			var handler = new DeleteTermHandler(_mockRepository.Object, _mockLocalizerFailedToCreate.Object,_mockLocalizerCannotConvertNull.Object);

			//Act
			var result = await handler.Handle(new DeleteTermCommand(id), CancellationToken.None);

			//Assert
			Assert.Multiple(
				() => Assert.NotNull(result),
				() => Assert.True(result.IsSuccess)
			);
		}

		[Theory]
		[InlineData(2)]
		public async Task ShouldThrowExeption_IdNotExisting(int id) 
		{
			//Arrange
			MockRepoInitial_GetFirstOrDefault_Delete(_mockRepository, id, false);

			var expectedError = "Cannot convert null to Term";
			var handler = new DeleteTermHandler(_mockRepository.Object, _mockLocalizerFailedToCreate.Object, _mockLocalizerCannotConvertNull.Object);

			//Act
			var result = await handler.Handle(new DeleteTermCommand(id), CancellationToken.None);

			//Assert
			Assert.Equal(expectedError, result.Errors.First().Message);
		}

		[Theory]
		[InlineData(2,0)]
		public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful(int id,int returnNuber) 
		{
			//Arange
			MockRepoInitial_GetFirstOrDefault_Delete(_mockRepository, id, true);
			_mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);

			var expectedError = "Failed to delete a term";
			var hendler = new DeleteTermHandler(_mockRepository.Object, _mockLocalizerFailedToCreate.Object, _mockLocalizerCannotConvertNull.Object);

			//Act
			var result = await hendler.Handle(new DeleteTermCommand(id), CancellationToken.None);

			//Assert
			Assert.Equal(expectedError, result.Errors.First().Message);
		}

		private static Mock<IRepositoryWrapper> MockRepoInitial_GetFirstOrDefault_Delete(
			Mock<IRepositoryWrapper> mockRepo, int id, bool IsIdExist)
		{
			mockRepo.Setup(x => x.TermRepository
				.GetFirstOrDefaultAsync(
					It.IsAny<Expression<Func<Term, bool>>>(),
					It.IsAny<Func<IQueryable<Term>,
						IIncludableQueryable<Term, object>>>()))
				.ReturnsAsync(IsIdExist ? GetTerm(id): GetNotExistingTerm()!);

			mockRepo.Setup(x => x.TermRepository.Delete(IsIdExist ? GetTerm(id) : GetNotExistingTerm()!));			

			return mockRepo;
		} 

		private static Term GetTerm(int id) => new() { Id = id };

		private static Term? GetNotExistingTerm() => null;
	}
}
