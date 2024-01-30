using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Persistence;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    public class SqlDbHelper
    {
        private StreetcodeDbContext dbContext;

        public SqlDbHelper(DbContextOptions<StreetcodeDbContext> options)
        {
            this.dbContext = new StreetcodeDbContext(options);
        }

        public string GetEntityTableName<T>()
        {
            var entityType = this.dbContext.Model.FindEntityType(typeof(T));
            return $"{entityType?.GetSchema()}.{entityType?.GetTableName()}";
        }

        public string GetIdentityInsertString<T>(bool enable)
        {
            var value = enable ? "ON" : "OFF";
            return $"SET IDENTITY_INSERT {this.GetEntityTableName<T>()} {value};";
        }

        public bool ItemWithIdExist<T>(int id)
            where T : class, new()
        {
            var idProp = typeof(T).GetProperty("Id");
            if (this.dbContext.Set<T>().AsEnumerable().FirstOrDefault(predicate: s => (int)idProp?.GetValue(s)! == id) == null)
            {
                return false;
            }

            return true;
        }

        public T GetExistItemId<T>(int id)
            where T : class, new()
        {
            var idProp = typeof(T).GetProperty("Id");
            return this.dbContext.Set<T>().AsEnumerable().FirstOrDefault(s => ((int)idProp?.GetValue(s)!) == id);
        }

        public T AddNewItem<T>(T newItem)
            where T : class, new()
        {
            var idProp = typeof(T).GetProperty("Id");
            object value = idProp?.GetValue(newItem);
            if (value != null && (int)value != 0)
            {
                idProp?.SetValue(newItem, 0);
            }
            return this.dbContext.Set<T>().Add(newItem).Entity;
        }

        public T GetExistItem<T>(Func<T, bool>? predicate = default)
            where T : class, new()
        {
            if (predicate != null)
            {
                return this.dbContext.Set<T>().AsEnumerable().FirstOrDefault(predicate);
            }

            return this.dbContext.Set<T>().FirstOrDefault();
        }

        public bool Any<T>(Func<T, bool>? predicate = default)
           where T : class, new()
        {
            if (predicate != null)
            {
                return this.dbContext.Set<T>().AsNoTracking().Any(predicate);
            }

            return this.dbContext.Set<T>().AsNoTracking().Any();
        }

        public IEnumerable<T> GetAll<T>(Func<T, bool>? predicate = default)
             where T : class
        {
            if (predicate != null)
            {
                return this.dbContext.Set<T>().AsEnumerable().Where(predicate);
            }

            return this.dbContext.Set<T>();
        }

        public T DeleteItem<T>(T item)
            where T : class, new()
        {
            return this.dbContext.Set<T>().Remove(item).Entity;
        }

        public void SaveChanges() => this.dbContext.SaveChanges();
    }
}
