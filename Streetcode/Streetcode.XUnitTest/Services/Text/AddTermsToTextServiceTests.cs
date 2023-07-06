using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.Services.Text;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.Services.Text
{
    public class AddTermsToTextServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;

        public AddTermsToTextServiceTests()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
        }

        [Theory]
        [InlineData("S")]
        [InlineData("Sample")]
        [InlineData("SampleLonger")]
        public async Task ShouldReturnPopupTag_Success(string inputText)
        {
            string expectedOutput = $"<Popover><Term>{inputText}</Term><Desc>Sample Description</Desc></Popover>";
            SetupRepository(GetTerm(), GetRelatedTerm());
            var service = new AddTermsToTextService(_mockRepository.Object);

            // Act
            var result = await service.AddTermsTag(inputText);

            // Assert
            Assert.Equal(expectedOutput, result);
        }

        [Theory]
        [InlineData("SampleNull")]
        public async Task ShouldReturnText_NullTermAndRelatedTerm(string inputText)
        {
            string expectedOutput = inputText;
            SetupRepository(null, null);

            var service = new AddTermsToTextService(_mockRepository.Object);

            // Act
            var result = await service.AddTermsTag(inputText);

            // Assert
            Assert.Equal(expectedOutput, result);
        }
        [Theory]
        [InlineData("SampleRelated")]
        public async Task ShouldReturnRelatedWordWithDefinition(string inputText)
        {
            string expectedOutput = $"<Popover><Term>{inputText}</Term><Desc>Desc from term for related</Desc></Popover>"; ;
            SetupRepository(null, GetRelatedTerm());

            var service = new AddTermsToTextService(_mockRepository.Object);

            // Act
            var result = await service.AddTermsTag(inputText);

            // Assert
            Assert.Equal(expectedOutput, result);
        }

        private void SetupRepository(Term? term, RelatedTerm? relatedTerm)
        {
            _mockRepository.Setup(repo => repo.RelatedTermRepository.GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<RelatedTerm, bool>>>(),
                It.IsAny<Func<IQueryable<RelatedTerm>,
                IIncludableQueryable<RelatedTerm, object>>>()))
            .ReturnsAsync(relatedTerm);

            _mockRepository.Setup(repo => repo.TermRepository.GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Term, bool>>>(),
                It.IsAny<Func<IQueryable<Term>,
                IIncludableQueryable<Term, object>>>()))
            .ReturnsAsync(term);
        }

        private static Term GetTerm(string title = "Sample Term", string description = "Sample Description")
            => new() { Id = 1, Description = description, Title = title, RelatedTerms = new List<RelatedTerm>() };

        private static RelatedTerm GetRelatedTerm(string title = "Sample Related Term")
            => new() { Id = 1, Term = GetTerm("Do not show title", "Desc from term for related"), TermId = 1, Word = title };
    }
}
