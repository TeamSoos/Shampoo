using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.SQL;
using Npgsql;

namespace GUI.Logic.Models.Reservation;

public class ReservationSQL {
    public static void save_reservation(string name, string phone, DateTime time, int guests, int table_id) {
        Library.Database db = new Library.Database();

        NpgsqlCommand cmd =
            new NpgsqlCommand(
                "INSERT INTO table_reservations (name, phone, time, guests, table_id) VALUES ($1, $2, $3, $4, $5)",
                db.Conn) {
                Parameters = {
                    new NpgsqlParameter { Value = name },
                    new NpgsqlParameter { Value = phone },
                    new NpgsqlParameter { Value = time },
                    new NpgsqlParameter { Value = guests },
                    new NpgsqlParameter { Value = table_id }
                }
            };

        db.Store(cmd);
    }

    public static async Task<List<Reservation>> get_all() {
        Library.Database db = new Library.Database();

        NpgsqlCommand cmd =
            new NpgsqlCommand("SELECT name, phone, time, guests, table_id FROM table_reservations", db.Conn);

        NpgsqlDataReader reader = await db.Query(cmd);
        List<Reservation> reservations = new List<Reservation>();

        while (reader.Read()) {
            string name = reader.GetString(reader.GetOrdinal("name"));
            string phone = reader.GetString(reader.GetOrdinal("phone"));
            DateTime time = reader.GetDateTime(reader.GetOrdinal("time"));
            int guests = reader.GetInt32(reader.GetOrdinal("guests"));
            int table = reader.GetInt32(reader.GetOrdinal("table_id"));
            reservations.Add(
                new Reservation(name, phone, time, guests, table)
            );
        }

        return reservations;
    }

    public static void delete_reservation(int table) {
        Library.Database db = new Library.Database();

        NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM table_reservations WHERE table_id = ($1)", db.Conn) {
            Parameters = {
                new NpgsqlParameter { Value = table }
            }
        };

        db.Store(cmd);
    }
}