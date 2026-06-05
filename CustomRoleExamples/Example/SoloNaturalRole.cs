using CustomRoleLib.API;
using CustomRoleLib.API.Attributes;
using CustomRoleLib.API.DefaultComponents;
using CustomRoleLib.API.DefaultComponents.Generic;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace CustomRoleExamples.Example
{
    [CustomRole(RoleTypeId.ChaosConscript)]
    [CustomRoleAttributeBase(typeof(RoleReceivedHintComponent<SoloNaturalRoleInstance>))]
    [CustomRoleAttributeBase(typeof(InitializerComponent<SoloNaturalRoleInstance>))]
    public class SoloNaturalRoleRole : CustomRoleBase<SoloNaturalRoleInstance>
    {
        public override string Name => "Anomalous Scientist";
        public override string Description => "Scientist who has Hume Shield!";
        public override string Id => "anomalous_scientist";

        public override bool NaturallySpawnable => true;

        public SoloNaturalRoleRole()
        {
            CustomSpawnManager.SetGroupMaxTokens("anomalousVariants", 1);

            // This is unnecessary as it is the default.
            CustomSpawnManager.SetGroupTokenReset("anomalousVariants", CustomSpawnManager.TokenResetType.RoundRestart);
        }

        public override string RoleSpawnGroup => "anomalousVariants";
        public override float RoleSpawnWeight => 1f;
        public override float RoleNotSpawnWeight => 4f;

        public override RoleTypeId[] RoleSpawnOriginalRoleIds => [RoleTypeId.Scientist];
    }

    public class SoloNaturalRoleInstance : RoleInstanceBase, IInitializable
    {
        public void OnInitialized()
        {
            Timing.CallDelayed(Timing.WaitForOneFrame, () =>
            {
                Owner.HumeShield = 25f;
                Owner.HumeShieldRegenRate = 1f;
                Owner.MaxHumeShield = 50f;

                Owner.AddItem(ItemType.Adrenaline);
            }, Owner.GameObject);
        }
    }
}