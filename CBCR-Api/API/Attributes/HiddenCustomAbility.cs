namespace CustomRoleLib.API.Attributes
{
    /// <summary>
    /// Used for hiding a <see cref="ICustomAbility{T}"/> from the automatic
    /// registration process (only if in use by the specified assembly)
    /// <seealso cref="CustomAbilityManager.RegisterAbilityRoles"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class HiddenCustomAbility : Attribute
    {
    }
}