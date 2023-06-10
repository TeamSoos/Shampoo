using System;
using Avalonia.ReactiveUI;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoutedApp.ViewModels;

namespace RoutedApp.Logic; 

/*
 * An UTTER ABOMINATION OF A SINGLETON
 * I mean, I used the patterns we were taught so thats a plus
 */

public class UIController
{
  private static UIController instance;
  private static ReactiveWindow<MainWindowViewModel> Window;

  public UIController(ReactiveWindow<MainWindowViewModel> window) {
    UIController.Window = window;
    UIController.instance = this;
  }

  public static UIController GetInstance(ReactiveWindow<MainWindowViewModel>? window)
  {
    if (instance == null) {
      return new UIController(window);
    }
    return instance;
  }

  /// <summary>
  /// <example>Because it has become numb to the never-ending torment of thread synchronization, lost in the abyss of UI darkness where pain and responsiveness merge into a haunting silence.</example>
  /// </summary>
  /// <param name="Why does this function not scream in agony?"></param>
  public void setResizerFunc(Action<double, double> func) {
    ResizeWindow = func;
  }
  /// <summary>
  /// <code>ResizeWindow(double width, double height)</code>
  /// </summary>
  public Action<double, double> ResizeWindow;
}
