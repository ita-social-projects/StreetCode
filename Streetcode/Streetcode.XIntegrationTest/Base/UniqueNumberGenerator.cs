namespace Streetcode.XIntegrationTest.Base
{
    public static class UniqueNumberGenerator
    {
        private static readonly object @lock = new object();

        public static int GenerateInt()
        {
            lock (@lock)
            {
                byte[] byte_arr = Guid.NewGuid().ToByteArray();
                int uniqueNumber = BitConverter.ToInt32(byte_arr);
                return uniqueNumber < 0 ? -uniqueNumber : uniqueNumber;
            }
        }

        public static int GenerateIntFromGuidInRange()
        {
            lock (@lock)
            {
                byte[] byteArray = Guid.NewGuid().ToByteArray();

                int generatedValue = BitConverter.ToInt32(byteArray, 0);

                int result = Math.Abs(generatedValue % 9999) + 1;

                return result;
            }
        }
    }
}
