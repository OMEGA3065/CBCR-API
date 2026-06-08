using System.Reflection;
using CustomRoleLib.API;
using CustomRoleLib.API.Attributes;
using HarmonyLib;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;

namespace CustomAbilityLib.API
{
    /// <summary>
    /// Manages namespaces and registration of items.
    /// </summary>
    public static class CustomAbilityManager
    {
        /// <summary>
        /// Used for the <see cref="CustomAbilityManager.CustomAbilityRegistryChangeEvent"/> event.
        /// </summary>
        /// <param name="customAbility">What item was registered / unregistered.</param>
        /// <param name="wasRegistered">Whether the item was registered (true) or unregistered (false).</param>
        public delegate void CustomAbilityRegistryChangeEventHandler(ICustomAbility customAbility, bool wasRegistered);

        /// <summary>
        /// An event invoked each time a custom item is registered or unregistered.
        /// </summary>
        public static event CustomAbilityRegistryChangeEventHandler CustomAbilityRegistryChangeEvent;

        /// <summary>
        /// The list of all registered items.
        /// </summary>
        /// <returns>The list of all registered items according to their namespace.</returns>
        public static readonly Dictionary<ObjectNamespace, ICustomAbility> Abilities = [];

        /// <summary>
        /// Registers a Custom Item Definition.
        /// </summary>
        /// <param name="ability">The custom item definition to register.</param>
        /// <returns>Whether the item was registered successfully.</returns>
        public static bool RegisterAbility(ICustomAbility ability)
        {
            if (Abilities.ContainsKey(ability.Namespace))
                return false;
            ability.Initialize();
            Abilities[ability.Namespace] = ability;
            CustomAbilityRegistryChangeEvent?.Invoke(ability, true);
            return true;
        }

        /// <summary>
        /// Unregisters a Custom Item Definition.
        /// </summary>
        /// <param name="ability">The custom item definition to unregister.</param>
        /// <returns>Whether the item was unregistered successfully.</returns>
        public static bool UnregisterAbility(ICustomAbility ability)
        {
            if (!Abilities.Remove(ability.Namespace)) return false;
            CustomAbilityRegistryChangeEvent?.Invoke(ability, true);
            return true;
        }

        /// <summary>
        /// Used for obtaining an ability based on a namespace.
        /// </summary>
        /// <param name="abilityNamespace">The namespace of the target ability.</param>
        /// <param name="ability">The resulting ability.</param>
        /// <returns>Whether the namespace has an ability definition assigned to it.</returns>
        public static bool TryGetAbility(ObjectNamespace abilityNamespace, out ICustomAbility ability)
        {
            return Abilities.TryGetValue(abilityNamespace, out ability);
        }

        /// <summary>
        /// Used for obtaining an ability based on a generic type argument.
        /// </summary>
        /// <param name="ability">The resulting ability.</param>
        /// <typeparam name="T">The type of the target ability.</typeparam>
        /// <returns>Whether the namespace has an ability definition assigned to it.</returns>
        public static bool TryGetAbility<T>(out ICustomAbility ability)
        {
            ability = Abilities.Values.FirstOrDefault(a => a is T);
            return ability != null;
        }

        /// <summary>
        /// Used for giving an ability based on a generic type argument safely.
        /// </summary>
        /// <param name="player">The player to give the ability to.</param>
        /// <typeparam name="T">The type of the target ability.</typeparam>
        /// <returns>Whether the ability was given successfully.</returns>
        public static bool TryGiveAbility<T>(Player player)
        {
            var ability = Abilities.Values.FirstOrDefault(a => a is T);
            return ability?.TryGiveAbility(player) == true;
        }

        /// <summary>
        /// Registers all Item definitions in a specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search for item definitions.</param>
        /// <returns>An <see cref="IEnumerable{ICustomAbility}"/> of definitions that have been successfully registered.</returns>
        public static IEnumerable<ICustomAbility> RegisterAllAbilities(Assembly assembly)
        {
            List<ICustomAbility> registeredItems = [];
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface || !typeof(ICustomAbility).IsAssignableFrom(type)
                    || type.GetCustomAttribute<HiddenCustomAbility>() != null)
                    continue;
                try
                {
                    var item = (ICustomAbility)Activator.CreateInstance(type);
                    if (RegisterAbility(item))
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
        /// <returns>An <see cref="IEnumerable{ICustomAbility}"/> of definitions that have been successfully registered.</returns>
        public static IEnumerable<ICustomAbility> RegisterAllAbilities()
        {
            return RegisterAllAbilities(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Unregisters all Item definitions from a specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search for item definitions.</param>
        /// <returns>An <see cref="IEnumerable{ICustomAbility}"/> of definitions that have been successfully registered.</returns>
        public static IEnumerable<ICustomAbility> UnregisterAllAbilities(Assembly assembly)
        {
            IEnumerable<ICustomAbility> unregisteredItems = [];
            foreach (var item in Abilities.Values.ToArray())
            {
                if (item.GetType().Assembly != assembly) continue;
                if (UnregisterAbility(item))
                    unregisteredItems = unregisteredItems.AddItem(item);
            }
            return unregisteredItems;
        }

        /// <summary>
        /// Unregisters all Item definitions in a <see cref="Assembly.GetCallingAssembly"/> assembly.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ICustomAbility}"/> of definitions that have been successfully registered.</returns>
        public static IEnumerable<ICustomAbility> UnregisterAllAbilities()
        {
            return UnregisterAllAbilities(Assembly.GetCallingAssembly());
        }
    }
}