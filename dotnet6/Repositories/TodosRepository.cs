using Dapper;
using Npgsql;
using dotnet6.Models;

namespace dotnet6.Repositories;

public class TodosRepository
{
    private String connectionString = "";

    public TodosRepository() {
        var connStringBuilder = new NpgsqlConnectionStringBuilder();
        connStringBuilder.Host = Environment.GetEnvironmentVariable("DATABASE_HOST");
        connStringBuilder.Port = Int16.Parse(Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "00000");
        connStringBuilder.Username = Environment.GetEnvironmentVariable("DATABASE_USERNAME");
        connStringBuilder.Password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
        connStringBuilder.Database = Environment.GetEnvironmentVariable("DATABASE_NAME");
        connStringBuilder.ApplicationName = Environment.GetEnvironmentVariable("DATABASE_APPLICATION_NAME");
        connStringBuilder.SslMode = SslMode.VerifyFull;
        this.connectionString = connStringBuilder.ConnectionString;
    }

    public IEnumerable<TodoModel> fetchAll() 
    {
        using (var connection = new NpgsqlConnection(this.connectionString))
        {
            var sql = "SELECT id AS \"Id\", content AS \"Content\", \"isCompleted\" AS \"IsCompleted\" FROM todos_dev";
            return connection.Query<TodoModel>(sql);
        }
    }

    public TodoModel findOne(Guid id) 
    {
        using (var connection = new NpgsqlConnection(this.connectionString))
        {
            var sql = "SELECT id AS \"Id\", content AS \"Content\", \"isCompleted\" AS \"IsCompleted\" FROM todos_dev WHERE id = @id";
            return connection.QueryFirstOrDefault<TodoModel>(sql, new { id });
        }
    }

    public TodoModel create(String content)
    {
        using (var connection = new NpgsqlConnection(this.connectionString))
        {
            var sql = "INSERT INTO todos_dev(content, \"isCompleted\") VALUES (@content, FALSE) RETURNING *;";
            return connection.QuerySingle<TodoModel>(sql, new { content });
        }
    }

    public TodoModel delete(Guid id)
    {
        using (var connection = new NpgsqlConnection(this.connectionString))
        {
            var sql = "DELETE FROM todos_dev WHERE id = @id RETURNING *";
            return connection.QuerySingleOrDefault<TodoModel>(sql, new { id });
        }
    }

    public TodoModel update(Guid id, String content)
    {
        using (var connection = new NpgsqlConnection(this.connectionString))
        {
            var sql = "UPDATE todos_dev SET content = @content WHERE id = @id RETURNING *";
            return connection.QuerySingleOrDefault<TodoModel>(sql, new { id, content });
        }
    }
}
