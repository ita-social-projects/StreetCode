using AutoMapper;
using MimeKit;

namespace Streetcode.BLL.Mapping.Converters;

public class StringToDateTimeConverter : IValueConverter<string?, DateTime?>
{
    public DateTime? Convert(string? source, ResolutionContext context)
    {
        DateTime dateTime;

        if (source == null)
        {
            throw new ArgumentNullException();
        }

        if (DateTime.TryParse(source, out dateTime))
        {
            return dateTime;
        }

        throw new ArgumentException("Cannot parse date");
    }
}