
namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter
{
    public static class BaseExtracter
    {
        private static SqlDbHelper _dbHelper;
        private static object _lock;

        static BaseExtracter()
        {
            _dbHelper = BaseControllerTests.GetSqlDbHelper();
            _lock = new object();
        }

        public static T Extract<T>(T entity, Func<T, bool> searchPredicate)
            where T : class, new()
        {

            lock (_lock)
            {
                if (!_dbHelper.Any<T>(searchPredicate))
                {
                    _dbHelper.AddItemWithCustomId<T>(entity);
                }

                return _dbHelper.GetExistItem<T>(searchPredicate);
            }
        }

        public static void RemoveByPredicate<T>(Func<T, bool> searchPredicate)
            where T : class, new()
        {
            lock (_lock)
            {
                var entityFromDb = _dbHelper.GetExistItem<T>(searchPredicate);
                if (entityFromDb is not null)
                {
                    _dbHelper.DeleteItem<T>(entityFromDb);
                    _dbHelper.SaveChanges();
                }
            }
        }

        public static void RemoveById<T>(int id)
            where T : class, new()
        {
            lock (_lock)
            {
                var entityFromDb = _dbHelper.GetExistItemId<T>(id);
                if (entityFromDb is not null)
                {
                    _dbHelper.DeleteItem<T>(entityFromDb);
                    _dbHelper.SaveChanges();
                }
            }
        }
    }
}
