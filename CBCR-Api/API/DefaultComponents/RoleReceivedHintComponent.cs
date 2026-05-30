using LabApi.Events.Arguments.PlayerEvents;

namespace CustomRoleLib.API.DefaultComponents;

/// <summary>
/// A component used for giving the attached <see cref="CustomRoleBase{T}"/> an on-screen hint for the player who is obtains the role.
/// </summary>
/// <typeparam name="T"><inheritdoc/></typeparam>
public class RoleReceivedHintComponent<T> : ComponentBase<T>
    where T : RoleInstanceBase
{
    /// <inheritdoc/>
    public override void SubscribeEvents(T itemInstance)
    {
        base.SubscribeEvents(itemInstance);
        LabApi.Events.Handlers.PlayerEvents.ChangedRole += GetLabEvent<PlayerChangedRoleEventArgs>(itemInstance, OnOwnerChangedRole, "ownerChangedRole");
    }

    /// <inheritdoc/>
    public override void UnsubscribeEvents(T itemInstance)
    {
        base.UnsubscribeEvents(itemInstance);
        LabApi.Events.Handlers.PlayerEvents.ChangedRole -= GetLabEvent<PlayerChangedRoleEventArgs>(itemInstance, OnOwnerChangedRole, "ownerChangedRole");
    }

    protected void OnOwnerChangedRole(PlayerChangedRoleEventArgs ev, T itemInstance)
    {
        if (!itemInstance.Check(ev.Player)) return;
        ev.Player.SendHint($"You have:\n{itemInstance.Parent.Name}");
    }
}
