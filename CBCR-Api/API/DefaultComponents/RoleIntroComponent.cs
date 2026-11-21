using CustomRoleLib.DisplayKit;
using CustomRoleLib.Helpers;
using DisplayKit.Elements;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using MEC;
using Mirror;
using Respawning.Objectives;

namespace CustomRoleLib.API.DefaultComponents;

/// <summary>
/// A component used for giving the attached <see cref="CustomRoleBase{T}"/> an custom intro screen once obtained.
/// </summary>
public class RoleIntroComponent : ComponentBase<RoleInstanceBase>
{
    private const float DisplayTimeSeconds = 7.75f;

    private static readonly Dictionary<Player, CanvasHandler<RoleIntroScreen>> SavedReferences = [];

    internal static void Init()
    {
        ServerEvents.WaitingForPlayers += OnWaitingForPlayers;
        PlayerEvents.Left += OnPlayerLeft;
    }

    private static void OnPlayerLeft(PlayerLeftEventArgs ev)
    {
        SavedReferences.Remove(ev.Player);
    }

    private static void OnWaitingForPlayers()
    {
        SavedReferences.Clear();
    }

    public override void InitComponent(ICustomRole<RoleInstanceBase> role)
    {
        base.InitComponent(role);
        role.SkipIntro = true;
    }

    public override void OnCreatedInstance(RoleInstanceBase instance)
    {
        base.OnCreatedInstance(instance);
        if (!SavedReferences.TryGetValue(instance.Owner, out var handler))
            SavedReferences[instance.Owner] = handler = new CanvasHandler<RoleIntroScreen>(new RoleIntroScreen(instance.Owner));

        var reference = handler.Reference;
        handler.Effect(() =>
        {
            reference.Role = instance.Parent.Name.ToUpper();
            reference.RoleColor = instance.Parent.Color ?? instance.Owner.Role.GetRoleColor();
            reference.Description = instance.Parent.Description;
            reference.LastOwner = instance;
            reference.Canvas.Show();

            Timing.CallDelayed(DisplayTimeSeconds, () =>
            {
                if (!ReferenceEquals(reference.LastOwner, instance))
                    return;

                reference.Canvas.Hide();
            }, instance.Owner.GameObject);
        });
    }
}
