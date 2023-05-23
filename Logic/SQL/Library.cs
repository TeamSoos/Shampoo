using Npgsql;

namespace Logic.SQL; 

public class Library {
  class Database
  {
    /// <summary>
    /// Database class. Facilitates the connection to Postgres.
    /// Provides both execution of queries and notification listeners
    /// </summary>
    private String _ConnectionString;

    private readonly NpgsqlConnection _Conn;

    public Database(string connstr)
    {
      this._ConnectionString = connstr;
      this._Conn = new NpgsqlConnection(connstr);
      this._Conn.Open();
    }

    public async void Test()
    {
      var cmd = new NpgsqlCommand("SELECT * FROM items WHERE id=($1)", _Conn)
      {
          Parameters =
          {
              new() { Value = 1 }
          }
      };
      var reader = await Query(cmd);
      await reader.ReadAsync();
      Console.WriteLine(reader["id"]);
      Console.WriteLine(reader["name"]);
      Console.WriteLine(reader["price"]);
      Console.WriteLine(reader["description"]);
      await reader.CloseAsync();
    }
        
    async Task<NpgsqlDataSource> BuildConnection()
    {
      await using var dataSource = NpgsqlDataSource.Create(_ConnectionString);
      return dataSource;
    }

    public async Task<NpgsqlDataReader> Query(NpgsqlCommand cmd)
    {
      var reader = await cmd.ExecuteReaderAsync();
      return reader;
    }
  }
}