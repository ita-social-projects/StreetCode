using System.Text;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Util.Helpers;

public static class UserHelper
{
    public static string EmailToUserNameConverter(User user)
    {
        var cleanEmail = RemoveNonAlphaNumericFromEmail(user.Email);

        var randomSuffix = Guid.NewGuid().ToString("N").Substring(0, 8);

        return (cleanEmail + randomSuffix).ToLower();
    }

    private static string RemoveNonAlphaNumericFromEmail(string email)
    {
        var sb = new StringBuilder();
        foreach (var character in email)
        {
            if (char.IsLetterOrDigit(character))
            {
                sb.Append(character);
            }
        }

        return sb.ToString();
    }
}