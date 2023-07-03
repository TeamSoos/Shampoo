using DataLayer.Employee;
using ModelLayer;

namespace ServiceLayer.Employee; 

public class EmployeeService {
  private EmployeeSQL _sql;

  public EmployeeService() {
    _sql = new EmployeeSQL();
  }
  
  public ModelLayer.Employee GetOne(int employeeID) {
    // First check if the id exists in db
    if (!_sql.exists(employeeID)) {
      Console.WriteLine("User does not exist");
      return new ModelLayer.Employee();
    } // return empty object, signifies no such employee exists
    
    // Return employee
    return _sql.get_one(employeeID);
  }
  
  public List<ModelLayer.Employee> GetAll() {
    // Get all Employees
    return _sql.get_all();
  }
  
  public List<ModelLayer.Employee> GetAllByJob(EmployeeJob job) {
    // Get all Employees
    return  _sql.get_all_by_job(job);
  }

  public void Create(ModelLayer.Employee employee) {
    _sql.add_new(employee);
  }
  
  public void Delete(ModelLayer.Employee employee) {
    // Check if employee exists before deletion
    if (_sql.exists(employee.ID))
      _sql.delete(employee);
    else {
      throw new Exception("Tried to delete an employee that does not exist");
    }
  }
  
  public void Update(ModelLayer.Employee employee) {
    if (_sql.exists(employee.ID))
      _sql.update(employee);
    else {
      throw new Exception("Tried to update an employee that does not exist");
    }
  }

  public bool Authenticate(ModelLayer.Employee employee, string input_login) {
    // Here we will try to verify the hash
    
    // Guard to prevent new employees from being evaluated via hash
    // Verify if the employee exists to prevent from empty objects
    // requesting authentication
    if (!_sql.exists(employee.ID) || !employee.Login.hashed)
      return false;
    
    // Let our EmployeeLogin class handle validation
    return employee.Login.Validate(input_login);
  }
}