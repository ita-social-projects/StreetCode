using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockFieldNamesLocalizer: IStringLocalizer<FieldNamesSharedResource>
{
    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        throw new NotImplementedException();
    }

    public LocalizedString this[string name] => new (name, name);

    public LocalizedString this[string name, params object[] arguments] => new (name, name);
}