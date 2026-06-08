using CustomRoleLib.API;
using CustomRoleLib.API.DefaultComponents;
using LabApi.Features.Extensions;
using PlayerRoles;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace CustomRoleLib;

public class RoleSpawnpointComponent<T>(RoleTypeId role) : ComponentBase<T>
    where T : RoleInstanceBase
{
    public override void OnCreatedInstance(T instance)
    {
        base.OnCreatedInstance(instance);
        if (!role.TryGetRandomSpawnPoint(out var pos, out var rot))
        {
            Logger.Warn($"Could not find spawnpoint for role: {role} for CustomRole ({instance.Namespace}).");
            return;
        }

        instance.Owner.Position = pos;
        instance.Owner.LookRotation = new Vector2(0f, rot);
    }
}