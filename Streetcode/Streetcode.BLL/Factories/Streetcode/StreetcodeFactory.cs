using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Factories.Streetcode
{
    public static class StreetcodeFactory
    {
        public static StreetcodeContent CreateStreetcode(StreetcodeType type)
        {
            switch (type)
            {
                case StreetcodeType.Person:
                    return new PersonStreetcode();
                case StreetcodeType.Event:
                    return new EventStreetcode();
                default:
                    throw new ArgumentException($"Unsupported streetcode type: {type}", nameof(type));
            }
        }
    }
}
