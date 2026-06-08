using CustomRoleLib.API;
using CustomRoleLib.API.Attributes;
using PlayerRoles;

namespace CustomRoleLib;

public class CustomSpawnpointRoleAttribute<T>(RoleTypeId roleSpawnpoint) : CustomRoleAttributeBase
    where T : RoleInstanceBase
{
    public override object Component { get; } = new RoleSpawnpointComponent<T>(roleSpawnpoint);
}