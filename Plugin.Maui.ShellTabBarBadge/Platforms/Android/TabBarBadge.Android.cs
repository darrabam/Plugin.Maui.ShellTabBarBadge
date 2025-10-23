using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;

namespace Plugin.Maui.ShellTabBarBadge;

public static partial class TabBarBadge
{
    static partial void ShowImpl(
        int tabIndex, bool isDot, string? text,
        Color textColor, Color color,
        int anchorX, int anchorY,
        HorizontalAlignment horizontal,
        VerticalAlignment vertical,
        double fontSize)
    {
        var shell = Application.Current?.Windows.FirstOrDefault()?.Page as Shell;
        if (shell == null) return;

        var handler = shell.Handler?.PlatformView as ViewGroup;
        if (handler == null) return;

        var bottomNav = FindBottomNavigationView(handler);
        if (bottomNav == null) return;

        if (tabIndex < 0 || tabIndex >= bottomNav.Menu.Size())
            return;

        var itemContainer = FindTabItemContainer(bottomNav);
        if (itemContainer == null || tabIndex >= itemContainer.ChildCount)
            return;

        var tabButton = itemContainer.GetChildAt(tabIndex) as ViewGroup;
        if (tabButton == null) return;

        var tag = 90000 + tabIndex;
        var existing = tabButton.FindViewWithTag(tag);
        if (existing != null)
            tabButton.RemoveView(existing); // ensures anchor updates

        var ctx = tabButton?.Context
                  ?? throw new InvalidOperationException("Tab context is null.");

        int dpToPx(int dp)
        {
            var metrics = ctx?.Resources?.DisplayMetrics
                          ?? throw new InvalidOperationException("DisplayMetrics unavailable.");
            return (int)(dp * metrics.Density);
        }

        // If style is Dot, replace text with a Unicode dot
        if (isDot)
            text = "●";   // could also use "•" for smaller dot

        // ---------- Unified TEXT/DOT Badge ----------
        if (!string.IsNullOrWhiteSpace(text))
        {
            var label = new AppCompatTextView(ctx)
            {
                Tag = tag,
                Gravity = GravityFlags.Center,
            };
            label.TextSize = (float)(!isDot ? fontSize : fontSize + 3);
            label.SetTextColor(textColor.ToPlatform());
            label.Text = ForceTextGlyph(text);

            int sidePad = dpToPx(6);
            label.SetPadding(sidePad, dpToPx(2), sidePad, dpToPx(2));

            if (!isDot && color != Colors.Transparent)
            {
                // pill background for text/number badges
                var pill = new Android.Graphics.Drawables.GradientDrawable();
                pill.SetColor(color.ToPlatform().ToArgb());
                pill.SetCornerRadius(dpToPx(8));
                label.Background = pill;
            }
            else
            {
                // dots always transparent background
                label.SetBackgroundColor(Android.Graphics.Color.Transparent);
                if (isDot)
                    label.SetTextColor(color.ToPlatform());
            }

            // --- measure so we can offset for left alignment ---
            label.Measure(0, 0);
            int badgeWidth = label.MeasuredWidth;
            int badgeHeight = label.MeasuredHeight;

            if (isDot)
            {
                badgeHeight = dpToPx(8);
            }

            var lp = new FrameLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent);

            // Anchor relative to icon or text
            var iconView = FindChildOfType<ImageView>(tabButton);
            var textView = FindChildOfType<TextView>(tabButton);

            if (iconView != null)
            {
                int[] iconLoc = new int[2];
                int[] tabLoc = new int[2];
                iconView.GetLocationOnScreen(iconLoc);
                tabButton.GetLocationOnScreen(tabLoc);

                int iconLeft = iconLoc[0] - tabLoc[0];
                int iconRight = iconLeft + iconView.Width;
                int iconTop = iconView.Top;
                int iconBottom = iconView.Bottom;

                // ✅ Horizontal placement
                if (horizontal == HorizontalAlignment.Right)
                    lp.LeftMargin = iconRight + dpToPx(anchorX);
                else if (horizontal == HorizontalAlignment.Center)
                    lp.LeftMargin = iconLeft + (iconView.Width / 2) - (badgeWidth / 2) + dpToPx(anchorX);
                else if (horizontal == HorizontalAlignment.Left)
                    lp.LeftMargin = iconLeft - badgeWidth + dpToPx(anchorX);

                // ✅ Vertical placement
                if (vertical == VerticalAlignment.Top)
                    lp.TopMargin = iconTop + dpToPx(anchorY);
                else if (vertical == VerticalAlignment.Center)
                    lp.TopMargin = iconTop + (iconView.Height / 2) - (badgeHeight / 2) + dpToPx(anchorY);
                else if (vertical == VerticalAlignment.Bottom)
                    lp.TopMargin = iconBottom - badgeHeight + dpToPx(anchorY);
            }
            else if (textView != null)
            {
                int[] txtLoc = new int[2];
                int[] tabLoc = new int[2];
                textView.GetLocationOnScreen(txtLoc);
                tabButton.GetLocationOnScreen(tabLoc);

                int txtLeft = txtLoc[0] - tabLoc[0];
                int txtRight = txtLeft + textView.Width;
                int txtTop = textView.Top;
                int txtBottom = textView.Bottom;

                if (horizontal == HorizontalAlignment.Right)
                    lp.LeftMargin = txtRight + dpToPx(anchorX);
                else if (horizontal == HorizontalAlignment.Center)
                    lp.LeftMargin = txtLeft + (textView.Width / 2) - (badgeWidth / 2) + dpToPx(anchorX);
                else if (horizontal == HorizontalAlignment.Left)
                    lp.LeftMargin = txtLeft - badgeWidth + dpToPx(anchorX);

                if (vertical == VerticalAlignment.Top)
                    lp.TopMargin = txtTop + dpToPx(anchorY);
                else if (vertical == VerticalAlignment.Center)
                    lp.TopMargin = txtTop + (textView.Height / 2) - (badgeHeight / 2) + dpToPx(anchorY);
                else if (vertical == VerticalAlignment.Bottom)
                    lp.TopMargin = txtBottom + dpToPx(anchorY);
            }
            else
            {
                // fallback: align to button edge
                lp.Gravity = GravityFlags.Top | GravityFlags.End;
                lp.RightMargin = dpToPx(-anchorX);
                lp.TopMargin   = dpToPx(anchorY);
            }

            // ✅ Prevent FlowDirection mirroring from pushing badge to the far edge when shell is RTL
            label.LayoutDirection = Android.Views.LayoutDirection.Ltr;
            lp.LayoutDirection = Android.Views.LayoutDirection.Ltr;
            tabButton.LayoutDirection = Android.Views.LayoutDirection.Ltr;

            tabButton.AddView(label, lp);
        }
    }


    static partial void HideImpl(int tabIndex)
    {
        var shell = Application.Current?.Windows.FirstOrDefault()?.Page as Shell;
        if (shell == null) return;

        var handler = shell.Handler?.PlatformView as ViewGroup;
        if (handler == null) return;

        var bottomNav = FindBottomNavigationView(handler);
        if (bottomNav == null) return;

        var itemContainer = FindTabItemContainer(bottomNav);
        if (itemContainer == null || tabIndex >= itemContainer.ChildCount)
            return;

        var tabButton = itemContainer.GetChildAt(tabIndex) as ViewGroup;
        if (tabButton == null) return;

        var tag = 90000 + tabIndex;
        var existing = tabButton.FindViewWithTag(tag);
        if (existing != null)
            tabButton.RemoveView(existing);
    }

    // ----- Helpers -----

    static BottomNavigationView? FindBottomNavigationView(ViewGroup root)
    {
        if (root is BottomNavigationView bnv) return bnv;
        for (int i = 0; i < root.ChildCount; i++)
        {
            if (root.GetChildAt(i) is ViewGroup vg)
            {
                var result = FindBottomNavigationView(vg);
                if (result != null) return result;
            }
        }
        return null;
    }

    static ViewGroup? FindTabItemContainer(BottomNavigationView bottomNav)
    {
        // Scan children until we find one holding clickable tab buttons
        for (int i = 0; i < bottomNav.ChildCount; i++)
        {
            if (bottomNav.GetChildAt(i) is ViewGroup vg && vg.ChildCount > 0)
            {
                if (Enumerable.Range(0, vg.ChildCount)
                              .Any(idx => vg.GetChildAt(idx)?.Clickable == true))
                {
                    return vg;
                }
            }
        }
        return null;
    }

    static T? FindChildOfType<T>(ViewGroup parent) where T : Android.Views.View
    {
        for (int i = 0; i < parent.ChildCount; i++)
        {
            var child = parent.GetChildAt(i);

            if (child is T tChild)
                return tChild;

            if (child is ViewGroup vg)
            {
                var result = FindChildOfType<T>(vg);
                if (result != null) return result;
            }
        }
        return null;
    }

    static string ForceTextGlyph(string s)
    {
        if (string.IsNullOrEmpty(s))
            return s;

        // Detect last codepoint safely (handles surrogate pairs)
        int lastCode;
        if (s.Length >= 2 && char.IsSurrogatePair(s, s.Length - 2))
            lastCode = char.ConvertToUtf32(s, s.Length - 2);
        else
            lastCode = char.ConvertToUtf32(s, s.Length - 1);

        if (lastCode == 0xFE0E || lastCode == 0xFE0F)
            return s; // already has a selector

        return s + "\uFE0E"; // enforce text presentation
    }

    static bool IsEmoji(string s)
    {
        if (string.IsNullOrEmpty(s)) return false;

        // Check if contains surrogate pair (common for emoji)
        if (char.IsSurrogatePair(s, 0)) return true;

        // Or falls in known emoji Unicode ranges
        int codepoint = char.ConvertToUtf32(s, 0);
        return (codepoint >= 0x1F000 && codepoint <= 0x1FAFF)   // Misc emoji, symbols, pictographs
            || (codepoint >= 0x1F300 && codepoint <= 0x1F5FF)   // Misc symbols & pictographs
            || (codepoint >= 0x1F600 && codepoint <= 0x1F64F)   // Emoticons
            || (codepoint >= 0x1F680 && codepoint <= 0x1F6FF)   // Transport & map symbols
            || (codepoint >= 0x2600 && codepoint <= 0x26FF);    // Misc symbols (☀, ☂, ♠, etc.)
    }

}
