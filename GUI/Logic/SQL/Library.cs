using System;
using System.Data;
using System.Threading.Tasks;
using DotNetEnv;
using Npgsql;

namespace Logic.SQL;

public class Library {
        /// <summary>
        ///     Database class. Facilitates the connection to Postgres.
        ///     Provides both execution of queries and notification listeners
        /// </summary>
    public class Database {

        public static NpgsqlConnection conn;
        public static NpgsqlConnection lockedconn;
        public NpgsqlConnection Conn;

        readonly string _ConnectionString;

        public Database(bool holdlock = false) {
            Env.Load();
            Env.TraversePath().Load();

            string connstr = Env.GetString("SQL_CONN_STR");
            _ConnectionString = connstr;
            
            Conn = new NpgsqlConnection(connstr);
            Conn.Open();
        }

        public async void Test() {
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM items WHERE id=($1)", Conn) {
                Parameters = {
                    new NpgsqlParameter { Value = 1 }
                }
            };
            NpgsqlDataReader reader = await Query(cmd);
            await reader.ReadAsync();
            Console.WriteLine(reader["id"]);
            Console.WriteLine(reader["name"]);
            Console.WriteLine(reader["price"]);
            Console.WriteLine(reader["description"]);
            await reader.CloseAsync();
        }

        async Task<NpgsqlDataSource> BuildConnection() {
            await using NpgsqlDataSource dataSource = NpgsqlDataSource.Create(_ConnectionString);
            return dataSource;
        }

        public async Task<NpgsqlDataReader> Query(NpgsqlCommand cmd) {
            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            return reader;
        }

        public void Finalize() {
            Conn.Close();
        }

        public async void Store(NpgsqlCommand cmd) {
            await cmd.ExecuteNonQueryAsync();
            Finalize();
        }

        public void ListenToNotif(string connstr) {
            string notificationName = "testchannel";
            NpgsqlConnection notificationConnection = new NpgsqlConnection(connstr);
            notificationConnection.Open();

            try {

                if (notificationConnection.State != ConnectionState.Open) {
                    Console.WriteLine("Connection to database failed");
                    return;
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand($"LISTEN {notificationName}",
                           notificationConnection)) {
                    cmd.ExecuteNonQuery();
                }

                notificationConnection.Notification += PostgresNotification;
                notificationConnection.WaitAsync();
            }
            catch (Exception ex) {
                Console.WriteLine($"Exception thrown with message : {ex.Message}");
                return;
            }

            //  wait forever, press enter key to exit program  
            Console.ReadLine();

            // stop the db notifcation
            notificationConnection.Notification -= PostgresNotification;

            using (NpgsqlCommand command = new NpgsqlCommand($"unlisten {notificationName}",
                       notificationConnection)) {
                command.ExecuteNonQuery();
            }

            notificationConnection.Close();
        }

        static void PostgresNotification(object sender, NpgsqlNotificationEventArgs e) {
            Console.WriteLine("New Order notification received " + e.Payload);
        }
    }
}