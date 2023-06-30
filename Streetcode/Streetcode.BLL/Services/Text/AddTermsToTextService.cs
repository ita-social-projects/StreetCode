using System.Text.RegularExpressions;
using Streetcode.BLL.Interfaces.Text;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Text
{
    public class AddTermsToTextService : ITextService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly Regex _pattern = new("(\\s)|(<[^>]*>)");

        public AddTermsToTextService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<string> AddTermsTag(string text)
        {
            var resultText = string.Empty;

            var splittedText = _pattern.Split(text);

            foreach (var word in splittedText)
            {
                if (word.Contains('<') || word.Equals(" ") || word.Equals(string.Empty))
                {
                    resultText += word;
                    continue;
                }

                var clearedWord = word.Split('.', ',').First();

                var term = await _repositoryWrapper.TermRepository
                    .GetFirstOrDefaultAsync(
                        t => t.Title.Contains(clearedWord.ToLower()));

                var resultedWord = word;
                var extras = string.Empty;

                if (!resultedWord.Equals(clearedWord))
                {
                    extras = new string(resultedWord.Except(clearedWord).ToArray());
                    resultedWord = clearedWord;
                }

                if (term == null)
                {
                    var buffer = await AddRelatedAsync(clearedWord);
                    if (!buffer.Equals(string.Empty))
                    {
                        resultedWord = buffer;
                    }
                }
                else
                {
                    resultedWord = MarkTermWithDescription(term.Title, term.Description);
                }

                resultText += resultedWord + extras;
            }

            return resultText;
        }

        private static string MarkTermWithDescription(string word, string description) => $"<Popover><Term>{word}</Term><Desc>{description}</Desc></Popover>";

        private async Task<string> AddRelatedAsync(string clearedWord)
        {
            var relatedTerm = await _repositoryWrapper.RelatedTermRepository
                .GetFirstOrDefaultAsync(
                rt => rt.Word.Contains(clearedWord.ToLower()));

            if (relatedTerm == null)
            {
                return string.Empty;
            }

            var termToRelate = await _repositoryWrapper.TermRepository
                        .GetFirstOrDefaultAsync(t => t.Id == relatedTerm.TermId);

            if (termToRelate == null)
            {
                return string.Empty;
            }

            return MarkTermWithDescription(relatedTerm.Word, termToRelate.Description);
        }
    }
}
