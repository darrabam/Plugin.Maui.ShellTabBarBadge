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

    public override void UpdateLayout(UITabBarController controller)
    {
        base.UpdateLayout(controller);
        s_controller = controller; // keep latest controller
    }

    static UIView? GetTabButton(int tabIndex, bool getActive)
    {
        var tabBar = s_controller?.TabBar;
        if (tabBar == null)
            return null;

        var candidates = FindLogicalTabButtons(tabBar)
            .OrderBy(v => v.Frame.X)
            .ToArray();

        int total = candidates.Length;
        int tabCount = s_controller?.TabBar.Items?.Length ?? 0;

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

        var activeLayer = GetTabButton(tabIndex, getActive: true);

        if (UIDevice.CurrentDevice.CheckSystemVersion(26, 0))
        {
            var inactiveLayer = GetTabButton(tabIndex, getActive: false);
            anchorX = -anchorX;
            ApplyBadgeToLayer(inactiveLayer);
        }

        ApplyBadgeToLayer(activeLayer);

        void ApplyBadgeToLayer(UIView? button)
        {
            if (button == null) return;

            int tag = 90000 + tabIndex;
            button.ViewWithTag(tag)?.RemoveFromSuperview();

            if (!isDot && string.IsNullOrWhiteSpace(text))
                return;

            var imageView = button.Subviews.FirstOrDefault(v => v is UIImageView);
            var labelView = button.Subviews.FirstOrDefault(v => v is UILabel);

            if (isDot)
            {
                // ---------- Tiny 8x8 dot ----------
                var dot = new UIView { Tag = tag };
                dot.BackgroundColor = bg ?? UIColor.Red;
                dot.Layer.CornerRadius = 4;
                dot.TranslatesAutoresizingMaskIntoConstraints = false;

                button.AddSubview(dot);

                NSLayoutConstraint.ActivateConstraints(new[] {
                dot.WidthAnchor.ConstraintEqualTo(8),
                dot.HeightAnchor.ConstraintEqualTo(8),
                 });

                ApplyPositionConstraints(button, dot, imageView ?? labelView, anchorX, anchorY, horizontal, vertical);
                return;
            }

            // ---------- Text / Number Badge ----------
            const float minHeight = 16f;
            const float sidePadding = 6f;

            var container = new UIView { Tag = tag };
            container.BackgroundColor = bg ?? UIColor.Red;
            container.TranslatesAutoresizingMaskIntoConstraints = false;
            container.Layer.MasksToBounds = true;
            container.Layer.CornerRadius = minHeight / 2f;

            var label = new UILabel
            {
                TextColor = fg ?? UIColor.White,
                Font = UIFont.SystemFontOfSize((nfloat)fontSize, UIFontWeight.Bold),
                TextAlignment = UITextAlignment.Center,
                Lines = 1,
                LineBreakMode = UILineBreakMode.Clip,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Text = text
            };

            button.AddSubview(container);
            container.AddSubview(label);

            NSLayoutConstraint.ActivateConstraints(new[] {
            label.TopAnchor.ConstraintEqualTo(container.TopAnchor, 1),
            label.BottomAnchor.ConstraintEqualTo(container.BottomAnchor, -1),
            label.LeadingAnchor.ConstraintEqualTo(container.LeadingAnchor, sidePadding),
            label.TrailingAnchor.ConstraintEqualTo(container.TrailingAnchor, -sidePadding),
            container.HeightAnchor.ConstraintGreaterThanOrEqualTo(minHeight),
        });

            ApplyPositionConstraints(button, container, imageView ?? labelView, anchorX, anchorY, horizontal, vertical);
        }
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
