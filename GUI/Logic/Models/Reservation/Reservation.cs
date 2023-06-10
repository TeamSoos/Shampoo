using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace GUI.Logic.Models.Reservation; 

public class Reservation {
  public string name;
  public string phone;
  public DateTime time;
  public int guests;
  public int table;

  public Reservation(string name, string phone, DateTime time, int guests, int table) {
    this.name = name;
    this.phone = phone;
    this.time = time;
    this.guests = guests;
    this.table = table;
  }

  public static void Create(string name, string phone, string time, int guests, int table) {
    ReservationSQL.save_reservation(
        name,
        phone,
        DateTime.ParseExact(time, "HH:mm:ss", CultureInfo.InvariantCulture),
        guests,
        table
    );
  }
  public static void Delete(int table) {
    ReservationSQL.delete_reservation(table);
  }

  public async static Task<List<Reservation>> getAll() {
    return await ReservationSQL.get_all();
  }

}