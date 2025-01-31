using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Persistence;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    public class SqlDbHelper
    {
        private readonly StreetcodeDbContext dbContext;
        private readonly object _lock = new object();
        private static readonly ConcurrentDictionary<Type, object> _entityLocks = new ConcurrentDictionary<Type, object>();

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
            if (this.dbContext.Set<T>()
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
            return this.dbContext.Set<T>()
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
                return this.dbContext.Set<T>().Add(newItem).Entity;
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
                    this.dbContext.Database.OpenConnection();

                    string tableSchema = this.dbContext.Model.FindEntityType(typeof(T))?.GetSchema()!;
                    string tableName = this.dbContext.Model.FindEntityType(typeof(T))?.GetTableName()!;

                    string identityOnCommand = $"SET IDENTITY_INSERT {tableSchema}.{tableName} ON";
                    string identityOffCommand = $"SET IDENTITY_INSERT {tableSchema}.{tableName} OFF";

                    if (TableHasIdentityColumn(typeof(T)))
                    {
                        this.dbContext.Database.ExecuteSqlRaw(identityOnCommand);
                    }
                    
                    var trackedEntity = this.dbContext.ChangeTracker.Entries<T>().FirstOrDefault(e => e.Entity == newItem);
                    if (trackedEntity != null)
                    {
                        trackedEntity.State = EntityState.Detached;
                    }

                    this.dbContext.Add(newItem);
                    this.dbContext.SaveChanges();

                    if (TableHasIdentityColumn(typeof(T)))
                    {
                        this.dbContext.Database.ExecuteSqlRaw(identityOnCommand);
                    }
                }
                finally
                {
                    this.dbContext.Database.CloseConnection();
                }
            }
        }

        private bool TableHasIdentityColumn(Type entityType)
        {
            var entityTypeMeta = this.dbContext.Model.FindEntityType(entityType);
            if (entityTypeMeta == null) return false;

            return entityTypeMeta.GetProperties().Any(p => p.ValueGenerated == ValueGenerated.OnAdd);
        }


        public T? GetExistItem<T>(Func<T, bool>? predicate = default)
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
                return this.dbContext.Set<T>().AsNoTracking().AsEnumerable().Where(predicate);
            }

            return this.dbContext.Set<T>().AsNoTracking();
        }

        public T DeleteItem<T>(T item)
            where T : class, new()
        {
            lock (_entityLocks.GetOrAdd(typeof(T), new object()))
            {
                return this.dbContext.Set<T>().Remove(item).Entity;
            }
        }

        public void SaveChanges()
        {
            lock (_lock)
            {
                this.dbContext.SaveChanges();
            }
        }
    }
}
