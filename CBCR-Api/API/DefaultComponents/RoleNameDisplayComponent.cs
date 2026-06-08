using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;

namespace CustomRoleLib.API.DefaultComponents;

/// <summary>
/// A component used for making the attached <see cref="CustomRoleBase{T}"/>'s name show up when hovering a player who has this role.
/// </summary>
public class RoleNameDisplayComponent : ComponentBase<RoleInstanceBase>
{
    public override void OnCreatedInstance(RoleInstanceBase instance)
    {
        base.OnCreatedInstance(instance);
        if (instance.Owner == null) return;

        var info = $"{instance.Owner.DisplayName}\n{instance.Parent.Name}";
        if (!Player.ValidateCustomInfo(info, out var reason))
            Logger.Error($"Custom Role ({instance.Namespace}) CustomInfo is not valid!: {reason}");
        instance.Owner.CustomInfo = info;
        instance.Owner.InfoArea = PlayerInfoArea.CustomInfo | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus | PlayerInfoArea.Badge;
    }
}
