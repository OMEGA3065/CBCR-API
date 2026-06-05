using CustomRoleLib.API;
using CustomRoleLib.API.Attributes;
using CustomRoleLib.API.DefaultComponents;
using CustomRoleLib.API.DefaultComponents.Generic;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace CustomRoleExamples.Example
{
    [CustomRole(RoleTypeId.Scientist)]
    [CustomRoleAttributeBase(typeof(RoleReceivedHintComponent<NaturalRoleInstance>))]
    [CustomRoleAttributeBase(typeof(InitializerComponent<NaturalRoleInstance>))]
    public class NaturalRole : CustomRoleBase<NaturalRoleInstance>
    {
        public override string Name => "Lost Scientist";
        public override string Description => "He got lost I guess...";
        public override string Id => "lost_scientist";

        public override bool NaturallySpawnable => true;

        public override float RoleSpawnWeight => 1f;
        public override float RoleNotSpawnWeight => 4f;

        public override RoleTypeId[] RoleSpawnOriginalRoleIds => [RoleTypeId.ClassD];
    }

    public class NaturalRoleInstance : RoleInstanceBase, IInitializable
    {
        public void OnInitialized()
        {
            Timing.CallDelayed(Timing.WaitForOneFrame, () =>
            {
                Owner.MaxHealth = 100f;
                Owner.Health = 70f;

                Owner.AddItem(ItemType.GunCOM15);
            }, Owner.GameObject);
        }
    }
}