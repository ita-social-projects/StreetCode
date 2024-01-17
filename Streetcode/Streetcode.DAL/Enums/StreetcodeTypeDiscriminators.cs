namespace Streetcode.DAL.Enums
{
    public static class StreetcodeTypeDiscriminators
    {
        public static string StreetcodeBaseType { get => "streetcode-base"; }
        public static string StreetcodePersonType { get => "streetcode-person"; }
        public static string StreetcodeEventType { get => "streetcode-event"; }
        public static string DiscriminatorName { get => "StreetcodeType"; }

        public static string GetStreetcodeType(StreetcodeType streetcodeType)
        {
            switch (streetcodeType)
            {
                case StreetcodeType.Event: return StreetcodeEventType;
                case StreetcodeType.Person: return StreetcodePersonType;
                default: return StreetcodeBaseType;
            }
        }
    }
}
