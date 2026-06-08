using CustomAbilityLib.API;
using LabApi.Features.Wrappers;

namespace CustomRoleLib.API
{
    /// <summary>
    /// The base class for all instances of any Item from any ItemDefinition <see cref="CustomRoleBase{T}"/>
    /// </summary>
    public abstract class AbilityInstanceBase
    {
        /// <summary>
        /// The parent to which this <see cref="RoleInstanceBase"/> belongs to.
        /// </summary>
        /// <value>The parent as a generic <see cref="ICustomRole{T}"/> object.</value>
        public ICustomAbility Parent { get; set; }

        /// <summary>
        /// The namespace of the parent to which this <see cref="RoleInstanceBase"/> belongs to.
        /// </summary>
        /// <value>The parent's <see cref="ObjectNamespace"/>.</value>
        public ObjectNamespace Namespace { get; set; }

        /// <summary>
        /// The <see cref="LabApi.Features.Wrappers.Player"/> this <see cref="RoleInstanceBase"/> belongs to.
        /// </summary>
        /// <value>The role owner.</value>
        public Player Owner { get; set; }

        /// <summary>
        /// The internal Custom Ability Instance ID for this object.
        /// </summary>
        /// <value>The Instance ID.</value>
        public ushort InstanceId { get; set; } = GetNextInstanceId();

        public abstract void Create(Player player);
        public abstract void Destroy();
        public abstract bool Execute(out string response);

        protected bool Equals(RoleInstanceBase other)
        {
            return Owner == other.Owner || InstanceId == other.InstanceId;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RoleInstanceBase)obj);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return InstanceId.GetHashCode();
        }

        private static ushort _lastInstanceId = 0;

        public static ushort GetNextInstanceId()
        {
            return ++_lastInstanceId;
        }

        public static void ResetInstanceCounter(ushort newCount = 0)
        {
            _lastInstanceId = newCount;
        }
    }
}