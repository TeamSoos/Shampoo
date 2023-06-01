using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Logic.SQL;
using Npgsql;
using ReactiveUI;

namespace GUI.ViewModels;

public class LoginPageViewModel : RouterPage {
  private string _text;

  public string Text
  {
    get => _text;
    set => _text = value;
  }

  private List<string> staff = new List<string>() { "chef", "barman", "waiter"};
  private bool _visible;

  public ICommand LoginStaff { get; }

  public bool ErrorIsVisible {
    get => _visible;
    set => this.RaiseAndSetIfChanged(ref _visible, value);
  }

  public LoginPageViewModel() {
    LoginStaff = ReactiveCommand.Create(loginStaff);
  }
  
  // Styling
  public int FormContentWidth => 350;
  public int FormContentSpacing => 10;
  // END STYLING
  public override bool CanNavigate() {
    return true;
  }

  private async Task loginStaff() {
    ErrorIsVisible = false;
    Console.WriteLine($"Login key: {Text}");
    Library.Database db = new Library.Database();
    
    var cmd = new NpgsqlCommand("SELECT id, job FROM employees WHERE login=($1)", db.Conn)
    {
        Parameters =
        {
            new() { Value = Text }
        }
    };
    var reader = await db.Query(cmd);


    while (await reader.ReadAsync()) {
      if (reader["id"] != null) // Ignore the warning, stupid C#
      {
        // We have an ID, let's check if it's one of the staff
        if (staff.Contains(reader["job"].ToString()!)) {
          Console.WriteLine("Staff access!");
        }
        else {
          Console.WriteLine("Not staff!");
        }
      }
    }

    ErrorIsVisible = true;
  }
}