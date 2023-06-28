using System.Reactive;
using ReactiveUI;

namespace GUI.ViewModels;

public class AddItemViewModel : RoutablePage
{

    public int MenuTypeIndex { get; set; }
    public int MenuTypeString { get; set; }

    public string ItemName { get; set; } = "";
    public string ItemCat { get; set; } = "";
    
    public ReactiveCommand<Unit, Unit> GoBack { get; }
    public ReactiveCommand<Unit, Unit> AddItem { get; }

    public AddItemViewModel(IHostScreen hostScreen)
    {
        GoBack = ReactiveCommand.Create(hostScreen.GoBack);
        AddItem = ReactiveCommand.Create(() => {});
        HostScreen = hostScreen;
    }

    public override IHostScreen HostScreen { get; }
    
    
}