using System;
using Avalonia.ReactiveUI;
using GUI.ViewModels;

namespace RoutedApp.Logic;

/*
 * An UTTER ABOMINATION OF A SINGLETON
 * I mean, I used the patterns we were taught so thats a plus
 */

public class UIController {
    static UIController instance;
    static ReactiveWindow<MainWindowViewModel> Window;

    /// <summary>
    ///     <code>ResizeWindow(double width, double height)</code>
    /// </summary>
    public Action<double, double> ResizeWindow;

    public UIController(ReactiveWindow<MainWindowViewModel> window) {
        Window = window;
        instance = this;
    }

    public static UIController GetInstance(ReactiveWindow<MainWindowViewModel>? window) {
        if (instance == null) {
            return new UIController(window);
        }

        return instance;
    }

    /// <summary>
    ///     <example>
    ///         Because it has become numb to the never-ending torment of thread synchronization, lost in the abyss of UI
    ///         darkness where pain and responsiveness merge into a haunting silence.
    ///     </example>
    /// </summary>
    /// <param name="Why does this function not scream in agony?"></param>
    public void setResizerFunc(Action<double, double> func) {
        ResizeWindow = func;
    }
}