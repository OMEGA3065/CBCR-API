using System.Reflection;
using LabApi.Events;

namespace CustomRoleLib.API.DefaultComponents;

/// <summary>
/// The base <see cref="ICustomRoleComponent{T}"/> object with default method implementation compared to the interface.
/// </summary>
/// <typeparam name="T"><inheritdoc/></typeparam>
public abstract class ComponentBase<T> : ICustomRoleComponent<T> where T : RoleInstanceBase
{
    protected readonly Dictionary<ushort, EventSubscriptionManager<T>> EventSubscriptions = [];

    private EventSubscriptionManager<T> GetOrCreateManager(T item)
    {
        if (EventSubscriptions.TryGetValue(item.InstanceId, out var manager))
            return manager;
        manager = new EventSubscriptionManager<T>(item);
        EventSubscriptions[item.InstanceId] = manager;
        return manager;
    }
    
    protected Action<TEvent> GetEvent<TEvent>(T instance,
        EventSubscriptionManager<T>.SubscriptionDelegate<TEvent> method, string name = null)
    {
        name ??= method.GetMethodInfo().Name;
        var manager = GetOrCreateManager(instance);
        return manager.GetOrCreate(name, method);
    }
    
    protected Action GetEvent(T instance,
        EventSubscriptionManager<T>.SubscriptionDelegate method, string name = null)
    {
        name ??= method.GetMethodInfo().Name;
        var manager = GetOrCreateManager(instance);
        return manager.GetOrCreate(name, method);
    }
    
    protected LabEventHandler<TEvent> GetLabEvent<TEvent>(T instance,
        EventSubscriptionManager<T>.SubscriptionDelegate<TEvent> method, string name = null)
        where TEvent : EventArgs
    {
        name ??= method.GetMethodInfo().Name;
        var manager = GetOrCreateManager(instance);
        return manager.GetOrCreateLab(name, method);
    }
    
    protected LabEventHandler GetLabEvent(T instance,
        EventSubscriptionManager<T>.SubscriptionDelegate method, string name = null)
    {
        name ??= method.GetMethodInfo().Name;
        var manager = GetOrCreateManager(instance);
        return manager.GetOrCreateLab(name, method);
    }
    
    /// <inheritdoc/>
    public virtual void DestroyComponent(ICustomRole<T> role) { }

    /// <inheritdoc/>
    public virtual void InitComponent(ICustomRole<T> role) { }

    /// <inheritdoc/>
    public virtual void OnCreatedInstance(T itemInstance) => SubscribeEvents(itemInstance);

    /// <inheritdoc/>
    public virtual bool OnCreatingInstance(T itemInstance) => true;

    /// <inheritdoc/>
    public virtual void OnDestroyedInstance(T itemInstance) => UnsubscribeEvents(itemInstance);

    /// <inheritdoc/>
    public virtual bool OnDestroyingInstance(T itemInstance) => true;

    /// <inheritdoc/>
    public virtual void SubscribeEvents(T itemInstance) { }

    /// <inheritdoc/>
    public virtual void UnsubscribeEvents(T itemInstance) { }
}
