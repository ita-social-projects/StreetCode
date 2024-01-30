using System.Runtime.CompilerServices;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter
{
    public static class Extracter
    {
        private static SqlDbHelper _dbHelper;

        static Extracter()
        {
            _dbHelper = BaseControllerTests.GetSqlDbHelper();
        }

        public static T Extract<T>(T entity, Func<T, bool>? predicate = null)
            where T : class, new()
        {

            if (!_dbHelper.Any<T>(predicate))
            {
                _dbHelper.AddNewItem<T>(entity);
            }

            return _dbHelper.GetExistItem<T>(predicate);
        }

        public static void Remove<T>(T entity, Func<T, bool>? predicate = null)
            where T : class, new()
        {

            if (_dbHelper.Any<T>(predicate))
            {
                _dbHelper.DeleteItem<T>(entity);
            }
        }
    }
}
