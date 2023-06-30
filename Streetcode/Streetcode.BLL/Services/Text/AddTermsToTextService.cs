using System.Text;
using System.Text.RegularExpressions;
using Streetcode.BLL.Interfaces.Text;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Text
{
    public class AddTermsToTextService : ITextService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        private readonly StringBuilder _text = new StringBuilder();

        public AddTermsToTextService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            Pattern = new("(\\s)|(<[^>]*>)", RegexOptions.None, TimeSpan.FromMilliseconds(1000));
        }

        public Regex Pattern { get; private set; }

        public async Task<string> AddTermsTag(string text)
        {
            _text.Clear();

            var splittedText = Pattern.Split(text);

            foreach (var word in splittedText)
            {
                if (word.Contains('<') || string.IsNullOrWhiteSpace(word) || string.IsNullOrEmpty(word))
                {
                    _text.Append(word);
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
                    if (!string.IsNullOrEmpty(buffer))
                    {
                        resultedWord = buffer;
                    }
                }
                else
                {
                    resultedWord = MarkTermWithDescription(term.Title, term.Description);
                }

                _text.Append(resultedWord + extras);
            }

            return _text.ToString();
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
