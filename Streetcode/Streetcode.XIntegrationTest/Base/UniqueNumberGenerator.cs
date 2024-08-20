namespace Streetcode.XIntegrationTest.Base
{
    public static class UniqueNumberGenerator
    {
        private static readonly object _lock = new object();

        public static int GenerateInt()
        {
            lock (_lock)
            {
                byte[] byte_arr = Guid.NewGuid().ToByteArray();
                int uniqueNumber = BitConverter.ToInt32(byte_arr);
                return uniqueNumber < 0 ? -uniqueNumber : uniqueNumber;
            }
        }
    }
}
