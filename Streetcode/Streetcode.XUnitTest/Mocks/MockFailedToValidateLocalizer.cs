using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockFailedToValidateLocalizer : BaseMockStringLocalizer<FailedToValidateSharedResource>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        var groupedErrors = new Dictionary<int, List<string>>
        {
            {
                0, new List<string>()
                {
                    "LogoMustMatchUrl",
                    "EmailAddressFormat",
                    "TransliterationUrlFormat",
                    "InvalidNewsUrl",
                    "DateStringFormat",
                    "EventStreetcodeCannotHasFirstName",
                    "InvalidPaginationParameters",
                }
            },
            {
                1, new List<string>()
                {
                    "CannotBeEmpty",
                    "ValidUrl",
                    "IsRequired",
                    "Invalid",
                    "MustBeUnique",
                    "ImageDoesntExist",
                    "InvalidPrecision",
                    "MustContainExactlyOneAlt1",
                    "MustContainAtMostOneAlt0",
                    "MustContainAtMostOneAlt2",
                    "ImageSizeExceeded",
                }
            },
            {
                2, new List<string>()
                {
                    "CannotBeEmptyWithCondition",
                    "ValidUrl_UrlDisplayed",
                    "GreaterThan",
                    "MustBeOneOf",
                    "MaxLength",
                }
            },
            {
                3, new List<string>()
                {
                    "LengthMustBeInRange",
                    "MustBeBetween",
                }
            },
        };
        return groupedErrors;
    }
}