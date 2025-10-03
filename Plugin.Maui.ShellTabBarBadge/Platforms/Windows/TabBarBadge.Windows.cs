using System;
using System.Linq;
using Microsoft.Maui.Platform;
using MauiControls = Microsoft.Maui.Controls;
using MauiGraphics = Microsoft.Maui.Graphics;
using WinUI = Microsoft.UI.Xaml;
using WinControls = Microsoft.UI.Xaml.Controls;
using WinMedia = Microsoft.UI.Xaml.Media;
using WinShapes = Microsoft.UI.Xaml.Shapes;

namespace Plugin.Maui.ShellTabBarBadge;

/// <summary>
/// Provides platform-specific implementation of the TabBarBadge 
/// for Windows (WinUI). Used internally by the plugin.
/// </summary>
public static partial class TabBarBadge
{
    /// <summary>
    /// Shows a badge on the given tab index.
    /// </summary>
    /// <param name="tabIndex">Zero-based index of the tab.</param>
    /// <param name="isDot">True if rendering a dot badge, otherwise false.</param>
    /// <param name="text">The badge text (ignored if dot).</param>
    /// <param name="textColor">Text color.</param>
    /// <param name="color">Background color.</param>
    /// <param name="anchorX">Horizontal offset.</param>
    /// <param name="anchorY">Vertical offset.</param>
    /// <param name="horizontal">Horizontal alignment relative to the tab item.</param>
    /// <param name="vertical">Vertical alignment relative to the tab item.</param>
    /// <param name="fontSize">Font size for text badges.</param>
   
    static partial void ShowImpl(
        int tabIndex,
        bool isDot,
        string? text,
        MauiGraphics.Color textColor,
        MauiGraphics.Color color,
        int anchorX,
        int anchorY,
        MauiGraphics.HorizontalAlignment horizontal,
        MauiGraphics.VerticalAlignment vertical,
        double fontSize)
    {
        // ✅ get the Window
        var win = MauiControls.Application.Current?.Windows.FirstOrDefault()
            ?.Handler?.PlatformView as WinUI.Window;
        if (win == null) return;

        var root = win.Content as WinUI.FrameworkElement;
        if (root == null) return;

        var navView = FindChild<WinControls.NavigationView>(root);
        if (navView == null || tabIndex < 0) return;

        // ✅ Get realized items from visual tree
        var navItems = navView.GetDescendants()
                              .OfType<WinControls.NavigationViewItem>()
                              .ToList();

        if (tabIndex >= navItems.Count) return;

        var menuItem = navItems[tabIndex];
        if (menuItem == null) return;

        var tag = 90000 + tabIndex;

        // Ensure content is a Grid so we can overlay a badge
        //var container = menuItem.Content as WinControls.Grid;
        var container = FindLayoutRoot(menuItem);
        if (container == null) return;

        // remove existing badge first
        var oldBadge = container.Children
            .OfType<WinUI.FrameworkElement>()
            .FirstOrDefault(c => (int?)c.Tag == tag);

        if (oldBadge != null)
            container.Children.Remove(oldBadge);

        if (container == null)
        {
            container = new WinControls.Grid();

            if (menuItem.Content is string s)
                container.Children.Add(new WinControls.TextBlock { Text = s });
            else if (menuItem.Content is WinUI.FrameworkElement fe)
                container.Children.Add(fe);

            menuItem.Content = container;
        }

        WinUI.FrameworkElement badge;
        if (isDot)
        {
            badge = new WinShapes.Ellipse
            {
                Width = 8,
                Height = 8,
                Fill = new WinMedia.SolidColorBrush(color.ToWindowsColor()),
                Stroke = new WinMedia.SolidColorBrush(textColor.ToWindowsColor()),
                StrokeThickness = 1.5,
                Tag = tag
            };
        }
        else
        {
            var tb = new WinControls.TextBlock
            {
                Text = text,
                Foreground = new WinMedia.SolidColorBrush(textColor.ToWindowsColor()),
                FontSize = fontSize,
                HorizontalAlignment = WinUI.HorizontalAlignment.Center,
                VerticalAlignment = WinUI.VerticalAlignment.Center,
                Margin = new WinUI.Thickness(4, 0, 4, 0),
                Tag = tag
            };

            var border = new WinControls.Border
            {
                Background = new WinMedia.SolidColorBrush(color.ToWindowsColor()),
                CornerRadius = new WinUI.CornerRadius(8),
                Padding = new WinUI.Thickness(6, 2, 6, 2),
                Child = tb,
                Tag = tag
            };
            badge = border;
        }

        badge.HorizontalAlignment = ConvertHAlign(horizontal);
        badge.VerticalAlignment = ConvertVAlign(vertical);
        badge.Margin = new WinUI.Thickness(anchorX, anchorY, -anchorX, -anchorY);

        container.Children.Add(badge);
        //container.Background = new WinMedia.SolidColorBrush(MauiGraphics.Colors.Yellow.ToWindowsColor());
    }

    static partial void HideImpl(int tabIndex)
    {
        var win = MauiControls.Application.Current?.Windows.FirstOrDefault()
            ?.Handler?.PlatformView as WinUI.Window;
        if (win == null) return;

        var root = win.Content as WinUI.FrameworkElement;
        if (root == null) return;

        var navView = FindChild<WinControls.NavigationView>(root);
        if (navView == null || tabIndex < 0) return;

        // ✅ Use visual tree instead of MenuItems
        var navItems = navView.GetDescendants()
                              .OfType<WinControls.NavigationViewItem>()
                              .ToList();

        if (tabIndex >= navItems.Count) return;

        var menuItem = navItems[tabIndex];
        if (menuItem?.Content is WinControls.Grid grid)
        {
            var tag = 90000 + tabIndex;
            var existing = grid.Children.OfType<WinUI.FrameworkElement>()
                                        .FirstOrDefault(c => (int?)c.Tag == tag);
            if (existing != null)
                grid.Children.Remove(existing);
        }
    }

    static T? FindChild<T>(WinUI.FrameworkElement parent) where T : WinUI.FrameworkElement
    {
        if (parent is T t) return t;
        int count = WinMedia.VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < count; i++)
        {
            var child = WinMedia.VisualTreeHelper.GetChild(parent, i) as WinUI.FrameworkElement;
            var result = child != null ? FindChild<T>(child) : null;
            if (result != null) return result;
        }
        return null;
    }

    static T? FindChildByTag<T>(WinUI.FrameworkElement parent, int tag) where T : WinUI.FrameworkElement
    {
        if (parent is T t && (int?)t.Tag == tag) return t;
        int count = WinMedia.VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < count; i++)
        {
            var child = WinMedia.VisualTreeHelper.GetChild(parent, i) as WinUI.FrameworkElement;
            var result = child != null ? FindChildByTag<T>(child, tag) : null;
            if (result != null) return result;
        }
        return null;
    }

    static WinUI.HorizontalAlignment ConvertHAlign(MauiGraphics.HorizontalAlignment h)
        => h switch
        {
            MauiGraphics.HorizontalAlignment.Left => WinUI.HorizontalAlignment.Left,
            MauiGraphics.HorizontalAlignment.Center => WinUI.HorizontalAlignment.Center,
            MauiGraphics.HorizontalAlignment.Right => WinUI.HorizontalAlignment.Right,
            _ => WinUI.HorizontalAlignment.Right
        };

    static WinUI.VerticalAlignment ConvertVAlign(MauiGraphics.VerticalAlignment v)
        => v switch
        {
            MauiGraphics.VerticalAlignment.Top => WinUI.VerticalAlignment.Top,
            MauiGraphics.VerticalAlignment.Center => WinUI.VerticalAlignment.Center,
            MauiGraphics.VerticalAlignment.Bottom => WinUI.VerticalAlignment.Bottom,
            _ => WinUI.VerticalAlignment.Top
        };

    static WinControls.Grid? FindLayoutRoot(WinControls.NavigationViewItem item)
    {
        return item.GetDescendants()
                   .OfType<WinControls.Grid>()
                   .FirstOrDefault(g => g.Name == "LayoutRoot");
    }

}


/// <summary>
/// Provides extension methods for traversing the WinUI visual tree.
/// </summary>
public static class VisualTreeExtensions
{
    /// <summary>
    /// Recursively enumerates all descendant elements of the given parent
    /// in the WinUI visual tree.
    /// </summary>
    /// <param name="parent">The root dependency object to start traversal from.</param>
    /// <returns>An enumerable collection of descendant <see cref="Microsoft.UI.Xaml.DependencyObject"/>s.</returns>
    public static System.Collections.Generic.IEnumerable<WinUI.DependencyObject> GetDescendants(this WinUI.DependencyObject parent)
    {
        int count = WinMedia.VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < count; i++)
        {
            var child = WinMedia.VisualTreeHelper.GetChild(parent, i);
            yield return child;

            foreach (var descendant in GetDescendants(child))
                yield return descendant;
        }
    }
}

