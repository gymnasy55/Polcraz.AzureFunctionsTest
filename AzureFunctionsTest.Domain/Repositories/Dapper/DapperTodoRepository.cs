using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AzureFunctionsTest.Domain.Models;
using AzureFunctionsTest.Domain.Repositories.Abstract;
using Dapper;

namespace AzureFunctionsTest.Domain.Repositories.Dapper
{
    public class DapperTodoRepository : ITodoRepository
    {
        private readonly string _connectionString;

        public DapperTodoRepository(string connectionString) => _connectionString = connectionString;

        public IEnumerable<Todo> GetAll()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.Query<Todo>("SELECT * FROM [dbo].[todos]");
        }

        public Todo GetByKey(Guid key)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var query = db.Query<Todo>("SELECT * FROM [dbo].[todos] WHERE [Id]=@Key", new {Key = key.ToString()});
            var result = query.ToList();
            return result.Any() ? result.First() : null;
        }

        public Todo Add(Todo entity)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var isExists = db.Query("SELECT [Id] FROM [dbo].[todos] WHERE [Id]=@Key", new {Key = entity.Id.ToString()}).Any();
            if (isExists)
                return null;
            db.Execute("INSERT INTO [dbo].[todos] ([Id], [TaskDescription], [IsCompleted], [CreatedTime])  VALUES (@Id, @TaskDescription, @IsCompleted, @CreatedTime)", entity);
            return entity;
        }

        public void Delete(Todo entity)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            db.Execute("DELETE FROM [dbo].[todos] WHERE Id = @Key", new {Key = entity.Id.ToString()});
        }
    }
}
