using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Maui.ShellTabBarBadge;


/// <summary>
/// Defines the available styles for tab bar badges.
/// </summary>
public enum BadgeStyle
{
    /// <summary>
    /// No badge is displayed (or clears the current badge if one exists).
    /// </summary>
    Hidden,

    /// <summary>
    /// A badge rendered using Unicode text (letters, numbers, symbols, or emoji)
    /// inside a pill-shaped background.  
    /// Examples: "9", "New", "⚡", "🍕", "💚".
    /// </summary>
    Text,

    /// <summary>
    /// A small colored dot badge (8–10px).  
    /// The dot color is controlled by the <c>color</c> parameter.  
    /// Ignores text, text size, and text color settings.
    /// </summary>
    Dot
}