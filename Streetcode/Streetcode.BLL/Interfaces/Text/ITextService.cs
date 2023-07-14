namespace Streetcode.BLL.Interfaces.Text
{
    public interface ITextService
    {
        Task<string> AddTermsTag(string text);
    }
}
