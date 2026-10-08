using LabApi.Features.Wrappers;

namespace CustomRoleLib.Helpers;

public static class RueIHelpers
{
    public static void DisplayHint(Player player, string tag, string hint, float position, float? duration,
        bool showToSpectators = false, bool resolutionBasedAlign = false)
        => DisplayHint(player, new RueI.API.Elements.Tag(tag), hint, position, duration, showToSpectators,
            resolutionBasedAlign);

    public static void DisplayHint(Player player, object tag, string hint, float position, float? duration, bool showToSpectators = false, bool resolutionBasedAlign = false)
    {
        var display = RueI.API.RueDisplay.Get(player);
        var element = new RueI.API.Elements.BasicElement(position, hint)
        {
            ShowToSpectators = showToSpectators,
            ResolutionBasedAlign = resolutionBasedAlign
        };

        if (duration.HasValue)
            display.Show((RueI.API.Elements.Tag)tag, element, duration.Value);
        else
            display.Show((RueI.API.Elements.Tag)tag, element);
    }

    public static void RemoveHint(Player player, string tag)
        => RemoveHint(player, new RueI.API.Elements.Tag(tag));

    public static void RemoveHint(Player player, object tag)
    {
        var display = RueI.API.RueDisplay.Get(player);
        display.Remove((RueI.API.Elements.Tag)tag);
    }
}