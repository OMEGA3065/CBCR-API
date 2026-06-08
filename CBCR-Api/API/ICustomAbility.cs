using System.Diagnostics.CodeAnalysis;
using CustomRoleLib.API;
using LabApi.Features.Wrappers;

namespace CustomAbilityLib.API
{
    /// <summary>
    /// The base interface defining the base features of an Ability Definition. <see cref="CustomAbilityBase{T}"/>
    /// </summary>
    public interface ICustomAbility
    {
        /// <summary>
        /// The name of this Ability.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The description of this Ability.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The ID part of this Ability's namespace (ex. pluginNamespace:ID).
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The <see cref="ObjectNamespace"/> of this Ability.
        /// </summary>
        public ObjectNamespace Namespace { get; }

        /// <summary>
        /// The dictionary of <see cref="AbilityInstanceBase"/> handled by this Ability.
        /// </summary>
        public Dictionary<Player, AbilityInstanceBase> Instances { get; }

        /// <summary>
        /// Checks whether the specified <see cref="LabApi.Features.Wrappers.Player"/> has an instance of this Ability Definition.
        /// </summary>
        /// <param name="player">The <see cref="LabApi.Features.Wrappers.Player"/> to check.</param>
        /// <param name="roleInstance">The <see cref="AbilityInstanceBase"/> of this type the player is in the possession of, if any.</param>
        /// <returns>Whether the <see cref="LabApi.Features.Wrappers.Player"/> had an instance of this Ability Definition.</returns>
        public bool Check(Player player, [NotNullWhen(true)] out AbilityInstanceBase roleInstance);

        /// <summary>
        /// Checks whether the specified <see cref="LabApi.Features.Wrappers.Player"/> has an instance of this Ability Definition.
        /// </summary>
        /// <param name="player">The <see cref="LabApi.Features.Wrappers.Player"/> to check.</param>
        /// <returns>Whether the <see cref="LabApi.Features.Wrappers.Player"/> had an instance of this Ability Definition.</returns>
        public bool Check(Player player);

        /// <summary>
        /// Tries to give this Ability Definition's <see cref="AbilityInstanceBase"/> to a specified <see cref="LabApi.Features.Wrappers.Player"/>.
        /// </summary>
        /// <param name="player">The <see cref="LabApi.Features.Wrappers.Player"/> to which the role will be given.</param>
        /// <returns>Whether the role was given successfully.</returns>
        public bool TryGiveAbility(Player player);

        /// <summary>
        /// Used to initialize this Custom Ability Definition.
        /// </summary>
        public void Initialize();
    }
}