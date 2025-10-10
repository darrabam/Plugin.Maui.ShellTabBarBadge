# Plugin.Maui.ShellTabBarBadge

A .NET MAUI plugin that adds customizable badges to Shell TabBar items.

------------------------------------------------------------------------

## Installation

``` bash
dotnet add package Plugin.Maui.ShellTabBarBadge
```

------------------------------------------------------------------------

## Quick Usage

In your **MauiProgram.cs**:

``` csharp
builder.UseTabBarBadge();

// Example usage
TabBarBadge.Set(0, "9");								// Shows number 9 on red pill-shaped badge on Tab 0
TabBarBadge.Set(1, style: BadgeStyle.Dot);				// Shows a red Dot badge on Tab 1
TabBarBadge.Set(2, "🍕", color: Colors.Transparent);		// Shows a pizza badge on Tab 2
TabBarBadge.Set(3, "New", color: Colors.Purple);		// Shows the word New on purple pill-shaped badge on Tab 3
TabBarBadge.Set(0, style: BadgeStyle.Hidden);			// Hides badge on Tab 0
```

------------------------------------------------------------------------

## More Information

👉 Full documentation, advanced usage, and examples are available on
GitHub:\
<https://github.com/darrabam/Plugin.Maui.ShellTabBarBadge>

------------------------------------------------------------------------

## License

MIT
