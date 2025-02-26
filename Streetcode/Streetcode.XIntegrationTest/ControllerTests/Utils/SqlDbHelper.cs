using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Streetcode.DAL.Persistence;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    public class SqlDbHelper
    {
        private readonly StreetcodeDbContext _dbContext;
        private readonly object _lock = new object();
        private static readonly ConcurrentDictionary<Type, object> _entityLocks = new ConcurrentDictionary<Type, object>();

        public SqlDbHelper(DbContextOptions<StreetcodeDbContext> options)
        {
            _dbContext = new StreetcodeDbContext(options);
        }

        public string GetIdentityInsertString<T>(bool enable)
        {
            var value = enable ? "ON" : "OFF";
            return $"SET IDENTITY_INSERT {GetEntityTableName<T>()} {value};";
        }

        public bool ItemWithIdExist<T>(int id)
            where T : class, new()
        {
            var idProp = typeof(T).GetProperty("Id");
            if (_dbContext.Set<T>()
                .AsEnumerable()
                .FirstOrDefault(predicate: s => (int)idProp?.GetValue(s) ! == id) == null)
            {
                return false;
            }

            return true;
        }

        public T? GetExistItemId<T>(int id)
            where T : class, new()
        {
            var idProp = typeof(T).GetProperty("Id");
            return _dbContext.Set<T>()
                .AsEnumerable()
                .FirstOrDefault(s => ((int)idProp?.GetValue(s) !) == id);
        }

        public T AddNewItem<T>(T newItem)
            where T : class, new()
        {
            var idProp = typeof(T).GetProperty("Id");
            if (idProp != null)
            {
                string? value = idProp.GetValue(newItem) as string;
                if (!string.IsNullOrEmpty(value) && int.TryParse(value, out _))
                {
                    idProp.SetValue(newItem, 0);
                }
            }

            lock (_entityLocks.GetOrAdd(typeof(T), new object()))
            {
                return _dbContext.Set<T>().Add(newItem).Entity;
            }
        }

        public void AddItemWithCustomId<T>(T newItem)
            where T : class, new()
        {
            var entityLock = _entityLocks.GetOrAdd(typeof(T), new object());

            lock (entityLock)
            {
                try
                {
                    _dbContext.Database.OpenConnection();

                    string tableSchema = _dbContext.Model.FindEntityType(typeof(T))?.GetSchema() !;
                    string tableName = _dbContext.Model.FindEntityType(typeof(T))?.GetTableName() !;

                    string identityOnCommand = $"SET IDENTITY_INSERT {tableSchema}.{tableName} ON";
                    string identityOffCommand = $"SET IDENTITY_INSERT {tableSchema}.{tableName} OFF";

                    if (TableHasIdentityColumn(typeof(T)))
                    {
                        _dbContext.Database.ExecuteSqlRaw(identityOnCommand);
                    }

                    var trackedEntity = _dbContext.ChangeTracker.Entries<T>().FirstOrDefault(e => e.Entity == newItem);
                    if (trackedEntity != null)
                    {
                        trackedEntity.State = EntityState.Detached;
                    }

                    _dbContext.Add(newItem);
                    _dbContext.SaveChanges();

                    if (TableHasIdentityColumn(typeof(T)))
                    {
                        _dbContext.Database.ExecuteSqlRaw(identityOnCommand);
                    }
                }
                finally
                {
                    _dbContext.Database.CloseConnection();
                }
            }
        }

        public T? GetExistItem<T>(Func<T, bool>? predicate = default)
            where T : class, new()
        {
            if (predicate != null)
            {
                return _dbContext.Set<T>().AsEnumerable().FirstOrDefault(predicate);
            }

            return _dbContext.Set<T>().FirstOrDefault();
        }

        public bool Any<T>(Func<T, bool>? predicate = default)
           where T : class, new()
        {
            if (predicate != null)
            {
                return _dbContext.Set<T>().AsNoTracking().Any(predicate);
            }

            return _dbContext.Set<T>().AsNoTracking().Any();
        }

        public IEnumerable<T> GetAll<T>(Func<T, bool>? predicate = default)
             where T : class
        {
            if (predicate != null)
            {
                return _dbContext.Set<T>().AsNoTracking().AsEnumerable().Where(predicate);
            }

            return _dbContext.Set<T>().AsNoTracking();
        }

        public T DeleteItem<T>(T item)
            where T : class, new()
        {
            lock (_entityLocks.GetOrAdd(typeof(T), new object()))
            {
                return _dbContext.Set<T>().Remove(item).Entity;
            }
        }

        public void SaveChanges()
        {
            lock (_lock)
            {
                _dbContext.SaveChanges();
            }
        }

        private bool TableHasIdentityColumn(Type entityType)
        {
            var entityTypeMeta = _dbContext.Model.FindEntityType(entityType);
            if (entityTypeMeta == null)
            {
                return false;
            }

            return entityTypeMeta.GetProperties().Any(p => p.ValueGenerated == ValueGenerated.OnAdd);
        }

        private string GetEntityTableName<T>()
        {
            var entityType = _dbContext.Model.FindEntityType(typeof(T));
            return $"{entityType?.GetSchema()}.{entityType?.GetTableName()}";
        }
    }
}
