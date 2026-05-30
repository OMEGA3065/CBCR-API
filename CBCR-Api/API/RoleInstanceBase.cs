using LabApi.Features.Wrappers;

namespace CustomRoleLib.API
{
    /// <summary>
    /// The base class for all instances of any Item from any ItemDefinition <see cref="CustomRoleBase{T}"/>
    /// </summary>
    public abstract class RoleInstanceBase
    {
        /// <summary>
        /// The parent to which this <see cref="RoleInstanceBase"/> belongs to.
        /// </summary>
        /// <value>The parent as a generic <see cref="ICustomRole{T}"/> object.</value>
        public ICustomRole<object> Parent { get; set; }

        /// <summary>
        /// The namespace of the parent to which this <see cref="RoleInstanceBase"/> belongs to.
        /// </summary>
        /// <value>The parent's <see cref="RoleNamespace"/>.</value>
        public RoleNamespace Namespace { get; set; }

        /// <summary>
        /// The <see cref="LabApi.Features.Wrappers.Player"/> this <see cref="RoleInstanceBase"/> belongs to.
        /// </summary>
        /// <value>The role owner.</value>
        public Player Owner { get; set; }

        /// <summary>
        /// A boolean value representing whether this role went through the initialization process.
        /// This process ends either immediately if the role doesn't require the owner to have a role,
        /// or it ends after their role was set to the configured role.
        /// </summary>
        /// <value>A value indicating whether this role instance has been initialized.</value>
        public bool Initialized { get; internal set; }

        /// <summary>
        /// The internal Custom Item Instance ID for this object.
        /// </summary>
        /// <value>The Instance ID.</value>
        public ushort InstanceId { get; set; } = GetNextInstanceId();

        /// <summary>
        /// Destroys this <see cref="RoleInstanceBase"/>.
        /// </summary>
        /// <param name="force">Whether to bypass any decision made by this <see cref="RoleInstanceBase"/>'s Item Definition's components.</param>
        public void Destroy(bool force)
        {
            if (!CustomRoleManager.TryGetRole(Namespace, out var customRole))
                return;
            customRole.TryDestroyInstance(this, force);
        }
        /// <summary>
        /// Checks whether a Player owner matches this <see cref="RoleInstanceBase"/>'s owner.
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public bool Check(Player owner)
        {
            return Owner == owner;
        }

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