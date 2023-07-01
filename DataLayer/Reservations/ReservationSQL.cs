using DataLayer.Tables;
using ModelLayer.Tables;
using Npgsql;

namespace DataLayer.Reservations; 

public class ReservationSQL : BaseSQL<Reservation> {
  
  public List<Reservation> get_all() {
    var cmd = new NpgsqlCommand("SELECT * FROM table_reservations ORDER BY id ASC");
    return QueryMultiple(cmd);
  }
  
  public void save_reservation(Reservation reservation) {
    NpgsqlCommand cmd =
        new NpgsqlCommand(
            "INSERT INTO table_reservations (name, phone, time, guests, table_id) VALUES ($1, $2, $3, $4, $5)") {
            Parameters = {
                new NpgsqlParameter { Value = reservation.Name },
                new NpgsqlParameter { Value = reservation.Phone },
                new NpgsqlParameter { Value = reservation.Time },
                new NpgsqlParameter { Value = reservation.Guests },
                new NpgsqlParameter { Value = reservation.Table.ID }
            }
        };
    
    this.Store(cmd);
  }

  public void delete_reservation(Reservation reservation) {
    NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM table_reservations WHERE id = ($1)") {
        Parameters = {
            new NpgsqlParameter { Value = reservation.ID }
        }
    };
    
    this.Store(cmd);
  }

  protected override Reservation ReadTables(NpgsqlDataReader reader) {
    TablesSQL tableDB = new TablesSQL();
    return new Reservation {
        ID = (int)reader["id"],
        Guests = (int)reader["guests"],
        Name = (string)reader["name"],
        Phone = (string)reader["phone"],
        Table = tableDB.get_one((int)reader["table_id"]),
        Time = reader.GetDateTime(reader.GetOrdinal("time"))
    };
  }
}