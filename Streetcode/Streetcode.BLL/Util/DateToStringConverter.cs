using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Util
{
    public class DateToStringConverter
    {
        public static string FromDateToString(DateTime date, DateViewPattern pattern)
        {
            return pattern switch
            {
                DateViewPattern.Year => date.ToString("yyyy"),
                DateViewPattern.MonthYear => date.ToString("yyyy, MMMM"),
                DateViewPattern.SeasonYear => $"{GetSeason(date)} {date.Year}",
                DateViewPattern.DateMonthYear => date.ToString("yyyy, d MMMM"),
                _ =>""
            };
        }

        private static string GetSeason(DateTime dateTime)
        {
            if (dateTime.Month < 3 || dateTime.Month == 12)
            {
                return "зима";
            }
            else if (dateTime.Month >= 3 && dateTime.Month < 6)
            {
                return "весна";
            }
            else if(dateTime.Month >= 6 && dateTime.Month < 9)
            {
                return "літо";
            }
            else
            {
                return "осінь";
            }
        }
    }
}
