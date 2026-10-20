using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PW1.Model;

namespace PW1.DataAccessLayer
{
    
    public class GnomeDbContext : DbContext
    {
        public DbSet<Gnome> Gnomes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PW2_GnomeDB;Trusted_Connection=True;");
        }
    }

    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly GnomeDbContext _context;
        private readonly DbSet<T> _dbSet;

        public EntityRepository()
        {
            _context = new GnomeDbContext();
            _context.Database.EnsureCreated();
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity) { _dbSet.Add(entity); _context.SaveChanges(); }
        public void Update(T entity) { _dbSet.Update(entity); _context.SaveChanges(); }

        public void Delete(int id)
        {
            var e = ReadById(id);
            if (e != null) { _dbSet.Remove(e); _context.SaveChanges(); }
        }

        public List<T> ReadAll() { return _dbSet.ToList(); }
        public T ReadById(int id) { return _dbSet.Find(id); }
    }
}