using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CustomAbilityLib.API;
using CustomRoleLib.API;
using CustomRoleLib.Helpers;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace CustomAbilityLib.API
{
    /// <summary>
    /// The base for most Custom Item Definitions.
    /// </summary>
    /// <typeparam name="T">An <see cref="AbilityInstanceBase"/> used for each spawned item object.</typeparam>
    public abstract class CustomAbilityBase<T> : ICustomAbility where T : AbilityInstanceBase, new()
    {
        /// <inheritdoc/>
        public abstract string Name { get; }
        /// <inheritdoc/>
        public abstract string Description { get; }
        /// <inheritdoc/>
        public abstract string Id { get; }
        protected virtual string PluginNamespace => GetType().Assembly.GetName().Name.ToSnakeCase();
        /// <inheritdoc/>
        public virtual ObjectNamespace Namespace => ObjectNamespace.Get(PluginNamespace, Id);

        public bool IsInitialized { get; private set; } = false;

        public virtual void Initialize()
        {
            if (IsInitialized) return;

            SubscribeEvents();
            IsInitialized = true;
        }

        ~CustomAbilityBase()
        {
            if (!IsInitialized) return;

            UnsubscribeEvents();
            IsInitialized = false;
        }

        /// <inheritdoc/>
        public Dictionary<Player, AbilityInstanceBase> Instances { get; } = [];

        private T CreateInstance(Player owner)
        {
            var typed = new T();

            Instances[owner] = typed;

            typed.Parent = this;
            typed.Owner = owner;

            return typed;
        }

        /// <summary>
        /// Tries to give this Ability Definition's <see cref="AbilityInstanceBase"/> to a specified <see cref="LabApi.Features.Wrappers.Player"/>.
        /// </summary>
        /// <param name="player">The <see cref="LabApi.Features.Wrappers.Player"/> to which the ability will be given.</param>
        /// <param name="abilityInstance">The created <see cref="AbilityInstanceBase"/>.</param>
        /// <returns>Whether the item was gives successfully.</returns>
        public virtual bool TryGiveAbility(Player player, out T abilityInstance)
        {
            abilityInstance = CreateInstance(player);
            if (abilityInstance == null) return false;
            abilityInstance.Namespace = Namespace;
            abilityInstance.Create(player);
            return true;
        }

        /// <inheritdoc/>
        public virtual bool Check(Player player, [NotNullWhen(true)] out AbilityInstanceBase abilityInstance)
        {
            return Instances.TryGetValue(player, out abilityInstance);
        }

        /// <inheritdoc/>
        public virtual bool Check(Player owner)
        {
            return Instances.ContainsKey(owner);
        }


        /// <summary>
        /// Subscribes this Item Definition to some events. Should never have to be used.
        /// </summary>
        protected virtual void SubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.ChangingRole += OnOwnerChangingRole;
        }

        /// <summary>
        /// Unsubscribes this Item Definition from some events. Should never have to be used.
        /// </summary>
        protected virtual void UnsubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.ChangingRole -= OnOwnerChangingRole;
        }

        protected virtual void DestroyInstance(Player owner, T abilityInstance)
        {
            abilityInstance.Destroy();
            Instances.Remove(owner);
        }

        /// <summary>
        /// Handles an event so that an <see cref="AbilityInstanceBase"/> isn't lost.
        /// </summary>
        protected virtual void OnOwnerChangingRole(PlayerChangingRoleEventArgs ev)
        {
            if (!ev.IsAllowed) return;
            if (!Check(ev.Player, out var abilityInstanceU) || abilityInstanceU is not T abilityInstance) return;
            DestroyInstance(ev.Player, abilityInstance);
        }

        /// <inheritdoc/>
        public bool TryGiveAbility(Player player)
        {
            return TryGiveAbility(player, out _);
        }
    }
}
