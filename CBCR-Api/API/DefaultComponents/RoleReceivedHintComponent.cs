using LabApi.Events.Arguments.PlayerEvents;

namespace CustomRoleLib.API.DefaultComponents;

/// <summary>
/// A component used for giving the attached <see cref="CustomRoleBase{T}"/> an on-screen hint for the player who is obtains the role.
/// </summary>
/// <typeparam name="T"><inheritdoc/></typeparam>
public class RoleReceivedHintComponent : ComponentBase<RoleInstanceBase>
{
    public override void OnCreatedInstance(RoleInstanceBase instance)
    {
        base.OnCreatedInstance(instance);
        instance.Owner?.SendHint($"You have:\n{instance.Parent.Name}");
    }
}
