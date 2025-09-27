# Plugin.Maui.ShellTabBarBadge

iOS | Android
----|--------
<video src="https://github.com/user-attachments/assets/8a2c9dcb-08bb-4004-a134-2a3e6635e256" controls width="300" title="iOS Demo"></video> | <video src="https://github.com/user-attachments/assets/cbaaf2b0-6a5c-486d-85e3-0feaae3c6b6f" controls width="300" title="Android Demo"></video>

`Plugin.Maui.ShellTabBarBadge` is a lightweight yet powerful .NET MAUI plugin that adds **badges** to Shell `TabBar` items.  

- ✅ Supports **text badges**, **dot (indicator) badges**, and **hidden badges**  
- ✅ Works with **Unicode text, symbols, and emoji**  
- ✅ Fully customizable: background color, text color, font size, and badge position  
- ✅ Stateless and easy to use with a single API  

---

## Installation

Install via NuGet:

```bash
dotnet add package Plugin.Maui.ShellTabBarBadge
```

---

## Getting Started

In your `MauiProgram.cs`, enable the plugin:

```csharp
using Plugin.Maui.ShellTabBarBadge;

public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder
        .UseMauiApp<App>()  
        .UseTabBarBadge();

    return builder.Build();
}
```

---

## API Overview

The plugin exposes a single static method:

```csharp
Badge.Set(
    int tabIndex,
    string? text = null,
    Color? textColor = null,
    Color? color = null,
    int? anchorX = null,
    int? anchorY = null,
    BadgeStyle? style = null,                  // Text | Dot | Hidden
    HorizontalAlignment? horizontal = null,    // Left | Center | Right
    VerticalAlignment? vertical = null,        // Top | Center | Bottom
    double? fontSize = null
);
```

- `tabIndex` – zero-based index of the tab in the Shell `TabBar`  
- `text` – string, symbol, or emoji  
- `style` – `BadgeStyle.Text`, `BadgeStyle.Dot`, or `BadgeStyle.Hidden`  
- Colors, anchors, and alignments are optional. Defaults can be configured globally.  

---

## Quick Examples

```csharp
// Basic text badge (defaults: white text, red background)
Badge.Set(0, "New");

// Badge with custom colors
Badge.Set(1, "9", textColor: Colors.Black, color: Colors.Yellow);

// Emoji badge
Badge.Set(2, "🍕", color: Colors.Transparent);

// Dot badge (tiny indicator — ignores textColor)
Badge.Set(3, style: BadgeStyle.Dot, color: Colors.Blue);

// Hide a badge
Badge.Set(0, style: BadgeStyle.Hidden);
```

---

## Global Defaults

Configure defaults in `MauiProgram.cs`:

```csharp
builder.UseTabBarBadge(options =>
{
    options.Style = BadgeStyle.Text;
    options.TextColor = Colors.White;
    options.Color = Colors.Red;
    options.HorizontalAlignment = HorizontalAlignment.Right;
    options.VerticalAlignment = VerticalAlignment.Top;
    options.FontSize = 11;
    options.AnchorX = 0;
    options.AnchorY = 0;
});
```

---
## Supported Versions & Platforms

| Platform     | Support Status | Notes |
|--------------|----------------|-------|
| **.NET MAUI** | 9.0+ | Not tested on older versions |
| **iOS**      | iOS 16.0+ | Required by .NET MAUI 9. Not tested on iOS 15 (deployment not supported) |
| **Android**  | Android 8.0 (API 26)+ | Compatible |
| **MacCatalyst** | Not supported | Contributions are welcomed |
| **Windows**  | Not supported | Contributions are welcomed |
---

## Badge Behavior Matrix

| Style          | Text Example   | `textColor` |  `color` |
|----------------|-----------|------------------|---------------|
| Text           | `"New"`   | ✔ (applied to text) | ✔ (applied to background) |
| Text (Symbol)  | `"★"`     | ✔ (applied to sybol)      | ✔ (applied to background) |
| Text (Emoji)   | `"🍕"`    | ❌ (system controlled) | ✔ (applied to background if not transparent) |
| Dot            | ❌ (ignored)     | ❌ (ignored)     | ✔ (dot fill color) |
| Hidden         | ❌ (ignored)  | ❌ (ignored)     | ❌ (ignored) |

---

## Anchors & Alignment

Badges are positioned relative to the **tab icon (preferred)** or **tab text**.

- **HorizontalAlignment**: `Left`, `Center`, `Right`  
- **VerticalAlignment**: `Top`, `Center`, `Bottom`  
- **AnchorX/AnchorY**: fine-tune offsets (in device-independent units)  

Coordinate system:  
- `AnchorX > 0` → moves right  
- `AnchorX < 0` → moves left  
- `AnchorY > 0` → moves down  
- `AnchorY < 0` → moves up  

> **Note**  
> On **iOS**, you can position badges anywhere — even *above* the TabBar.  
> On **Android**, if the badge goes outside the TabBar bounds, it will be clipped (hidden).  

---

## Lifecycle & State

The library is **stateless**:
- It does **not track badge state**.  
- You must re-apply badges in your page lifecycle (`OnAppearing`, `OnDisappearing`) or bind via ViewModels.  

Example:

```csharp
protected override void OnDisappearing()
{
    base.OnDisappearing();
    Badge.Set(0, style: BadgeStyle.Hidden);
}
```

---

## Theme Support

Badges do **not automatically adjust** for dark/light themes.  
Handle it manually:

```csharp
Application.Current.RequestedThemeChanged += (s, e) =>
{
    if (e.RequestedTheme == AppTheme.Dark)
        Badge.Set(0, "New", textColor: Colors.White, color: Colors.Black);
    else
        Badge.Set(0, "New", textColor: Colors.Black, color: Colors.Red);
};
```

---

## Motivation

Badges are a common UX element in mobile apps, yet **.NET MAUI** and the **Community Toolkit** do not provide built-in support.  
This plugin was created to fill that gap with a **simple** API.

---

## Contributing

Contributions are welcome!  
- Add support for additional platforms (e.g., Windows)  
- Report issues and suggest enhancements  
- Submit pull requests  

---

## Credits

A significant portion of this library was co-created with **ChatGPT**.  
Made with 💚 in Saudi Arabia 
