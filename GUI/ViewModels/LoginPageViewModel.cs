using System.Windows.Input;
using ReactiveUI;

namespace GUI.ViewModels;

public class LoginPageViewModel : RouterPage {
  
  // Styling
  public int FormContentWidth => 350;
  public int FormContentSpacing => 10;
  // END STYLING
  public override bool CanNavigate() {
    return true;
  }
}