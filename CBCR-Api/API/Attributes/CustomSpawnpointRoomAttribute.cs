using CustomRoleLib.API;
using CustomRoleLib.API.Attributes;
using MapGeneration;
using PlayerRoles;
using UnityEngine;

namespace CustomRoleLib;

public class CustomSpawnpointRoomAttribute<T>(RoomName roomName, float offsetX = 0, float offsetY = 0, float offsetZ = 0) : CustomRoleAttributeBase
    where T : RoleInstanceBase
{
    public override object Component { get; } = new RoomSpawnpointComponent<T>(roomName, new Vector3(offsetX, offsetY, offsetZ));
}