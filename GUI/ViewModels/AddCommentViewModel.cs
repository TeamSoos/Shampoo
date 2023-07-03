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

    // Create a CustomerComment property
    private string customerComment;
    public string CustomerComment
    {
        get => customerComment;
        private set => this.RaiseAndSetIfChanged(ref customerComment, value);
    }

    // Note the ReactiveCommand<string, Unit> which means this command takes a string parameter
    public ReactiveCommand<string, Unit> SaveComment { get; set; }

    public ReactiveCommand<Unit, Unit> GoBack { get; set; }
    
    public AddCommentViewModel(IHostScreen screen)
    {
        HostScreen = screen;

        // Modify the SaveComment command to take a string parameter and set the CustomerComment property
        SaveComment = ReactiveCommand.Create<string>(comment =>
        {
            CustomerComment = comment;
        });

        GoBack = ReactiveCommand.Create(() =>
        {
            screen.GoBack();
        });
    }
}


