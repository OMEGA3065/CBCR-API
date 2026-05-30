using PlayerRoles;

namespace CustomRoleLib.API.Attributes
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomRoleAttribute"/> class.
    /// </summary>
    /// <param name="type">The <see cref="RoleTypeId"/> to serialize.</param>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class CustomRoleAttribute(RoleTypeId type) : Attribute
    {
        /// <summary>
        /// Gets the attribute's <see cref="RoleTypeId"/>.
        /// </summary>
        public RoleTypeId RoleType { get; } = type;
    }
}