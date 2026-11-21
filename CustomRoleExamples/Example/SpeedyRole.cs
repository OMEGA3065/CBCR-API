using CustomAbilityLib.API;
using CustomRoleLib.API;
using CustomRoleLib.API.Attributes;
using CustomRoleLib.API.DefaultComponents;
using CustomRoleLib.API.DefaultComponents.Generic;
using PlayerRoles;

namespace CustomRoleExamples.Example
{
    [CustomRole(RoleTypeId.NtfPrivate)]
    [CustomRoleAttributeBase(typeof(RoleIntroComponent))]
    [CustomRoleAttributeBase(typeof(RoleNameDisplayComponent))]
    [CustomRoleAttributeBase(typeof(InitializerComponent<SpeedyRoleInstance>))]
    public class SpeedyRole : CustomRoleBase<SpeedyRoleInstance>
    {
        public override string Name => "Speedy boy!";
        public override string Description => "I am speed!";
        public override string Id => "speedy_role";
    }

    public class SpeedyRoleInstance : RoleInstanceBase, IInitializable
    {
        public void OnInitialized()
        {
            CustomAbilityManager.TryGiveAbility<SpeedBoostAbility>(Owner);
        }
    }
}