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
                    "UserNameFormat",
                    "NameFormat",
                    "SurnameFormat",
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
                    "MustContainAtMostThreeExpertises",
                    "CannotFindAnyTermWithCorrespondingId",
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
                    "MinLength",
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