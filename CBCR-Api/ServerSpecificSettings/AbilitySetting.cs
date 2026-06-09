using JetBrains.Annotations;
using LabApi.Features.Wrappers;
using RueI.API;
using RueI.API.Elements;
using SecretAPI.Features.UserSettings;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace CustomRoleLib.ServerSpecificSettings;

public class AbilitySetting : CustomKeybindSetting
{
    private static readonly Dictionary<string, AbilitySetting> AbilitySettings = [];

    public static AbilitySetting RegisterNew(string label,
        Func<Player, string> onActivated,
        KeyCode suggestedKey = KeyCode.None,
        bool preventInteractionOnGui = true, [CanBeNull] string hint = null)
    {
        if (AbilitySettings.ContainsKey(label))
        {
            Logger.Error($"Ability {label} is already registered!");
            return null;
        }
        return AbilitySettings[label] = new AbilitySetting(
            label, onActivated, suggestedKey, preventInteractionOnGui, hint
        );
    }

    private readonly Func<Player, string> _onActivated;

    protected AbilitySetting(string label, Func<Player, string> onActivated,
        KeyCode suggestedKey = KeyCode.None,
        bool preventInteractionOnGui = true, [CanBeNull] string hint = null)
        : base(null, label, suggestedKey, preventInteractionOnGui,
            false, hint)
    {
        _onActivated = onActivated;
    }

    protected override CustomSetting CreateDuplicate()
        => new AbilitySetting(Label, _onActivated, Base.SuggestedKey,
            Base.PreventInteractionOnGUI, DescriptionHint);

    protected override void HandleSettingUpdate()
    {
        if (LastUpdateType == SettingResponseType.Initial) return;
        if (KnownOwner == null) return;
        if (!IsPressed) return;

        var response = _onActivated(KnownOwner);
        if (response == null) return;

        var text = response == string.Empty
            ? "<color=green>Successfully activated ability.</color>"
            : $"<color=red>{response}</color>";

        RueDisplay.Get(KnownOwner).Show(
            CustomRoleLibPlugin.RueITag,
            new BasicElement(200, text)
            {
                ShowToSpectators = true
            }, TimeSpan.FromSeconds(5f)
        );
    }

    public override CustomHeader Header => SSSHelpers.DefaultAbilityHeader;
}