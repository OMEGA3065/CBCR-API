using System.Diagnostics.CodeAnalysis;
using LabApi.Features.Wrappers;
using PlayerRoles;

namespace CustomRoleLib.API
{
    /// <summary>
    /// The base interface defining the base features of an Item Definition. <see cref="CustomRoleBase{T}"/>
    /// </summary>
    /// <typeparam name="T">The <see cref="RoleInstanceBase"/> to use for this Item Definition.</typeparam>
    public interface ICustomRole<out T>
    {
        /// <summary>
        /// The name of this Role.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The description of this Role.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The ID part of this Role's namespace (ex. pluginNamespace:ID).
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The <see cref="RoleNamespace"/> of this Role.
        /// </summary>
        public RoleNamespace Namespace { get; }

        /// <summary>
        /// The <see cref="RoleTypeId"/> of this Role.
        /// </summary>
        /// <remarks>Uses null when the role doesn't affect the visible role.</remarks>
        public RoleTypeId? Type { get; }

        /// <summary>
        /// Dictates whether the role can spawn naturally during some role changes.
        /// </summary>
        public bool NaturallySpawnable { get; }

        /// <summary>
        /// How likely is the role to spawn on a player.
        /// </summary>
        public float RoleSpawnWeight { get; }

        /// <summary>
        /// How likely is the role to NOT spawn on a player.
        /// </summary>
        public float RoleNotSpawnWeight { get; }

        /// <summary>
        /// The group this role belongs to.
        /// </summary>
        public string RoleSpawnGroup => Namespace.ToString();

        /// <summary>
        /// Role will only be spawned if the reason for the spawn is in this array.
        /// </summary>
        public RoleTypeId[] RoleSpawnOriginalRoleIds => Type.HasValue ? [Type.Value] : [];

        /// <summary>
        /// The dictionary of <see cref="RoleInstanceBase"/> handled by this Role.
        /// </summary>
        public Dictionary<Player, RoleInstanceBase> Instances { get; }

        /// <summary>
        /// Tries to destroy the specified <see cref="RoleInstanceBase"/>.
        /// </summary>
        /// <param name="roleInstance">The <see cref="RoleInstanceBase"/> to destroy.</param>
        /// <param name="force">Forces the destruction of the role instance bypassing any decisions made by its components.</param>
        /// <returns>Whether the <see cref="RoleInstanceBase"/> has been destroyed successfully.</returns>
        public bool TryDestroyInstance(RoleInstanceBase roleInstance, bool force = false);

        /// <summary>
        /// Checks whether the specified <see cref="LabApi.Features.Wrappers.Player"/> has an instance of this Role Definition.
        /// </summary>
        /// <param name="player">The <see cref="LabApi.Features.Wrappers.Player"/> to check.</param>
        /// <param name="roleInstance">The <see cref="RoleInstanceBase"/> of this type the player is in the possession of, if any.</param>
        /// <returns>Whether the <see cref="LabApi.Features.Wrappers.Player"/> had an instance of this Role Definition.</returns>
        public bool Check(Player player, [NotNullWhen(true)] out RoleInstanceBase roleInstance);

        /// <summary>
        /// Checks whether the specified <see cref="LabApi.Features.Wrappers.Player"/> has an instance of this Role Definition.
        /// </summary>
        /// <param name="player">The <see cref="LabApi.Features.Wrappers.Player"/> to check.</param>
        /// <returns>Whether the <see cref="LabApi.Features.Wrappers.Player"/> had an instance of this Role Definition.</returns>
        public bool Check(Player player);

        /// <summary>
        /// Tries to give this Role Definition's <see cref="RoleInstanceBase"/> to a specified <see cref="LabApi.Features.Wrappers.Player"/>.
        /// </summary>
        /// <param name="player">The <see cref="LabApi.Features.Wrappers.Player"/> to which the role will be given.</param>
        /// <param name="skipManualRoleChange">
        /// Will skip <see cref="Player.SetRole"/> in favor if it being handled externally if set to true.
        /// Will ALWAYS assume that the RoleChangedEvent will be fired
        /// with the appropriate <see cref="RoleChangeReason"/> afterwards.
        /// </param>
        /// <returns>Whether the role was given successfully.</returns>
        public bool TryGiveRole(Player player, bool skipManualRoleChange = false);

        /// <summary>
        /// Used to initialize this Custom Role Definition.
        /// </summary>
        public void Initialize();
    }
}