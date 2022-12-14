using Streetcode.DAL.Enums;

namespace Streetcode.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Assert.Equal(SubtitleStatus.Editor, SubtitleStatus.Illustrator);
    }
    
    [Fact]
    public void Test2()
    {
        Assert.Equal(SubtitleStatus.Editor, SubtitleStatus.Editor);
    }
}