namespace Streetcode.BLL.Services
{
    public static class DummyTestService
    {
        private static int d;
        static DummyTestService()
        {
            d = 10;
        }

        public static void Output()
        {
            for (int i = 0; i < d; i++)
            {
                Console.WriteLine(i);
            }
        }
    }
}
