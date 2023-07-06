using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
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

            var splittedText = Pattern.Split(text).Where(x => !string.IsNullOrEmpty(x)).ToArray();

            foreach (var word in splittedText)
            {
                if (word.Contains('<') || string.IsNullOrWhiteSpace(word))
                {
                    _text.Append(word);
                    continue;
                }

                var (resultedWord, extras) = CleanWord(word);

                var term = await _repositoryWrapper.TermRepository
                    .GetFirstOrDefaultAsync(
                        t => t.Title.ToLower().Equals(resultedWord.ToLower()));

                if (term == null)
                {
                    var buffer = await AddRelatedAsync(resultedWord);
                    if (!string.IsNullOrEmpty(buffer))
                    {
                        resultedWord = buffer;
                    }
                }
                else
                {
                    resultedWord = MarkTermWithDescription(resultedWord, term.Description);
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
                rt => rt.Word.ToLower().Equals(clearedWord.ToLower()),
                rt => rt.Include(rt => rt.Term));

            if (relatedTerm == null || relatedTerm.Term == null)
            {
                return string.Empty;
            }

            return MarkTermWithDescription(clearedWord, relatedTerm.Term.Description);
        }

        private (string _clearedWord, string _extras) CleanWord(string word)
        {
            var clearedWord = word.Split('.', ',').First();

            var extras = string.Empty;

            if (!word.Equals(clearedWord))
            {
                extras = new string(word.Except(clearedWord).ToArray());
            }

            return (clearedWord, extras);
        }
    }
}
