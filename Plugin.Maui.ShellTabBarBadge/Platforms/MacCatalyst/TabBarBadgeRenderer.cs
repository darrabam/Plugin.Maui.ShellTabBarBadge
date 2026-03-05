using CoreFoundation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;

namespace Plugin.Maui.ShellTabBarBadge;

/// <summary>
/// Custom Shell renderer that injects badge support into the iOS tab bar.
/// </summary>
public class TabBarBadgeRenderer : ShellRenderer
{
    /// <summary>
    /// Creates the custom tab bar appearance tracker that adds badge rendering support.
    /// </summary>
    /// <returns>
    /// A <see cref="BadgeShellTabBarAppearanceTracker"/> instance for managing tab bar badges.
    /// </returns>
    protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
        => new BadgeShellTabBarAppearanceTracker();
}

// Tracks UITabBarController and draws dot/label badges per tab
internal class BadgeShellTabBarAppearanceTracker : ShellTabBarAppearanceTracker
{
    static UITabBarController? s_controller;

    // 🔁 Persistent badge storage (keeps badge alive across icon changes / navigation)
    static readonly Dictionary<int, BadgeState> _badgeStates = new();

    class BadgeState
    {
        public bool IsDot;
        public string? Text;
        public UIColor? Bg;
        public UIColor? Fg;
        public int AnchorX;
        public int AnchorY;
        public HorizontalAlignment Horizontal;
        public VerticalAlignment Vertical;
        public double FontSize;
    }

    public override void UpdateLayout(UITabBarController controller)
    {
        base.UpdateLayout(controller);
        s_controller = controller; // keep latest controller

        // 🔁 Reapply all stored badges after any layout rebuild
        foreach (var index in _badgeStates.Keys.ToList())
        {
            ApplyStoredBadge(index);
        }
    }

    static UIView? GetTabButton(int tabIndex, bool getActive)
    {
        int tabCount = s_controller?.TabBar?.Items?.Length ?? 0;
        if (tabIndex < 0 || tabIndex >= tabCount)
        {
            #if DEBUG
            System.Diagnostics.Debug.WriteLine($"[ShellTabBarBadge] Invalid tab index {tabIndex}. Valid range is 0–{tabCount - 1} (tab count: {tabCount})");
            #endif
            return null;
        }

        var tabBar = s_controller?.TabBar;
        if (tabBar == null)
            return null;

        var candidates = FindLogicalTabButtons(tabBar)
            .OrderBy(v => v.Frame.X)
            .ToArray();

        int total = candidates.Length;

        if (tabCount == 0 || total == 0)
            return null;

        int layersPerTab = Math.Max(1, total / tabCount);

        int start = tabIndex * layersPerTab;
        int end = Math.Min(start + layersPerTab, total);

        var group = candidates[start..end]; // all layers for this tab

        return PickLayer(group, getActive, tabIndex);
    }

    static UIView PickLayer(UIView[] layers, bool getActive, int tabIndex)
    {
        if (layers.Length == 1)
            return layers[0];

        // 1) Try Z-order (most reliable across iOS versions)
        var super = layers[0].Superview;
        if (super != null)
        {
            var ordered = layers.OrderBy(v => Array.IndexOf(super.Subviews, v)).ToArray();
            return getActive ? ordered.Last() : ordered.First();
        }

        // 2) Last fallback: active tab logic
        bool isActive = IsActiveTab(tabIndex);
        return getActive
            ? (isActive ? layers[0] : layers.Last())
            : (isActive ? layers.Last() : layers[0]);
    }

    static bool IsActiveTab(int tabIndex)
    {
        if (s_controller == null) return false;
        if (s_controller.SelectedViewController == null) return false;

        var item = s_controller.TabBar.Items?[tabIndex];
        return item == s_controller.SelectedViewController.TabBarItem;
    }

    static IEnumerable<UIView> FindLogicalTabButtons(UIView root)
    {
        foreach (var v in root.Subviews)
        {
            if (v is UIControl)
            {
                bool hasImage = v.Subviews.Any(x => x is UIImageView);

                if (hasImage)
                    yield return v;
            }

            foreach (var child in FindLogicalTabButtons(v))
                yield return child;
        }
    }

    internal static void SetBadge(
        int tabIndex,
        bool isDot,
        string? text,
        UIColor? bg,
        UIColor? fg,
        int anchorX,
        int anchorY,
        HorizontalAlignment horizontal,
        VerticalAlignment vertical,
        double fontSize)
    {
        var tabCount = s_controller?.TabBar?.Items?.Length ?? 0;
        if (tabIndex < 0 || tabIndex >= tabCount)
        {
            #if DEBUG
            System.Diagnostics.Debug.WriteLine($"[ShellTabBarBadge] Invalid tab index {tabIndex}. Valid range is 0–{tabCount - 1} (tab count: {tabCount})");
            #endif
            return;
        }
        // 🔒 Handle hidden state (removes badge permanently)
        if (!isDot && string.IsNullOrWhiteSpace(text))
        {
            _badgeStates.Remove(tabIndex);
            RemoveBadge(tabIndex);
            return;
        }

        // 🔁 Store badge state so it survives icon rebuilds
        _badgeStates[tabIndex] = new BadgeState
        {
            IsDot = isDot,
            Text = text,
            Bg = bg,
            Fg = fg,
            AnchorX = anchorX,
            AnchorY = anchorY,
            Horizontal = horizontal,
            Vertical = vertical,
            FontSize = fontSize
        };

        ApplyStoredBadge(tabIndex);
    }

    static void ApplyStoredBadge(int tabIndex)
    {
        var tabCount = s_controller?.TabBar?.Items?.Length ?? 0;
        if (tabIndex < 0 || tabIndex >= tabCount)
        {
            #if DEBUG
            System.Diagnostics.Debug.WriteLine($"[ShellTabBarBadge] Invalid tab index {tabIndex}. Valid range is 0–{tabCount - 1} (tab count: {tabCount})");
            #endif
            return;
        }

        if (!_badgeStates.TryGetValue(tabIndex, out var state))
            return;

        DispatchQueue.MainQueue.DispatchAsync(() =>
        {
            s_controller?.TabBar?.LayoutIfNeeded();

            var activeLayer = GetTabButton(tabIndex, getActive: true);

            if (UIDevice.CurrentDevice.CheckSystemVersion(26, 0))
            {
                var inactiveLayer = GetTabButton(tabIndex, getActive: false);
                ApplyBadgeToLayer(inactiveLayer, tabIndex, state);
            }

            ApplyBadgeToLayer(activeLayer, tabIndex, state);
        });
    }

    static void RemoveBadge(int tabIndex)
    {
        DispatchQueue.MainQueue.DispatchAsync(() =>
        {
            var activeLayer = GetTabButton(tabIndex, true);
            var inactiveLayer = GetTabButton(tabIndex, false);

            activeLayer?.ViewWithTag(90000 + tabIndex)?.RemoveFromSuperview();
            inactiveLayer?.ViewWithTag(90000 + tabIndex)?.RemoveFromSuperview();
        });
    }

    static void ApplyBadgeToLayer(UIView? button, int tabIndex, BadgeState state)
    {
        if (button == null) return;

        int tag = 90000 + tabIndex;
        button.ViewWithTag(tag)?.RemoveFromSuperview();

        var imageView = button.Subviews.FirstOrDefault(v => v is UIImageView);
        var labelView = button.Subviews.FirstOrDefault(v => v is UILabel);

        if (state.IsDot)
        {
            // ---------- Tiny 8x8 dot ----------
            var dot = new UIView { Tag = tag };
            dot.BackgroundColor = state.Bg ?? UIColor.Red;
            dot.Layer.CornerRadius = 4;
            dot.TranslatesAutoresizingMaskIntoConstraints = false;

            button.AddSubview(dot);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                dot.WidthAnchor.ConstraintEqualTo(8),
                dot.HeightAnchor.ConstraintEqualTo(8),
            });

            ApplyPositionConstraints(button, dot, imageView ?? labelView,
                state.AnchorX, state.AnchorY,
                state.Horizontal, state.Vertical);

            return;
        }

        // ---------- Text / Number Badge ----------
        const float minHeight = 16f;
        const float sidePadding = 6f;

        var container = new UIView { Tag = tag };
        container.BackgroundColor = state.Bg ?? UIColor.Red;
        container.TranslatesAutoresizingMaskIntoConstraints = false;
        container.Layer.MasksToBounds = true;
        container.Layer.CornerRadius = minHeight / 2f;

        var label = new UILabel
        {
            TextColor = state.Fg ?? UIColor.White,
            Font = UIFont.SystemFontOfSize((nfloat)state.FontSize, UIFontWeight.Bold),
            TextAlignment = UITextAlignment.Center,
            Lines = 1,
            LineBreakMode = UILineBreakMode.Clip,
            TranslatesAutoresizingMaskIntoConstraints = false,
            Text = state.Text
        };

        button.AddSubview(container);
        container.AddSubview(label);

        NSLayoutConstraint.ActivateConstraints(new[]
        {
            label.TopAnchor.ConstraintEqualTo(container.TopAnchor, 1),
            label.BottomAnchor.ConstraintEqualTo(container.BottomAnchor, -1),
            label.LeadingAnchor.ConstraintEqualTo(container.LeadingAnchor, sidePadding),
            label.TrailingAnchor.ConstraintEqualTo(container.TrailingAnchor, -sidePadding),
            container.HeightAnchor.ConstraintGreaterThanOrEqualTo(minHeight),
        });

        ApplyPositionConstraints(button, container, imageView ?? labelView,
            state.AnchorX, state.AnchorY,
            state.Horizontal, state.Vertical);
    }

    static void ApplyPositionConstraints(
        UIView button,
        UIView badge,
        UIView? refView,
        int anchorX,
        int anchorY,
        HorizontalAlignment horizontal,
        VerticalAlignment vertical)
    {
        if (refView != null)
        {
            // Horizontal
            if (horizontal == HorizontalAlignment.Right)
                badge.LeftAnchor.ConstraintEqualTo(refView.RightAnchor, anchorX).Active = true;
            else if (horizontal == HorizontalAlignment.Center)
                badge.CenterXAnchor.ConstraintEqualTo(refView.CenterXAnchor, anchorX).Active = true;
            else // Left
                badge.RightAnchor.ConstraintEqualTo(refView.LeftAnchor, anchorX).Active = true;

            // Vertical
            if (vertical == VerticalAlignment.Top)
                badge.TopAnchor.ConstraintEqualTo(refView.TopAnchor, anchorY - 6).Active = true;
            else if (vertical == VerticalAlignment.Center)
                badge.CenterYAnchor.ConstraintEqualTo(refView.CenterYAnchor, anchorY).Active = true;
            else // Bottom
                badge.BottomAnchor.ConstraintEqualTo(refView.BottomAnchor, anchorY).Active = true;
        }
        else
        {
            // fallback: relative to tab button
            if (horizontal == HorizontalAlignment.Right)
                badge.RightAnchor.ConstraintEqualTo(button.RightAnchor, -anchorX).Active = true;
            else if (horizontal == HorizontalAlignment.Center)
                badge.CenterXAnchor.ConstraintEqualTo(button.CenterXAnchor, anchorX).Active = true;
            else
                badge.LeftAnchor.ConstraintEqualTo(button.LeftAnchor, anchorX).Active = true;

            if (vertical == VerticalAlignment.Top)
                badge.TopAnchor.ConstraintEqualTo(button.TopAnchor, anchorY - 6).Active = true;
            else if (vertical == VerticalAlignment.Center)
                badge.CenterYAnchor.ConstraintEqualTo(button.CenterYAnchor, anchorY).Active = true;
            else
                badge.BottomAnchor.ConstraintEqualTo(button.BottomAnchor, -anchorY).Active = true;
        }
    }
}