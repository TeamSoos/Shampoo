using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.Models.Base;
using Logic.SQL;
using Npgsql;
using RoutedApp.Logic.Models.Employee;

namespace GUI.Logic.Models.Employee;

public enum Jobs {
    admin,
    waiter,
    chef,
    bartender
}

public class EmployeeType {
    public int id;

    public string job;
    public string name;

    public EmployeeType(int ID) {
        Task.Run(async () => {
            Dictionary<string, dynamic> table_data = await EmployeeSQL.get_by_id(ID);
            name = table_data["name"];
            id = ID;
            // error handling goes to shit but oh well
            job = Enum.Parse<Jobs>(table_data["job"], ignoreCase: true).ToString();
        }).Wait();
    }
    private EmployeeType() {
    }


    private static EmployeeType raw(int id, string job, string name, string login) {
        return new EmployeeType {
                id = id,
                job = job,
                name = name,
                login = login
        };
    }
    public string login { get; set; }

    public static async Task<List<EmployeeType>> getAll() {
        return await EmployeeSQL.get_all();
    }

    public static async Task<List<EmployeeType>> getAll(string job) {
        return await EmployeeSQL.get_all_by_job(job);
    }
    public static void Delete(int userId) {
        EmployeeSQL.delete_by_id(userId);
    }
    public static void Create(string name, string job, string login) {
        EmployeeSQL.add_new(name, job, BCrypt.Net.BCrypt.HashPassword(login));
    }
    public void Update(string Login, string? Name = null, string? Job = null) {
        EmployeeSQL.update_by_id(
                this.id,
                Name ?? this.name,
                Job ?? this.job,
                BCrypt.Net.BCrypt.HashPassword(Login!),
                no_login: Login == "[hashed]"
        );
    }
    public async static Task<EmployeeType> Authenticate(string login, int id) {
        Library.Database db = new Library.Database();

        
        var cmd = new NpgsqlCommand("SELECT job, name, login FROM employees WHERE id=($1)", db.Conn) {
                Parameters = {
                        new NpgsqlParameter { Value = id }
                }
        };

        // This is redundant but kept for clarity sake
        var reader = await db.Query(cmd);
        string hash ="";
        string job ="";
        string name ="";
        

        while (await reader.ReadAsync()) {
            hash = reader["login"].ToString()!;
            job = reader["job"].ToString()!;
            name = reader["name"].ToString()!;
        }

        return EmployeeType.raw(id, job, name, hash);
    }
}