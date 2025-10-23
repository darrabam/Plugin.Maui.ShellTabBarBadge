# Plugin.Maui.ShellTabBarBadge

iOS | Android
----|--------
<table>
  <tr>
    <td><img src="https://github.com/user-attachments/assets/73439aed-46dd-4479-b234-3c2a6639b3bc" width="340"/></td>
    <td><img src="https://github.com/user-attachments/assets/5255d77b-ea6e-412d-b1c0-40dcc6f38974" width="340"/></td>
  </tr>
</table>

Windows | Mac Catalyst
----|--------
<table>
  <tr>
    <td><img src="https://github.com/user-attachments/assets/b876cb44-869b-4f4d-924d-736c73e4e27d" width="340"/></td>
    <td><img src="https://github.com/user-attachments/assets/e0c08740-ee7c-498a-94a6-8852ae3ae722" width="340"/></td>
  </tr>
</table>

`Plugin.Maui.ShellTabBarBadge` is a lightweight yet powerful .NET MAUI plugin that adds **badges** to Shell `TabBar` items.  

- ✅ Supports **text badges**, **dot (indicator) badges**
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
TabBarBadge.Set(
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
TabBarBadge.Set(0, "New");

// Badge with custom colors
TabBarBadge.Set(1, "9", textColor: Colors.Black, color: Colors.Yellow);

// Emoji badge
TabBarBadge.Set(2, "🍕", color: Colors.Transparent);

// Dot badge (tiny indicator — ignores textColor)
TabBarBadge.Set(3, style: BadgeStyle.Dot, color: Colors.Blue);

// Hide a badge
TabBarBadge.Set(0, style: BadgeStyle.Hidden);
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

| Platform        | Support Status | Notes |
|-----------------|----------------|-------|
| **.NET MAUI**   | 9.0+ | Not tested on older versions |
| **iOS**         | ✅ Supported (iOS 16.0+) | Required by .NET MAUI 9. Not tested on iOS 15 (deployment not supported) |
| **Android**     | ✅ Supported (Android 8.0 / API 26 +) | Fully compatible |
| **Mac Catalyst**| ✅ Supported | Uses same UIKit implementation as iOS |
| **Windows**     | ✅ Supported (Windows 10 / Build 19041+) | Implemented via WinUI NavigationView badges |

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
> On **Android** & **Windows**, if the badge goes outside the TabBar bounds, it will be clipped (hidden).  

---

## Lifecycle & State

The library is **stateless**:
- It does **not track badge state**.  
- You must manually update badge status, typically within page lifecycle methods such as `OnAppearing` and `OnDisappearing`, or in other relevant event handlers. 

Example:

```csharp
protected override void OnDisappearing()
{
    base.OnDisappearing();
    TabBarBadge.Set(0, style: BadgeStyle.Hidden);
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
        TabBarBadge.Set(0, "New", textColor: Colors.White, color: Colors.Black);
    else
        TabBarBadge.Set(0, "New", textColor: Colors.Black, color: Colors.Red);
};
```

---

## Motivation

Badges are a common UX element in mobile apps, yet **.NET MAUI** and the **Community Toolkit** do not provide built-in support.  
This plugin was created to fill that gap with a **simple** API.

---

## Contributing

Contributions are welcome!  
- Report issues and suggest enhancements  
- Submit pull requests  

---

## ⭐ Show Your Support

If you find **Plugin.Maui.ShellBadgeTabBarBadge** useful in your projects, please consider giving the repository a ⭐.  
It helps others discover the library and motivates continued development.

## Credits

A significant portion of this library was co-created with **ChatGPT**.  
Made with 💚 in Saudi Arabia 
