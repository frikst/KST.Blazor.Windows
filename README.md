# Blazor Windows

Blazor is an amazing technology. In combination with PWA concepts and some bleeding-edge JavaScript APIs
it has power to replace standard desktop applications. There is one important feature missing: possibility to open a new window at any screen connected to
the computer. And it is now possible with this library.

## Why is it important?

In the standard web development world, there is no need to open a new windows. SPAs are being designed with
single window usage in mind. When a dialog window is needed, it is simply shown as a part of the page.
There is many libraries to do that.

On the other hand, when you want to present many information to the user at the same time (stock exchange
information, power plant status information, big train station status information, etc...), support for
multiple screens can be handy. And showing information at multiple screens means opening multiple windows.

## How can I setup the library?

1) Simply add it to your project using nuget.

```
Install-Package KST.Blazor.Windows
```
or
```
dotnet add package KST.Blazor.Windows
```

2) Register the library into dependency injection container

```csharp
services.AddBlazorWindows();
```

In this step, you can choose to enable support for [Multi-Screen Window Placement API](https://webscreens.github.io/window-placement/)
in the supported browser:
```csharp
services.AddBlazorWindows(cfg =>
{
    cfg.EnableMultiScreenWindowPlacement = true;
});
```

3) Add `<WindowContainer />` helper component at the beginning of the `App.razor` file. So you have
something like this:
```cshtml
<WindowContainer />

<Router AppAssembly="@typeof(Program).Assembly" PreferExactMatches="@true">
    <Found Context="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
    </Found>
    <NotFound>
        <LayoutView Layout="@typeof(MainLayout)">
            <p>Sorry, there's nothing at this address.</p>
        </LayoutView>
    </NotFound>
</Router>
```

4) Now you can open as many windows as you likes.

## How to open a new window?

To open a new window, you can just inject `IWindowManager` and use its methods:

```cshtml
@inject IWindowManager WindowManager

<button @onclick="OpenWindow">Open component in a new window</button>

@code {
    private async Task OpenWindow() {
        // Opens new instance of WindowComponent component in a new window
        await this.WindowManager.OpenWindowAsync<WindowComponent>();
    }
}
```

## Examples

More examples can be found in [Examples](Examples/Examples) project.
