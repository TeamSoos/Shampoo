namespace GUI.ViewModels;

public abstract class RouterPage : ViewModelBase {
  public abstract bool CanNavigate();
}