using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CustomRoleLib.Helpers;
using CustomRoleLib.API.Attributes;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerRoles;
using Logger = LabApi.Features.Console.Logger;

namespace CustomRoleLib.API
{
    /// <summary>
    /// The base for most Custom Item Definitions.
    /// </summary>
    /// <typeparam name="T">An <see cref="RoleInstanceBase"/> used for each spawned item object.</typeparam>
    public abstract class CustomRoleBase<T> : ICustomRole<T> where T : RoleInstanceBase, new()
    {
        /// <inheritdoc/>
        public abstract string Name { get; }
        /// <inheritdoc/>
        public abstract string Description { get; }
        /// <inheritdoc/>
        public abstract string Id { get; }
        protected virtual string PluginNamespace => GetType().Assembly.GetName().Name.ToSnakeCase();
        /// <inheritdoc/>
        public virtual RoleNamespace Namespace => RoleNamespace.Get(PluginNamespace, Id);

        private bool IsInitialized { get; set; }

        /// <inheritdoc/>
        public virtual RoleTypeId? Type
        {
            get
            {
                if (field == RoleTypeId.Destroyed)
                {
                    var attr = GetType().GetCustomAttribute<CustomRoleAttribute>();
                    if (attr != null)
                    {
                        field = attr.RoleType;
                    }
                }
                return field;
            }
        } = RoleTypeId.Destroyed;

        /// <summary>
        /// The list of attached <see cref="ICustomRoleComponent{T}"/>
        /// </summary>
        public readonly List<ICustomRoleComponent<T>> ComponentAttributes;

        /// <summary>
        /// Initializes a new instance of an Item Definition.
        /// </summary>
        public CustomRoleBase()
        {
            ComponentAttributes = GetType().GetCustomAttributes<CustomRoleAttributeBase>().Select(a =>
            {
                if (a.Component is not ICustomRoleComponent<T> component)
                {
                    Logger.Error($"Failed to cast component of type {a.GetType()} to {typeof(ICustomRoleComponent<T>)}.{(a.GetType().GetGenericArguments() != typeof(ICustomRoleComponent<T>).GetGenericArguments() ? " Please check that your ICustomItemComponent<ItemInstanceBase> the ItemInstanceBase matches the one used for this item." : "")} This Component will not be added to the item and will be skipped!");
                    return null;
                }
                return component;
            }).Where(a => a != null).ToList();
        }

        public virtual void Initialize()
        {
            if (IsInitialized) return;

            SubscribeEvents();
            ComponentAttributes.ForEach(c => c.InitComponent(this));
            IsInitialized = true;
        }

        ~CustomRoleBase()
        {
            if (!IsInitialized) return;

            ComponentAttributes.ForEach(c => c.DestroyComponent(this));
            UnsubscribeEvents();
            IsInitialized = false;
        }

        /// <inheritdoc/>
        public Dictionary<Player, RoleInstanceBase> Instances { get; } = [];

        private T CreateInstance(Player owner)
        {
            var typed = new T();
            if (ComponentAttributes.Select(c => c.OnCreatingInstance(typed)).Any(b => !b))
            {
                return null;
            }

            if (Instances.TryGetValue(owner, out var oldRole))
                oldRole.Destroy(true);
            Instances[owner] = typed;

            typed.Parent = this;
            typed.Owner = owner;

            if (Type == null)
            {
                typed.Initialized = true;
                ComponentAttributes.ForEach(c => c.OnCreatedInstance(typed));
            }

            return typed;
        }

        public bool TryDestroyInstance(RoleInstanceBase roleInstanceU, bool force = false)
        {
            if (roleInstanceU is not T roleInstance) return true;
            var results = ComponentAttributes.Select(c => c.OnDestroyingInstance(roleInstance));
            if (!force && results.Any(b => !b))
            {
                return false;
            }

            if (Instances.TryGetValue(roleInstance.Owner, out var oldRole)
                && oldRole.InstanceId == roleInstance.InstanceId)
            {
                Instances.Remove(roleInstance.Owner);
            }

            ComponentAttributes.ForEach(c => c.OnDestroyedInstance(roleInstance));
            return true;
        }

        /// <summary>
        /// Tries to give this Role Definition's <see cref="RoleInstanceBase"/> to a specified <see cref="LabApi.Features.Wrappers.Player"/>.
        /// </summary>
        /// <param name="player">The <see cref="LabApi.Features.Wrappers.Player"/> to which the role will be given.</param>
        /// <param name="roleInstance">The created <see cref="RoleInstanceBase"/>.</param>
        /// <returns>Whether the item was gives successfully.</returns>
        public bool TryGiveRole(Player player, out T roleInstance)
        {
            roleInstance = CreateInstance(player);
            if (roleInstance == null) return false;
            roleInstance.Namespace = Namespace;
            if (Type != null)
                player.SetRole(Type.Value, RoleChangeReason.RemoteAdmin, RoleSpawnFlags.None);
            return true;
        }

        /// <inheritdoc/>
        public virtual bool Check(Player player, [NotNullWhen(true)] out RoleInstanceBase roleInstance)
        {
            return Instances.TryGetValue(player, out roleInstance);
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
            LabApi.Events.Handlers.PlayerEvents.ChangedRole += OnOwnerChangedRole;
        }

        /// <summary>
        /// Unsubscribes this Item Definition from some events. Should never have to be used.
        /// </summary>
        protected virtual void UnsubscribeEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.ChangedRole -= OnOwnerChangedRole;
        }

        /// <summary>
        /// Handles an event so that an <see cref="RoleInstanceBase"/> isn't lost.
        /// </summary>
        private void OnOwnerChangedRole(PlayerChangedRoleEventArgs ev)
        {
            if (!Check(ev.Player, out var roleInstanceU) || roleInstanceU is not T roleInstance) return;
            if (ev.ChangeReason is RoleChangeReason.Destroyed)
            {
                OnOwnerDestroyed(ev, roleInstance);
                return;
            }

            if (ev.Player.Role == Type && !roleInstance.Initialized)
            {
                roleInstance.Initialized = true;
                ComponentAttributes.ForEach(c => c.OnCreatedInstance(roleInstance));
                return;
            }

            if (ShouldKeepRole(ev.Player, ev.OldRole, ev.Player.Role))
                return;

            roleInstance.Destroy(true);
        }

        protected virtual void OnOwnerDestroyed(PlayerChangedRoleEventArgs ev, RoleInstanceBase roleInstance)
        {
            roleInstance.Destroy(true);
        }

        protected virtual bool ShouldKeepRole(Player player, RoleTypeId oldRole, RoleTypeId newRole)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool TryGiveRole(Player player)
        {
            return TryGiveRole(player, out _);
        }
    }
}
