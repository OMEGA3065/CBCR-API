namespace CustomRoleLib.API.Attributes
{
    /// <summary>
    /// Used for adding custom / built-in components to role definitions.
    /// Initializes a new instance of the <see cref="CustomRoleAttributeBase"/> class.
    /// </summary>
    /// <param name="type">The component type to add.</param>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class CustomRoleAttributeBase(Type type) : Attribute
    {
        /// <summary>
        /// Gets the attribute's <see cref="ICustomRoleComponent{T}"/>.
        /// </summary>
        public object Component { get; } = Activator.CreateInstance(type);
    }
}