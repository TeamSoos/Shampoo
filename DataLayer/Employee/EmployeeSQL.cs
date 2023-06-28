using Npgsql;
using ModelLayer;
namespace DataLayer.Employee; 

public class EmployeeSQL : BaseSQL<ModelLayer.Employee> {
  
  public async Task<ModelLayer.Employee> get_one(int id) {
    var cmd = new NpgsqlCommand("SELECT id FROM employees where id=($1)"){
        Parameters = {
            new NpgsqlParameter { Value = id }
        }
    };
    return await QueryOne(cmd);
  }
  
  public async Task<List<ModelLayer.Employee>> get_all() {
    var cmd = new NpgsqlCommand("SELECT * FROM employees");
    return await QueryMultiple(cmd);
  }
  
  public async Task<List<ModelLayer.Employee>> get_all_by_job(EmployeeJob job) {
    var cmd = new NpgsqlCommand("SELECT id FROM employees where job=($1)"){
        Parameters = {
            new NpgsqlParameter { Value = job.ToString() }
        }
    };
    return await QueryMultiple(cmd);
  }

  public void delete(int id) {
    NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM employees WHERE id = ($1)") {
        Parameters = {
            new NpgsqlParameter { Value = id }
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