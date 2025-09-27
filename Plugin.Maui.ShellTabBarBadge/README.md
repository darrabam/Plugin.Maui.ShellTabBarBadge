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
Badge.Set(0, "9");                         // Text badge
Badge.Set(1, style: BadgeStyle.Dot);       // Dot badge
Badge.Set(2, "🍕", color: Colors.Transparent);
Badge.Set(3, "New", color: Colors.Purple);
```

------------------------------------------------------------------------

## More Information

👉 Full documentation, advanced usage, and examples are available on
GitHub:\
<https://github.com/darrabam/Plugin.Maui.ShellTabBarBadge>

------------------------------------------------------------------------

## License

MIT
