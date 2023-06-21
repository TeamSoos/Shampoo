using System.Collections.Generic;
using System.Threading.Tasks;
using GUI.Logic.Models.Employee;
using Logic.SQL;
using Npgsql;

namespace RoutedApp.Logic.Models.Employee;

public class EmployeeSQL {

    public static async Task<List<EmployeeType>> get_all() {
        Library.Database db = new Library.Database();

        NpgsqlCommand cmd = new NpgsqlCommand("SELECT id FROM employees", db.Conn);

        NpgsqlDataReader reader = await db.Query(cmd);
        List<EmployeeType> employees = new List<EmployeeType>();

        while (reader.Read()) {
            EmployeeType employee = new EmployeeType((int)reader["id"]);
            employees.Add(employee);
        }
        
        
        db.Finalize();

        return employees;
    }

    public static async Task<List<EmployeeType>> get_all_by_job(string job) {
        Library.Database db = new Library.Database();

        NpgsqlCommand cmd = new NpgsqlCommand("SELECT id FROM employees where job=($1)", db.Conn) {
            Parameters = {
                new NpgsqlParameter { Value = job }
            }
        };

        NpgsqlDataReader reader = await db.Query(cmd);
        List<EmployeeType> employees = new List<EmployeeType>();

        while (reader.Read()) {
            EmployeeType employee = new EmployeeType((int)reader["id"]);
            employees.Add(employee);
        }
        
        db.Finalize();

        return employees;
    }

    public static async Task<Dictionary<string, dynamic>> get_by_id(int id) {
        Library.Database db = new Library.Database();

        NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM employees WHERE id=($1)", db.Conn) {
            Parameters = {
                new NpgsqlParameter { Value = id }
            }
        };


        NpgsqlDataReader reader = await db.Query(cmd);
        await reader.ReadAsync();
        // Consolidata data, this way we can shut off the connection in the same scope
        // This prevents memory leaks of passing around a reader object
        Dictionary<string, dynamic> employee_data = new Dictionary<string, dynamic> {
            { "id", reader["id"] },
            { "name", reader["name"] },
            { "job", reader["job"] }
        };
        await reader.CloseAsync();
        
        
        db.Finalize();

        return employee_data;
    }
    public static void delete_by_id(int userId) {
        Library.Database db = new Library.Database();

        NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM employees WHERE id = ($1)", db.Conn) {
                Parameters = {
                        new NpgsqlParameter { Value = userId }
                }
        };

        db.Store(cmd);
    }
    public static void add_new(string name, string job, string login) {
        Library.Database db = new Library.Database();

        NpgsqlCommand cmd =
                new NpgsqlCommand(
                        "INSERT INTO employees (name, job, login) VALUES ($1, $2, $3)",
                        db.Conn) {
                        Parameters = {
                                new NpgsqlParameter { Value = name },
                                new NpgsqlParameter { Value = job },
                                new NpgsqlParameter { Value = login },
                        }
                };

        db.Store(cmd);
    }
    public static void update_by_id(int userId, string name, string job, string login, bool no_login = false) {
        Library.Database db = new Library.Database();
        NpgsqlCommand cmd;

        job = job.ToLower();
        
        if (no_login) {
            cmd = new NpgsqlCommand("UPDATE employees SET name=($2),job=($3) WHERE id = ($1)", db.Conn) {
                    Parameters = {
                            new NpgsqlParameter { Value = userId },
                            new NpgsqlParameter { Value = name },
                            new NpgsqlParameter { Value = job }
                    }
            };
        }
        else {
             cmd = new NpgsqlCommand("UPDATE employees SET name=($2),job=($3),login=($4) WHERE id = ($1)", db.Conn) {
                              Parameters = {
                                      new NpgsqlParameter { Value = userId },
                                      new NpgsqlParameter { Value = name },
                                      new NpgsqlParameter { Value = job },
                                      new NpgsqlParameter { Value = login }
                              }
                      };
            
        }
        

        db.Store(cmd);
    }
}