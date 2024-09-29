using Streetcode.XIntegrationTest.ControllerTests.BaseController;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter
{
    public static class BaseExtracter
    {
        private static readonly object @lock = new object();
        private static SqlDbHelper dbHelper = BaseControllerTests.GetSqlDbHelper();

        public static T Extract<T>(T entity, Func<T, bool> searchPredicate, bool hasIdentity = true)
            where T : class, new()
        {
            lock (@lock)
            {
                if (!dbHelper.Any<T>(searchPredicate))
                {
                    if (hasIdentity)
                    {
                        dbHelper.AddItemWithCustomId<T>(entity);
                    }
                    else
                    {
                        dbHelper.AddNewItem<T>(entity);
                        dbHelper.SaveChanges();
                    }
                }

                return dbHelper.GetExistItem<T>(searchPredicate) !;
            }
        }

        public static void RemoveByPredicate<T>(Func<T, bool> searchPredicate)
            where T : class, new()
        {
            lock (@lock)
            {
                var entityFromDb = dbHelper.GetExistItem<T>(searchPredicate);
                if (entityFromDb is not null)
                {
                    dbHelper.DeleteItem<T>(entityFromDb);
                    dbHelper.SaveChanges();
                }
            }
        }

        public static void RemoveById<T>(int id)
            where T : class, new()
        {
            lock (@lock)
            {
                var entityFromDb = dbHelper.GetExistItemId<T>(id);
                if (entityFromDb is not null)
                {
                    dbHelper.DeleteItem<T>(entityFromDb);
                    dbHelper.SaveChanges();
                }
            }
        }
    }
}
