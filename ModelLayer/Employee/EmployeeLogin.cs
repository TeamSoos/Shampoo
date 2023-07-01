namespace ModelLayer; 

public class EmployeeLogin {
  private string _raw;
  private string _hash;
  public string value {
    get { return _raw != "" ? _raw : _hash; }
  }
  public bool hashed {
    get { return _hash != "" ? true : false; }
  }

  public void Hash() {
    if (this._raw == "") {
      throw new Exception("Unable to hash the employee login as the raw value is set.");
    }

    this._hash = BCrypt.Net.BCrypt.HashPassword(this._raw);
  }
  
  public bool Validate(string input_login) {
    if (!this.hashed) {
      throw new Exception("Unable to verify an empty hash.");
    }

    return BCrypt.Net.BCrypt.Verify(input_login, this._hash);
  }

  public EmployeeLogin(string raw = "", string hash = "") {
    _raw = raw;
    _hash = hash;
  }
}