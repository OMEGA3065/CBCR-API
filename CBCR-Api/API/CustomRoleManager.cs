using System.Reflection;
using CustomRoleLib.API.Attributes;
using HarmonyLib;
using LabApi.Features.Console;

namespace CustomRoleLib.API
{
    /// <summary>
    /// Manages namespaces and registration of items.
    /// </summary>
    public static class CustomRoleManager
    {
        /// <summary>
        /// Used for the <see cref="CustomItemRegistryChangeEvent"/> event.
        /// </summary>
        /// <param name="customRole">What item was registered / unregistered.</param>
        /// <param name="wasRegistered">Whether the item was registered (true) or unregistered (false).</param>
        public delegate void CustomItemRegistryChangeEventHandler(ICustomRole<object> customRole, bool wasRegistered);

        /// <summary>
        /// An event invoked each time a custom item is registered or unregistered.
        /// </summary>
        public static event CustomItemRegistryChangeEventHandler CustomItemRegistryChangeEvent;

        /// <summary>
        /// The list of all registered items.
        /// </summary>
        /// <returns>The list of all registered items according to their namespace.</returns>
        public static readonly Dictionary<RoleNamespace, ICustomRole<object>> Roles = new();

        /// <summary>
        /// Registers a Custom Item Definition.
        /// </summary>
        /// <param name="role">The custom item definition to register.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Whether the item was registered successfully.</returns>
        public static bool RegisterItem<T>(ICustomRole<T> role)
        {
            if (Roles.ContainsKey(role.Namespace))
                return false;
            if (role is not ICustomRole<object> typed)
                return false;
            typed.Initialize();
            Roles[role.Namespace] = typed;
            CustomItemRegistryChangeEvent?.Invoke(typed, true);
            return true;
        }

        /// <summary>
        /// Unregisters a Custom Item Definition.
        /// </summary>
        /// <param name="role">The custom item definition to unregister.</param>
        /// <returns>Whether the item was unregistered successfully.</returns>
        public static bool UnregisterItem(ICustomRole<object> role)
        {
            if (!Roles.Remove(role.Namespace)) return false;
            CustomItemRegistryChangeEvent?.Invoke(role, true);
            return true;
        }

        /// <summary>
        /// Used for obtaining an item based on a namespace.
        /// </summary>
        /// <param name="itemNamespace">The namespace of the target item.</param>
        /// <param name="role">The resulting item.</param>
        /// <returns>Whether the namespace has an item definition assigned to it.</returns>
        public static bool TryGetRole(RoleNamespace itemNamespace, out ICustomRole<object> role)
        {
            return Roles.TryGetValue(itemNamespace, out role);
        }

        /// <summary>
        /// Used for obtaining an item based on a namespace. Autocasts the result to a target <see cref="RoleInstanceBase"/>.
        /// </summary>
        /// <param name="itemNamespace">The namespace of the target item.</param>
        /// <param name="role">The resulting item.</param>
        /// <typeparam name="T">What <see cref="RoleInstanceBase"/> to cast to.</typeparam>
        /// <returns>Whether the namespace has an item definition assigned to it.</returns>
        public static bool TryGetRole<T>(RoleNamespace itemNamespace, out ICustomRole<T> role)
        {
            bool success = Roles.TryGetValue(itemNamespace, out var itemUncast);
            if (!success) { role = null; return false; }
            if (itemUncast is ICustomRole<T> castItem)
            {
                role = castItem;
                return true;
            }
            role = null;
            return false;
        }

        /// <summary>
        /// Registers all Item definitions in a specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search for item definitions.</param>
        /// <returns>An <see cref="IEnumerable{ICustomItem{object}}"/> of definitions that have been successfully registered.</returns>
        public static IEnumerable<ICustomRole<object>> RegisterAllRoles(Assembly assembly)
        {
            List<ICustomRole<object>> registeredItems = [];
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface || !typeof(ICustomRole<object>).IsAssignableFrom(type)
                    || type.GetCustomAttribute<HiddenCustomRole>() != null)
                    continue;
                try
                {
                    var item = (ICustomRole<object>)Activator.CreateInstance(type);
                    if (RegisterItem(item))
                    {
                        registeredItems.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Failed to create instance of {type}! Error: {ex}");
                }
            }
            return registeredItems;
        }

        /// <summary>
        /// Registers all Item definitions in a <see cref="Assembly.GetCallingAssembly"/> assembly.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ICustomItem{object}}"/> of definitions that have been successfully registered.</returns>
        public static IEnumerable<ICustomRole<object>> RegisterAllRoles()
        {
            return RegisterAllRoles(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Unregisters all Item definitions from a specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search for item definitions.</param>
        /// <returns>An <see cref="IEnumerable{ICustomItem{object}}"/> of definitions that have been successfully registered.</returns>
        public static IEnumerable<ICustomRole<object>> UnregisterAllItems(Assembly assembly)
        {
            IEnumerable<ICustomRole<object>> unregisteredItems = [];
            foreach (var item in Roles.Values.ToArray())
            {
                if (item.GetType().Assembly != assembly) continue;
                if (UnregisterItem(item))
                    unregisteredItems = unregisteredItems.AddItem(item);
            }
            return unregisteredItems;
        }

        /// <summary>
        /// Unregisters all Item definitions in a <see cref="Assembly.GetCallingAssembly"/> assembly.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ICustomItem{object}}"/> of definitions that have been successfully registered.</returns>
        public static IEnumerable<ICustomRole<object>> UnregisterAllItems()
        {
            return UnregisterAllItems(Assembly.GetCallingAssembly());
        }
    }
}