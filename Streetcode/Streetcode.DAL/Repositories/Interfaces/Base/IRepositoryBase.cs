
namespace StreetCode.DAL.Repositories.Interfaces.Base
{
    public interface IRepositoryBase
    {

        public void Create();

        public void CreateAsync();

        public void GetById();

        public void GetAll();

        public void GetAllAsync();

        public void Update();

        public void Delete();

    }
}