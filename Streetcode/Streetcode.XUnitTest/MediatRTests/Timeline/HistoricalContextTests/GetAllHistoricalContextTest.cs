using Xunit;
using AutoMapper;
using Moq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetAll;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;
using Streetcode.DAL.Entities.Streetcode.TextContent;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests
{
	public class GetAllHistoricalContextTest
	{
		public Mock<IRepositoryWrapper> _mockRepository;
		public Mock<IMapper> _mockMapper;

		public GetAllHistoricalContextTest()
		{
			_mockRepository = new();
			_mockMapper = new();
		}

		[Fact]
		public async Task ShouldReturnSuccessfully_CorectType() 
		{
            //Arrange
            (_mockMapper, _mockRepository) = GetMapperAndRepo(_mockMapper, _mockRepository);
            var hendler = new GetAllHistoricalContextHandler(_mockRepository.Object, _mockMapper.Object);

			//Act
			var result = await hendler.Handle(new GetAllHistoricalContextQuery(), CancellationToken.None);

			//Assert
			Assert.Multiple(
				() => Assert.NotNull(result),
				() => Assert.IsType<List<HistoricalContextDTO>>(result.ValueOrDefault)
				);
		}

		[Fact]
		public async Task ShouldReturnSuccessfully_CountMatch() 
		{
			//Arrange
			(_mockMapper, _mockRepository) = GetMapperAndRepo(_mockMapper, _mockRepository);
			var hendler = new GetAllHistoricalContextHandler(_mockRepository.Object, _mockMapper.Object);
			
			//Act
			var result = await hendler.Handle(new GetAllHistoricalContextQuery(), CancellationToken.None);

			//Assert
			Assert.Multiple(
				() => Assert.NotNull(result),
				() => Assert.Equal(GetListHistoricalContext().Count(), result.Value.Count())
				);  
			}

		[Fact]
		public async Task ShouldThrowException_WhenNotFound() 
		{
			//Arrange
			_mockRepository.Setup(x => x.HistoricalContextRepository
				  .GetAllAsync(
					  It.IsAny<Expression<Func<HistoricalContext, bool>>>(),
						It.IsAny<Func<IQueryable<HistoricalContext>,
				  IIncludableQueryable<HistoricalContext, object>>>()))
				  .ReturnsAsync(GetNullListHistoricalContext());

			_mockMapper
				.Setup(x => x
				.Map<IEnumerable<HistoricalContextDTO>>(It.IsAny<IEnumerable<HistoricalContext>>()))
				.Returns(GetNullListHistoricalContextDTO()!);

			var expectedError = "Cannot find any historical contexts";
			var handler = new GetAllHistoricalContextHandler(_mockRepository.Object, _mockMapper.Object);

			//Act
			var result = await handler.Handle(new GetAllHistoricalContextQuery(), CancellationToken.None);

			//Assert
			Assert.Multiple(
				() => Assert.True(result.IsSuccess == false),
				() => Assert.Equal(expectedError, result.Errors.First().Message)
				);
		}

		private static (Mock<IMapper>, Mock<IRepositoryWrapper>) GetMapperAndRepo(
			Mock<IMapper> mockMapper,
			Mock<IRepositoryWrapper> mockRepo) 
		{
			mockRepo.Setup(x => x.HistoricalContextRepository
				.GetAllAsync(
					It.IsAny<Expression<Func<HistoricalContext, bool>>>(),
					It.IsAny<Func<IQueryable<HistoricalContext>,
						IIncludableQueryable<HistoricalContext, object>>>()))
				.ReturnsAsync(GetListHistoricalContext());

			mockMapper
				.Setup(x => x
				.Map<IEnumerable<HistoricalContextDTO>>
					(It.IsAny<IEnumerable<HistoricalContext>>()))
				.Returns(GetListHistoricalContextDTO());

			return (mockMapper, mockRepo);
		} 

		private static IQueryable<HistoricalContext> GetListHistoricalContext()
		{
			var historicalContexts = new List<HistoricalContext>() {
				new HistoricalContext{ Id = 1, Title = "HistoricalContext1"},
				new HistoricalContext{ Id = 2, Title = "HistoricalContext2"},
				new HistoricalContext{ Id = 3, Title = "HistoricalContext3"},
			};
			return historicalContexts.AsQueryable();
		}
		private static IEnumerable<HistoricalContextDTO> GetListHistoricalContextDTO()
		{
			var historicalContextsDTO = new List<HistoricalContextDTO>() {
				new HistoricalContextDTO{ Id = 1, Title = "HistoricalContext1"},
				new HistoricalContextDTO{ Id = 2, Title = "HistoricalContext2"},
				new HistoricalContextDTO{ Id = 3, Title = "HistoricalContext3"},
			};
			return historicalContextsDTO;
		}

		private static List<HistoricalContext>? GetNullListHistoricalContext() => null;
		private static List<HistoricalContextDTO>? GetNullListHistoricalContextDTO() => null;
	}
}
