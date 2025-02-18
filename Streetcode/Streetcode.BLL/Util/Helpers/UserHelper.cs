using System.Text;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Util.Helpers;

public static class UserHelper
{
    public static string EmailToUserNameConverter(User user)
    {
        var cleanEmail = RemoveNonAlphaNumericFromEmail(user.Email!);

        var randomSuffix = Guid.NewGuid().ToString("N").Substring(0, 8);

        return (cleanEmail + randomSuffix).ToLower();
    }

    private static string RemoveNonAlphaNumericFromEmail(string email)
    {
        var beforeAtSymbol = email.Split("@")[0];
        var onlyLettersOrDigits = beforeAtSymbol.Where(char.IsLetterOrDigit);
        var cleanEmail = string.Concat(onlyLettersOrDigits);

        return cleanEmail;
    }
}