using CustomRoleLib.API;
using CustomRoleLib.API.DefaultComponents;
using LabApi.Features.Extensions;
using LabApi.Features.Wrappers;
using MapGeneration;
using PlayerRoles;
using SecretAPI.Extensions;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace CustomRoleLib;

public class RoomSpawnpointComponent<T>(RoomName roomName, Vector3? offset = null) : ComponentBase<T>
    where T : RoleInstanceBase
{
    public override void OnCreatedInstance(T instance)
    {
        base.OnCreatedInstance(instance);
        if (!Room.Get(roomName).TryGetRandomValue(out var room))
        {
            Logger.Warn($"Could not find room with name: {roomName} for CustomRole ({instance.Namespace}).");
            return;
        }

        instance.Owner.Position = room.Transform.TransformPoint(offset ?? Vector3.zero);
    }
}