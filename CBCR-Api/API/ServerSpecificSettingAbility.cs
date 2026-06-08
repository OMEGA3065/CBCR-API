using CustomAbilityLib.API;
using CustomRoleLib.ServerSpecificSettings;
using LabApi.Features.Wrappers;
using Mirror;
using SecretAPI.Features.UserSettings;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace CustomRoleLib.API;

public abstract class ServerSpecificSettingAbility<T> : CustomAbilityBase<T>
    where T : AbilityInstanceBase, new()
{
    private CustomKeybindSetting ServerSpecificSetting;

    protected virtual string SettingHintDescription => Description;
    protected virtual KeyCode SuggestedKey => KeyCode.None;
    protected virtual bool PreventInteractionsOnUI => true;
    protected virtual double Cooldown => 0;
    protected virtual int MaxUses => 0;

    protected readonly Dictionary<Player, double> Cooldowns = [];
    protected readonly Dictionary<Player, int> UseLimits = [];

    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();

        ServerSpecificSetting = AbilitySetting.RegisterNew(
            Name, SettingActivated, SuggestedKey,
            PreventInteractionsOnUI, SettingHintDescription
        );

        CustomSetting.Register(ServerSpecificSetting);
    }

    protected override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();

        if (ServerSpecificSetting == null) return;
        CustomSetting.UnRegister(ServerSpecificSetting);
    }

    protected override void DestroyInstance(Player owner, T abilityInstance)
    {
        Cooldowns.Remove(owner);
        UseLimits.Remove(owner);
        base.DestroyInstance(owner, abilityInstance);
    }

    protected virtual string SettingActivated(Player player)
    {
        if (!Check(player, out var instance))
            return $"You do not have this ability!";

        var lastUseCount = 0;
        if (MaxUses > 0 && UseLimits.TryGetValue(player, out lastUseCount)
                         && lastUseCount >= MaxUses)
            return $"Ability has already reached it's maximum number of uses ({lastUseCount}/{MaxUses}).";
        if (Cooldown > 0 && Cooldowns.TryGetValue(player, out var lastUse)
            && lastUse + Cooldown > NetworkTime.time)
            return $"Ability on cooldown for: {lastUse + Cooldown - NetworkTime.time:F0} seconds.";

        if (!instance.Execute(out var response))
            return response;

        if (MaxUses > 0) UseLimits[player] = ++lastUseCount;
        if (Cooldown > 0) Cooldowns[player] = NetworkTime.time + Cooldown;

        return null;
    }
}