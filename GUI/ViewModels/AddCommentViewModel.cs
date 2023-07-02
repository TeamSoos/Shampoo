using System.Reactive;
using ExCSS;
using GUI.ViewModels;
using ModelLayer.Tables;
using ReactiveUI;
using ServiceLayer.Tables;

namespace GUI.ViewModels;

public class AddCommentViewModel : RoutablePage
{
    public override IHostScreen HostScreen { get; }
    
    public ReactiveCommand<Unit, Unit> SaveComment { get; set; } 
    
    public ReactiveCommand<Unit, Unit> GoBack { get; set; }

    public AddCommentViewModel(IHostScreen screen) 
    {
        HostScreen = screen;
        
        GoBack = ReactiveCommand.Create(() =>
        {
            screen.GoBack();
        });
    }
}