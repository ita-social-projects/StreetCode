using Microsoft.Extensions.Localization;

namespace Streetcode.XUnitTest.Mocks;

public abstract class BaseMockStringLocalizer<TResource> : IStringLocalizer<TResource>
{
    private readonly Dictionary<int, List<string>> _groupedErrors;
    private readonly List<LocalizedString> _strings;

    protected BaseMockStringLocalizer()
    {
        _strings = new List<LocalizedString>();
        _groupedErrors = DefineGroupedErrors();
    }

    public LocalizedString this[string name]
    {
        get
        {
            if (_groupedErrors.TryGetValue(0, out var noArgumentErrors) && noArgumentErrors.Contains(name))
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
            if (_groupedErrors.TryGetValue(argumentsCount, out var argumentErrors) && argumentErrors.Contains(name))
            {
                return GetErrorMessage(name, arguments);
            }

            throw new ArgumentException($"Cannot find error message '{name}' that accepts {argumentsCount} arguments");
        }
    }

    protected abstract Dictionary<int, List<string>> DefineGroupedErrors();

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        return _strings;
    }

    private LocalizedString GetErrorMessage(string error, params object[] arguments)
    {
        if (arguments.Length == 0)
        {
            return new LocalizedString(error, $"Error '{error}'");
        }

        string formattedArguments = string.Join(", ", arguments);
        string errorMessage = $"Error '{error}'. Arguments: {formattedArguments}";

        return new LocalizedString(error, errorMessage);
    }
}
