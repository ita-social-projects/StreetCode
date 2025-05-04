using AutoMapper;

namespace Streetcode.BLL.Mapping.Converters;

public class StringToDateTimeConverter : IValueConverter<string?, DateTime?>
{
    public DateTime? Convert(string? source, ResolutionContext context)
    {
        if (source == null)
        {
            throw new ArgumentNullException(source);
        }

        if (DateTime.TryParse(source, out var dateTime))
        {
            return dateTime;
        }

        throw new ArgumentException("Cannot parse date");
    }
}