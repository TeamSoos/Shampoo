using System.Collections.Generic;
using GUI.ViewModels;
using RoutedApp.ViewModels;

namespace RoutedApp;

public class NavigationStack {
  private List<RoutablePage> stack;

  public void GoTo(RoutablePage page) {
    stack.Add(page);
  }

  public RoutablePage GetTopPage() {
    return stack[^1];
  }

  public RoutablePage Pop() {
    var stack_len = stack.Count;

    if (stack_len <= 1) {
      return stack[0];
    }

    var val = stack[stack_len - 1];
    stack.RemoveAt(stack_len - 1);
    return val;
  }

  public NavigationStack(RoutablePage defaultPage) {
    stack = new List<RoutablePage> { defaultPage };
  }
}