using System.Diagnostics.CodeAnalysis;

namespace Streetcode.XIntegrationTest.Constants.ControllerTests
{
    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1310:Field names should not contain underscore",
        Justification = "Underscores in constants make them more readable")]
    public static class StreetcodeConstants
    {
        public const int STREETCODE_INDEX = 9990;
        public const int STREETCODE_CREATE_INDEX = 9991;
        public const int STREETCODE_WITH_AUDIO_INDEX = 9992;
        public const int STREETCODE_RELATED_FIGURE_OBSERVER_INDEX = 9993;
        public const int STREETCODE_RELATED_FIGURE_TARGET_INDEX = 9994;
    }
}
