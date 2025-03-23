namespace Streetcode.XIntegrationTest.Base;

public static class UniqueNumberGenerator
{
    private static readonly object Lock = new object();

    public static int GenerateInt()
    {
        lock (Lock)
        {
            byte[] byteArr = Guid.NewGuid().ToByteArray();
            int uniqueNumber = BitConverter.ToInt32(byteArr);
            return uniqueNumber < 0 ? -uniqueNumber : uniqueNumber;
        }
    }

    public static int GenerateIntFromGuidInRange()
    {
        lock (Lock)
        {
            byte[] byteArray = Guid.NewGuid().ToByteArray();

            int generatedValue = BitConverter.ToInt32(byteArray, 0);

            int result = Math.Abs(generatedValue % 9999) + 1;

            return result;
        }
    }
}