namespace Streetcode.BLL.Validators.Common;

public class DateValidator
{
    public static bool IsValidDate(string? value)
    {
        if (value == null)
        {
            return true;
        }

        return DateTime.TryParse(value, out _);
    }
}