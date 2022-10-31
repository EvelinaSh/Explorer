using Explorer.Domain.Entity;

namespace Explorer.DAL.Interfaces
{
        public interface IFileRepository : IBaseRepository<Domain.Entity.File>
        {
            Task<Domain.Entity.File> GetByName(string name);
        }
}


