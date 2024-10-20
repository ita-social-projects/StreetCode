using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockFailedToValidateLocalizer : IStringLocalizer<FailedToValidateSharedResource>
{
    private readonly Dictionary<int, List<string>> groupedErrors;
    private readonly List<LocalizedString> strings;

    public MockFailedToValidateLocalizer()
    {
        this.strings = new List<LocalizedString>();
        this.groupedErrors = new Dictionary<int, List<string>>();
        this.groupedErrors.Add(0, new List<string>()
        {
            "LogoMustMatchUrl",
            "EmailAddressFormat",
            "TransliterationUrlFormat",
            "InvalidNewsUrl",
            "DateStringFormat",
            "EventStreetcodeCannotHasFirstName",
        });

        this.groupedErrors.Add(1, new List<string>()
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
        });

        this.groupedErrors.Add(2, new List<string>()
        {
            "CannotBeEmptyWithCondition",
            "ValidUrl_UrlDisplayed",
            "GreaterThan",
            "MustBeOneOf",
            "MaxLength",
        });

        this.groupedErrors.Add(3, new List<string>()
        {
            "LengthMustBeInRange",
            "MustBeBetween",
        });
    }

    public LocalizedString this[string name]
    {
        get
        {
            var isContains = this.groupedErrors[0].Contains(name);
            if (isContains)
            {
                return new LocalizedString(name, $"Error '{name}'");
            }

            throw new ArgumentException($"Cannot find error message '{name}' that accepts no arguments");
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var argumentsCount = arguments.Length;
            if (argumentsCount > 0)
            {
                var isContains = this.groupedErrors[argumentsCount].Contains(name);
                if (isContains)
                {
                    return this.GetErrorMessage(name, arguments);
                }
            }

            throw new ArgumentException($"Cannot find error message '{name}' that accepts {argumentsCount} arguments");
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        return this.strings;
    }

    private LocalizedString GetErrorMessage(string error, params object[] arguments)
    {
        string errorMessage = $"Error '{error}'";
        switch (arguments.Length)
        {
            case 1:
                errorMessage += ". Arguments: {0}";
                break;
            case 2:
                errorMessage += ". Arguments: {0}, {1}";
                break;
            case 3:
                errorMessage += ". Arguments: {0}, {1}, {2}";
                break;
            default:
                throw new ArgumentException("Not supported number of arguments");
        }

        return new LocalizedString(error, string.Format(errorMessage, arguments));
    }
}