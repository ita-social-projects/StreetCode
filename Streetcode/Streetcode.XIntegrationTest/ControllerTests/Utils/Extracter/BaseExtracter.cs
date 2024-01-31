
namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter
{
    public static class BaseExtracter
    {
        private static SqlDbHelper _dbHelper;

        static BaseExtracter()
        {
            _dbHelper = BaseControllerTests.GetSqlDbHelper();
        }

        public static T Extract<T>(T entity, Func<T, bool> searchPredicate)
            where T : class, new()
        {

            if (!_dbHelper.Any<T>(searchPredicate))
            {
                _dbHelper.AddNewItem<T>(entity);
                _dbHelper.SaveChanges();
            }

            return _dbHelper.GetExistItem<T>(searchPredicate);
        }

        public static void Remove<T>(T entity, Func<T, bool> searchPredicate)
            where T : class, new()
        {
            if (_dbHelper.Any<T>(searchPredicate))
            {
                _dbHelper.DeleteItem<T>(entity);
                _dbHelper.SaveChanges();
            }
        }
    }
}
