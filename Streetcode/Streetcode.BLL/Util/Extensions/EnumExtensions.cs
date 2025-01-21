namespace Streetcode.BLL.Util.Extensions;

public static class EnumExtensions
{
    public static T Min<T>()
        where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().Min();
    }

    public static T Max<T>()
        where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().Max();
    }
}