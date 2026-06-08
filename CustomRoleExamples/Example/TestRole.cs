using CustomRoleLib.API;
using CustomRoleLib.API.Attributes;
using CustomRoleLib.API.DefaultComponents;
using CustomRoleLib.API.DefaultComponents.Generic;
using PlayerRoles;
using UnityEngine;

namespace CustomRoleExamples.Example
{
    [CustomRole(RoleTypeId.ClassD)]
    [CustomRoleAttributeBase(typeof(RoleReceivedHintComponent))]
    [CustomRoleAttributeBase(typeof(InitializerComponent<TestRoleInstance>))]
    public class TestRole : CustomRoleBase<TestRoleInstance>
    {
        public override string Name => "Custom Class-D";
        public override string Description => "Unknown";
        public override string Id => "custom_classd";
    }

    public class TestRoleInstance : RoleInstanceBase, IInitializable
    {
        public TestRoleInstance()
        {
            Health = UnityEngine.Random.Range(0f, 100f);
        }

        public void OnInitialized()
        {
            Owner.MaxHealth = Health;
            Owner.Health = Health;
        }

        public float Health { get; set; }

    }
}