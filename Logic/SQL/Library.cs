using Npgsql;

namespace Logic.SQL; 

public class Library {
  public class Database
  {
    /// <summary>
    /// Database class. Facilitates the connection to Postgres.
    /// Provides both execution of queries and notification listeners
    /// </summary>
    private String _ConnectionString;

    private readonly NpgsqlConnection _Conn;

    public Database()
    {
      DotNetEnv.Env.Load();
      DotNetEnv.Env.TraversePath().Load();
      
      string connstr = DotNetEnv.Env.GetString("SQL_CONN_STR");
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
    
    public void ListenToNotif(string connstr)
    {
      String notificationName = "testchannel";
      NpgsqlConnection notificationConnection = new NpgsqlConnection(connstr);
      notificationConnection.Open();
      try
      {
               
        if (notificationConnection.State != System.Data.ConnectionState.Open)
        {
          Console.WriteLine("Connection to database failed");
          return;
        }

        using (NpgsqlCommand cmd = new NpgsqlCommand($"LISTEN {notificationName}", 
                   notificationConnection))
        {
          cmd.ExecuteNonQuery();
        }

        notificationConnection.Notification += PostgresNotification;
        notificationConnection.WaitAsync();
      }   
      catch(Exception ex)
      {
        Console.WriteLine($"Exception thrown with message : {ex.Message}");
        return;
      }

      //  wait forever, press enter key to exit program  
      Console.ReadLine();

      // stop the db notifcation
      notificationConnection.Notification -= PostgresNotification;
      using (var command = new NpgsqlCommand($"unlisten {notificationName}", 
                 notificationConnection))
      {
        command.ExecuteNonQuery();
      }
      notificationConnection.Close();
    }
    static void PostgresNotification(object sender, NpgsqlNotificationEventArgs e)
    {
      Console.WriteLine("New Order notification received "+ e.Payload);
    }
  }
}