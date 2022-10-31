using Explorer.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {

        }

        public DbSet<Domain.Entity.File> File { get; set; }
        public DbSet<Folder> Folder { get; set; }
        public DbSet<TypeFile> TypeFile { get; set; }
    }
}
