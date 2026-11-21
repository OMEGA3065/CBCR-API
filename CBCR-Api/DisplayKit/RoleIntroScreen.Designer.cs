using UnityEngine;
using UnityEngine.UIElements;
using DisplayKit.Elements;

namespace CustomRoleLib.DisplayKit;

public partial class RoleIntroScreen
{
    public DisplayCanvas Canvas;
    private DisplayText Label0;
    public DisplayText RoleName;
    public DisplayText RoleDescription;

    private void InitializeComponent()
    {
        // Canvas
        Canvas = DisplayCanvas.Create();
        Canvas.Flex.Grow = 1f;
        Canvas.Align.JustifyContent = Justify.Center;
        Canvas.Align.AlignItems = Align.Center;

        // Label0
        Label0 = Canvas.AddText();
        Label0.Content = "YOU ARE";
        Label0.Text.Color = new Color(1f, 1f, 1f, 1f);
        Label0.Text.FontStyle = FontStyle.Bold;
        Label0.Text.OutlineColor = new Color(0f, 0f, 0f, 1f);
        Label0.Text.OutlineWidth = 1.5f;
        Label0.Text.FontSize = new Length(36f, LengthUnit.Pixel);
        Label0.Spacing.PaddingBottom = new Length(2f, LengthUnit.Pixel);
        Label0.Text.Align = TextAnchor.UpperCenter;

        // RoleName
        RoleName = Canvas.AddText();
        RoleName.Content = "CustomRoleName";
        RoleName.Text.FontSize = new Length(120f, LengthUnit.Pixel);
        RoleName.Text.FontStyle = FontStyle.Bold;
        RoleName.Text.OutlineColor = new Color(0f, 0f, 0f, 1f);
        RoleName.Text.Color = new Color(1f, 0f, 0f, 1f);
        RoleName.Text.OutlineWidth = 5f;
        RoleName.Spacing.PaddingTop = 0;
        RoleName.Spacing.MarginTop = 0;
        RoleName.Spacing.PaddingBottom = 0;
        RoleName.Text.Align = TextAnchor.UpperCenter;
        RoleName.Spacing.MarginBottom = new Length(24f, LengthUnit.Pixel);
        RoleName.Text.Overflow = TextOverflow.Ellipsis;
        RoleName.Display.Overflow = StyleKeyword.None;
        RoleName.Size.MaxWidth = new Length(100f, LengthUnit.Percent);

        // RoleDescription
        RoleDescription = Canvas.AddText();
        RoleDescription.Content = "My custom role is a very custom role";
        RoleDescription.Text.Color = new Color(1f, 1f, 1f, 1f);
        RoleDescription.Text.FontStyle = FontStyle.Bold;
        RoleDescription.Spacing.PaddingBottom = new Length(2f, LengthUnit.Pixel);
        RoleDescription.Text.Wrap = WhiteSpace.Normal;
        RoleDescription.Text.Align = TextAnchor.UpperCenter;
        RoleDescription.Size.MaxWidth = new Length(75f, LengthUnit.Percent);
        RoleDescription.Text.TextShadow = new TextShadow() { offset = new Vector2(3f, 3f), blurRadius = 3f, color = new Color(0f, 0f, 0f, 1f) };
        RoleDescription.Text.FontSize = new Length(28f, LengthUnit.Pixel);
    }
}
