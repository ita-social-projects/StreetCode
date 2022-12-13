
namespace StreetCode.DAL.Repositories.Interfaces.Base;

public interface IRepositoryBase<T>
{

     void Create();

     Task CreateAsync();

     void GetById();

     IQueryable<T> GetAll();

     Task<IEnumerable<T>> GetAllAsync();

     void Update();

     void Delete();

}