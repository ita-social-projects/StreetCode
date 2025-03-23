using Streetcode.XIntegrationTest.ControllerTests.BaseController;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter
{
    public static class BaseExtracter
    {
        private static readonly object Lock = new object();
        private static SqlDbHelper _dbHelper = BaseControllerTests.GetSqlDbHelper();

        public static T Extract<T>(T entity, Func<T, bool> searchPredicate, bool hasIdentity = true)
            where T : class, new()
        {
            lock (Lock)
            {
                if (!_dbHelper.Any<T>(searchPredicate))
                {
                    if (hasIdentity)
                    {
                        _dbHelper.AddItemWithCustomId<T>(entity);
                    }
                    else
                    {
                        _dbHelper.AddNewItem<T>(entity);
                        _dbHelper.SaveChanges();
                    }
                }

                return _dbHelper.GetExistItem<T>(searchPredicate) !;
            }
        }

        public static void RemoveByPredicate<T>(Func<T, bool> searchPredicate)
            where T : class, new()
        {
            lock (Lock)
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
            lock (Lock)
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
