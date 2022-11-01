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
    public class TypeFileRepository : ITypeFileRepository
    {
        private readonly ApplicationDbContext _db;

        public TypeFileRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(TypeFile entity)
        {
            await _db.TypeFile.AddAsync(entity); //добавление
            await _db.SaveChangesAsync(); //сохранение
            return true;
        }

        public async Task<TypeFile> GetById(int id) //получение по Id
        {
            return await _db.TypeFile.FirstOrDefaultAsync(x => x.IdType == id);
        }

        public async Task<List<TypeFile>> Select()
        {
            var res =  await _db.TypeFile.ToListAsync();
            Console.WriteLine(res);
            return res;

        }


        public async Task<bool> Delete(TypeFile entity)
        {
            _db.TypeFile.Remove(entity); //удаление
            await _db.SaveChangesAsync(); //сохранение
            return true;
        }

        public async Task<TypeFile> Update(TypeFile entity)
        {
            _db.TypeFile.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<TypeFile> GetByName(string name) //получение по имени
        {
            return await _db.TypeFile.FirstOrDefaultAsync(x => x.NameType == name);
        }
    }
}
