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
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _db;

        public FileRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Domain.Entity.File entity)
        {
            Console.WriteLine("Rep");
            Console.WriteLine(entity.NameFile);
            Console.WriteLine(entity.IdFolder);
            await _db.File.AddAsync(entity); 
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Domain.Entity.File> GetById(int id) 
        {
            return await _db.File.Include(e => e.Folder).Include(e => e.TypeFile).FirstOrDefaultAsync(x => x.IdFile == id);
        }

        public async Task<List<Domain.Entity.File>> Select()
        {
            return await _db.File.Include(e => e.Folder).Include(e => e.TypeFile).ToListAsync();
        }

        public async Task<bool> Delete(Domain.Entity.File entity)
        {
            _db.File.Remove(entity); //удаление
            await _db.SaveChangesAsync(); //сохранение
            return true;
        }

        public async Task<Domain.Entity.File> Update(Domain.Entity.File entity)
        {
            _db.File.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Domain.Entity.File> GetByName(string name) //получение по имени
        {
            return await _db.File.Include(e => e.Folder).Include(e => e.TypeFile).FirstOrDefaultAsync(x => x.NameFile == name);
        }
    }
}
