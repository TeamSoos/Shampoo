namespace ModelLayer; 

public class EmployeeLogin {
  private string _raw;
  private string _hash;
  public string value {
    get { return _raw != "" ? _raw : _hash; }
  }

  public void Hash() {
    if (this._raw == "") {
      throw new Exception("Unable to hash the employee login as the raw value is set.");
    }

    this._hash = BCrypt.Net.BCrypt.HashPassword(this._raw);
  }

  public EmployeeLogin(string raw = "", string hash = "") {
    _raw = raw;
    _hash = hash;
  }
}