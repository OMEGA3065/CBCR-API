namespace CustomRoleLib.API
{
    /// <summary>
    /// The base interface for all Role Components that can be applied to Custom Role Definitions.
    /// </summary>
    /// <typeparam name="T">An <see cref="RoleInstanceBase"/> managed by this component.</typeparam>
    public interface ICustomRoleComponent<in T>
    {
        /// <summary>
        /// Called each time this <see cref="ICustomRoleComponent{T}"/> is assigned to a <see cref="ICustomRole{T}"/>>.
        /// </summary>
        /// <param name="role">The Item Definition the component was attached to.</param>
        public void InitComponent(ICustomRole<T> role);

        /// <summary>
        /// Called each time this <see cref="ICustomRoleComponent{T}"/> is removed from a <see cref="ICustomRole{T}"/>>.
        /// </summary>
        /// <param name="role">The Item Definition the component was removed from.</param>
        public void DestroyComponent(ICustomRole<T> role);

        /// <summary>
        /// Called when a new instance of <see cref="RoleInstanceBase"/> for one of the Item Definitions that this component has been attached to is starting it's creation process.
        /// </summary>
        /// <param name="item">The <see cref="RoleInstanceBase"/> that's being created.</param>
        public bool OnCreatingInstance(T itemInstance);

        /// <summary>
        /// Called when a new instance of <see cref="RoleInstanceBase"/> for one of the Item Definitions that this component has been attached to finishes it's creation process.
        /// </summary>
        /// <param name="item">The <see cref="RoleInstanceBase"/> that has been created.</param>
        public void OnCreatedInstance(T instance);

        /// <summary>
        /// Called when a new instance of <see cref="RoleInstanceBase"/> for one of the Item Definitions that this component has been attached to is starting it's destruction process.
        /// </summary>
        /// <param name="item">The <see cref="RoleInstanceBase"/> that's being destroyed.</param>
        public bool OnDestroyingInstance(T itemInstance);

        /// <summary>
        /// Called when a new instance of <see cref="RoleInstanceBase"/> for one of the Item Definitions that this component has been attached to finishes it's destruction process.
        /// </summary>
        /// <param name="item">The <see cref="RoleInstanceBase"/> that has been destroyed.</param>
        public void OnDestroyedInstance(T itemInstance);

        /// <summary>
        /// Subscribes a specifed <see cref="RoleInstanceBase"/> to it's events.
        /// </summary>
        /// <param name="itemInstance">The <see cref="RoleInstanceBase"/> which will subscribe to it's events.</param>
        public void SubscribeEvents(T itemInstance);

        /// <summary>
        /// Unsubscribes a specifed <see cref="RoleInstanceBase"/> from it's events.
        /// </summary>
        /// <param name="itemInstance">The <see cref="RoleInstanceBase"/> which will unsubscribe from it's events.</param>
        public void UnsubscribeEvents(T itemInstance);
    }
}