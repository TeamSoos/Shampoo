using Npgsql;
using ModelLayer;
namespace DataLayer.Employee; 

public class EmployeeSQL : BaseSQL<ModelLayer.Employee> {
  
  public async Task<ModelLayer.Employee> get_one_async(int id) {
    var cmd = new NpgsqlCommand("SELECT id FROM employees where id=($1)"){
        Parameters = {
            new NpgsqlParameter { Value = id }
        }
    };
    return await QueryOne(cmd);
  }
  
  /// <summary>
  /// Function to retrieve a single Employee via their ID.
  /// Contains an async overload via <see cref="get_one_async"/>
  /// </summary>
  /// <param name="id">Employee ID from database</param>
  /// <returns>ModelLayer.Employee</returns>
  public ModelLayer.Employee get_one(int id) {
      // verify the existance of the employee in database
      var cmd = new NpgsqlCommand("SELECT id, name, job, login FROM employees where id=($1)"){
        Parameters = {
            new NpgsqlParameter { Value = id }
        }
    };
      
    return QueryOneSync(cmd);
  }

  public bool exists(int id) {
      var db = new Library.Database();
      
      // Query to check if the employee id exists in table
      // returns 1 if it does, 0 if not
      var cmd = new NpgsqlCommand("SELECT EXISTS(SELECT 1 FROM employees WHERE id = ($1));"){
              Parameters = {
                      new NpgsqlParameter { Value = id }
              }
      };
      
      // Set connection object for command
      cmd.Connection = db.Conn;
      
      // execute query manually
      bool valid = (bool)cmd.ExecuteScalar()!;
      db.Finalise();
      
      return valid;
  }
  
  public async Task<List<ModelLayer.Employee>> get_all() {
    var cmd = new NpgsqlCommand("SELECT * FROM employees");
    return await QueryMultipleAsync(cmd);
  }
  
  public async Task<List<ModelLayer.Employee>> get_all_by_job(EmployeeJob job) {
    var cmd = new NpgsqlCommand("SELECT id FROM employees where job=($1)"){
        Parameters = {
            new NpgsqlParameter { Value = job.ToString() }
        }
    };
    return await QueryMultipleAsync(cmd);
  }

  public void delete(ModelLayer.Employee employee) {
    NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM employees WHERE id = ($1)") {
        Parameters = {
            new NpgsqlParameter { Value = employee.ID }
        }
    };
    
    Store(cmd);
  }
  
  public void add_new(ModelLayer.Employee employee) {
    NpgsqlCommand cmd =
        new NpgsqlCommand(
            "INSERT INTO employees (name, job, login) VALUES ($1, $2, $3)") {
            Parameters = {
                new NpgsqlParameter { Value = employee.Name },
                new NpgsqlParameter { Value = employee.Job.ToString() },
                new NpgsqlParameter { Value = employee.Login.value },
            }
        };
    
    Store(cmd);
  }

  public void update(ModelLayer.Employee employee) {
      NpgsqlCommand cmd = new NpgsqlCommand("UPDATE employees SET name=($2),job=($3),login=($4) WHERE id = ($1)") {
              Parameters = {
                      new NpgsqlParameter { Value = employee.ID },
                      new NpgsqlParameter { Value = employee.Name },
                      new NpgsqlParameter { Value = employee.Job.ToString() },
                      new NpgsqlParameter { Value = employee.Login.value }
              }
      };
      
      Store(cmd);
  }

  protected override ModelLayer.Employee ReadTables(NpgsqlDataReader reader) {
    return new ModelLayer.Employee {
        ID = (int)reader["id"],
        Name = (string)reader["name"],
        Job = Enum.Parse<EmployeeJob>((string)reader["job"], ignoreCase: true),
        // If we are reading from the db, the login is always hashed
        Login = new EmployeeLogin(hash: (string)reader["login"])
    };
  }
}