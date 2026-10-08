using CustomRoleLib.Helpers;
using JetBrains.Annotations;
using LabApi.Features.Wrappers;
using SecretAPI.Features.UserSettings;
using UnityEngine;

namespace CustomRoleLib.ServerSpecificSettings;

public class AbilitySetting : CustomKeybindSetting
{
    private const float HintDurationSeconds = 5f;
    private static readonly Dictionary<string, AbilitySetting> AbilitySettings = [];

    public static AbilitySetting RegisterNew(string label,
        Func<Player, string> onActivated,
        KeyCode suggestedKey = KeyCode.None,
        bool preventInteractionOnGui = true, [CanBeNull] string hint = null)
    {
        if (AbilitySettings.TryGetValue(label, out var setting))
        {
            setting.AbilityActivated.Add(onActivated);
            return null;
        }

        return AbilitySettings[label] = new AbilitySetting(
            label, onActivated, suggestedKey, preventInteractionOnGui, hint
        );
    }

    private List<Func<Player, string>> AbilityActivated = [];

    protected AbilitySetting(string label, Func<Player, string> onActivated,
        KeyCode suggestedKey = KeyCode.None,
        bool preventInteractionOnGui = true, [CanBeNull] string hint = null)
        : base(null, label, suggestedKey, preventInteractionOnGui,
            false, hint)
    {
        AbilityActivated.Add(onActivated);
    }

    protected AbilitySetting(string label, List<Func<Player, string>> onActivated,
        KeyCode suggestedKey = KeyCode.None,
        bool preventInteractionOnGui = true, [CanBeNull] string hint = null)
        : base(null, label, suggestedKey, preventInteractionOnGui,
            false, hint)
    {
        AbilityActivated.AddRange(onActivated);
    }

    protected override CustomSetting CreateDuplicate()
        => new AbilitySetting(Label, AbilityActivated, Base.SuggestedKey,
            Base.PreventInteractionOnGUI, DescriptionHint);

    protected override void HandleSettingUpdate()
    {
        if (LastUpdateType == SettingResponseType.Initial) return;
        if (KnownOwner == null) return;
        if (!IsPressed) return;

        string response = null;
        foreach (var activator in AbilityActivated)
        {
            response = activator(KnownOwner);
            if (response != null) break;
        }

        if (response == null) return;

        var text = response == string.Empty
            ? "<color=green>Successfully activated ability.</color>"
            : $"<color=red>{response}</color>";

        if (!CustomRoleLibPlugin.RueIActive)
        {
            KnownOwner.SendHint(text, HintDurationSeconds);
            return;
        }

        RueIHelpers.DisplayHint(KnownOwner, CustomRoleLibPlugin.RueITag, text, 200f, HintDurationSeconds, true);
    }

    public override CustomHeader Header => SSSHelpers.DefaultAbilityHeader;
}