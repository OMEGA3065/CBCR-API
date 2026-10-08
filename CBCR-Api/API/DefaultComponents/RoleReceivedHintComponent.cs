using CustomRoleLib.Helpers;

namespace CustomRoleLib.API.DefaultComponents;

/// <summary>
/// A component used for giving the attached <see cref="CustomRoleBase{T}"/> an on-screen hint for the player who is obtains the role.
/// </summary>
public class RoleReceivedHintComponent : ComponentBase<RoleInstanceBase>
{
    private const float HintTimeSeconds = 12f;

    public override void OnCreatedInstance(RoleInstanceBase instance)
    {
        base.OnCreatedInstance(instance);

        var hint = $"You have:\n{instance.Parent.Name}";

        if (!CustomRoleLibPlugin.RueIActive)
        {
            instance.Owner.SendHint(hint, HintTimeSeconds);
            return;
        }

        RueIHelpers.DisplayHint(instance.Owner, CustomRoleLibPlugin.RueITag, hint, 200f, HintTimeSeconds, showToSpectators: true);
    }
}
