using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockFailedToValidateLocalizer: IStringLocalizer<FailedToValidateSharedResource>
{
    private readonly Dictionary<string, string> errorMessages;
    private readonly List<LocalizedString> strings;

    public MockFailedToValidateLocalizer()
    {
        this.errorMessages = new Dictionary<string, string>()
        {
            { "IsRequired", "{0} is required." },
            { "MaxLength", "Length of {0} must be less than {1} characters long" },
            { "MustBeOneOf", "Value of {0} must be one of: {1}" },
        };
        this.strings = new List<LocalizedString>();
        foreach (var pair in this.errorMessages)
        {
            this.strings.Add(new LocalizedString(pair.Key, pair.Value));
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        return this.strings;
    }

    public LocalizedString this[string name] => new (name, this.errorMessages[name]);

    public LocalizedString this[string name, params object[] arguments] => new (name, string.Format(this.errorMessages[name], arguments));
}