using Explorer.DAL.Interfaces;
using Explorer.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.DAL.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly ApplicationDbContext _db;

        public FolderRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Folder entity)
        {
            await _db.Folder.AddAsync(entity); 
            await _db.SaveChangesAsync(); 
            return true;
        }

        public async Task<Folder> GetById(int id) 
        {
            return await _db.Folder.Include(e => e.Files).ThenInclude(f => f.TypeFile).FirstOrDefaultAsync(x => x.IdFolder == id);
        }

        public Task<List<Folder>> GetByParent(int id)
        {
            return (Task<List<Folder>>)_db.Folder.Include(e => e.Files).ThenInclude(f => f.TypeFile).Where(x => x.IdParentFolder == id);
        }

        public async Task<List<Folder>> Select()
        {
            return await _db.Folder.Include(e => e.Files).ThenInclude(f => f.TypeFile).ToListAsync();
        }

        public async Task<bool> Delete(Folder entity)
        {
            _db.Folder.Remove(entity);
            await _db.SaveChangesAsync(); 
            return true;
        }

        public async Task<Folder> Update(Folder entity)
        {
            _db.Folder.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Folder> GetByName(string name) 
        {
            return await _db.Folder.Include(e => e.Files).ThenInclude(f => f.TypeFile).FirstOrDefaultAsync(x => x.NameFolder == name);
        }
    }
}
