using DataLayer.Reservations;
using ModelLayer.Tables;

namespace ServiceLayer.Reservation; 

public class ReservationService {
  private ReservationSQL _sql;

  public ReservationService() {
    _sql = new ReservationSQL();
  }

  public List<ModelLayer.Tables.Reservation> getAll() {
    return _sql.get_all();
  }

  public void Reserve(ModelLayer.Tables.Reservation reservation) {
    _sql.save_reservation(reservation);
  } 
  
  public void Delete(ModelLayer.Tables.Reservation reservation) {
    _sql.delete_reservation(reservation);
  } 
}