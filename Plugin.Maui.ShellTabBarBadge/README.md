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
TabBarBadge.Set(0, "9");                         // Text badge
TabBarBadge.Set(1, style: BadgeStyle.Dot);       // Dot badge
TabBarBadge.Set(2, "🍕", color: Colors.Transparent);
TabBarBadge.Set(3, "New", color: Colors.Purple);
```

------------------------------------------------------------------------

## More Information

👉 Full documentation, advanced usage, and examples are available on
GitHub:\
<https://github.com/darrabam/Plugin.Maui.ShellTabBarBadge>

------------------------------------------------------------------------

## License

MIT
