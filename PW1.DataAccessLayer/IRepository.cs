using System.Collections.Generic;
using PW1.Model;

namespace PW1.DataAccessLayer
{
    public interface IRepository<T> where T : IDomainObject
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        List<T> ReadAll();
        T ReadById(int id);
    }
}