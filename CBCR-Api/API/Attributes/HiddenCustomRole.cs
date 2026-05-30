namespace CustomRoleLib.API.Attributes
{
    /// <summary>
    /// Used for hiding a <see cref="ICustomRole{T}"/> from the automatic
    /// registration process (only if in use by the specified assembly)
    /// <seealso cref="CustomRoleManager.RegisterAllRoles"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class HiddenCustomRole : Attribute
    {
    }
}