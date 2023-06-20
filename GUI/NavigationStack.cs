using System.Collections.Generic;
using GUI.ViewModels;

namespace RoutedApp;

public class NavigationStack {
    readonly List<RoutablePage> stack;

    public NavigationStack(RoutablePage defaultPage) {
        stack = new List<RoutablePage> { defaultPage };
    }

    public void GoTo(RoutablePage page) {
        stack.Add(page);
    }

    public RoutablePage GetTopPage() {
        return stack[^1];
    }

    public RoutablePage Pop() {
        int stack_len = stack.Count;

        if (stack_len <= 1) {
            return stack[0];
        }

        RoutablePage val = stack[stack_len - 1];
        stack.RemoveAt(stack_len - 1);
        return val;
    }
}