using Streetcode.BLL.Factories.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Enums;
using Xunit;

namespace Streetcode.XUnitTest.Factories.Streetcode;

public class StreetcodeFactoryTests
{
    [Fact]
    public void CreateStreetcode_StreetcodeTypePerson_ReturnsPersonStreetcode()
    {
        // Arrange
        var streetcodeTypePerson = StreetcodeType.Person;

        // Act
        var streetcode = StreetcodeFactory.CreateStreetcode(streetcodeTypePerson);

        // Assert
        Assert.IsType<PersonStreetcode>(streetcode);
    }

    [Fact]
    public void CreateStreetcode_StreetcodeTypeEvent_ReturnsEventStreetcode()
    {
        // Arrange
        var streetcodeTypeEvent = StreetcodeType.Event;

        // Act
        var streetcode = StreetcodeFactory.CreateStreetcode(streetcodeTypeEvent);

        // Assert
        Assert.IsType<EventStreetcode>(streetcode);
    }
}