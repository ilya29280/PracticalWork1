using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using PW1.Model;

namespace PW1.DataAccessLayer
{
    public class DapperRepository<T> : IRepository<T> where T : IDomainObject
    {
        private readonly string _connectionString = @"Server=(localdb)\mssqllocaldb;Database=PW2_GnomeDB;Trusted_Connection=True;";
        private readonly string _tableName;

        public DapperRepository()
        {
            _tableName = typeof(T).Name + "s"; 
        }

        public void Add(T entity)
        {
            using var db = new SqlConnection(_connectionString);
            string sql = "INSERT INTO Gnomes (Name, Clan, Profession, Age, BeardLength) VALUES (@Name, @Clan, @Profession, @Age, @BeardLength)";
            db.Execute(sql, entity);
        }

        public void Update(T entity)
        {
            using var db = new SqlConnection(_connectionString);
            string sql = "UPDATE Gnomes SET Name=@Name, Clan=@Clan, Profession=@Profession, Age=@Age, BeardLength=@BeardLength WHERE Id=@Id";
            db.Execute(sql, entity);
        }

        public void Delete(int id)
        {
            using var db = new SqlConnection(_connectionString);
            string sql = $"DELETE FROM {_tableName} WHERE Id=@Id";
            db.Execute(sql, new { Id = id });
        }

        public List<T> ReadAll()
        {
            using var db = new SqlConnection(_connectionString);
            return db.Query<T>($"SELECT * FROM {_tableName}").ToList();
        }

        public T ReadById(int id)
        {
            using var db = new SqlConnection(_connectionString);
            return db.QueryFirstOrDefault<T>($"SELECT * FROM {_tableName} WHERE Id=@Id", new { Id = id });
        }
    }
}