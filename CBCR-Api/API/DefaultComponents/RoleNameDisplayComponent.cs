using CustomRoleLib.Helpers;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using Respawning.Objectives;

namespace CustomRoleLib.API.DefaultComponents;

/// <summary>
/// A component used for making the attached <see cref="CustomRoleBase{T}"/>'s name show up when hovering a player who has this role.
/// </summary>
public class RoleNameDisplayComponent : ComponentBase<RoleInstanceBase>
{
    public const PlayerInfoArea AllAreas = PlayerInfoArea.CustomInfo | PlayerInfoArea.UnitName |
                                           PlayerInfoArea.Nickname | PlayerInfoArea.PowerStatus | PlayerInfoArea.Role |
                                           PlayerInfoArea.Badge;

    public const PlayerInfoArea SelectAreas = PlayerInfoArea.CustomInfo | PlayerInfoArea.UnitName |
                                              PlayerInfoArea.PowerStatus | PlayerInfoArea.Badge;

    private const string RoleTypeTag = "CustomRoleTypeHint";

    public override void OnCreatedInstance(RoleInstanceBase instance)
    {
        base.OnCreatedInstance(instance);
        if (instance.Owner == null) return;

        var info = $"{instance.Owner.DisplayName}\n{instance.Parent.Name}";
        if (!Player.ValidateCustomInfo(info, out var reason))
            Logger.Error($"Custom Role ({instance.Namespace}) CustomInfo is not valid!: {reason}");
        instance.Owner.CustomInfo = info;
        instance.Owner.InfoArea = SelectAreas;

        if (!CustomRoleLibPlugin.RueIActive) return;
        var hint =
            $"<pos=1em><line-height=50%><size=50%><color={instance.Owner.Role.GetRoleColor().ToHex()}><align=left><pos=4em>Role:</align><br><align=left><pos=4em>{instance.Parent.Name}</align></color></size></line-height>";
        RueIHelpers.DisplayHint(instance.Owner, RoleTypeTag, hint, 25, null, showToSpectators: true);
    }

    public override void OnDestroyedInstance(RoleInstanceBase instance)
    {
        base.OnDestroyedInstance(instance);
        if (instance.Owner == null) return;
        instance.Owner.CustomInfo = "";
        instance.Owner.InfoArea = AllAreas;

        if (!CustomRoleLibPlugin.RueIActive) return;
        RueIHelpers.RemoveHint(instance.Owner, RoleTypeTag);
    }
}
