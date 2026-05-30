using CustomRoleLib.API;
using LabApi.Features.Wrappers;
using NorthwoodLib.Pools;

namespace CustomRoleLib.Compat.RaCustomMenuCompatability;

/// <summary>
/// Provides the RaCustomMenu functionality allowing easier giving of items.
/// </summary>
public class RoleNamespaceProvider : RaCustomMenu.API.Provider
{
    public override string CategoryName => "CBCR Give Role";
    public override bool IsDirty { get; } = true;

    private static readonly List<string> NamespacesRegistered = [];

    private static void RegisterNameSpace(string pluginNamespace)
    {
        if (providersLoaded.Any(p => p.CategoryName == pluginNamespace))
            return;

        RegisterDynamicProvider($"{pluginNamespace}", true, referenceHub =>
        {
            List<RaCustomMenu.API.LimitedDummyAction> list = [new("<color=red>[CLOSE]</color>", (sender) =>
            {
                if (!sender.CheckPermission(PlayerPermissions.ForceclassWithoutRestrictions)) return;
                UnregisterDynamicProvider($"{pluginNamespace}");
            })];
            foreach (var cr in CustomRoleManager.Roles)
            {
                if (cr.Key.PluginNamespace != pluginNamespace)
                    continue;

                list.Add(new(cr.Key.RoleIdentifier, (sender) =>
                {
                    if (!sender.CheckPermission(PlayerPermissions.GivingItems)) return;
                    if (!CustomRoleManager.TryGetRole(cr.Key, out var item))
                    {
                        Player.Get(sender)!.SendConsoleMessage($"Could not find a role under the namespace of {cr.Key}. Please make sure that item exists.");
                        return;
                    }
                    if (item.TryGiveRole(Player.Get(referenceHub)))
                        Player.Get(sender)!.SendConsoleMessage($"Role ({cr.Key}) has been given to a player!");
                }));
            }
            return list;
        });
    }

    public override List<RaCustomMenu.API.LimitedDummyAction> AddActions(ReferenceHub hub)
    {
        List<RaCustomMenu.API.LimitedDummyAction> list = [];
        var foundNamespaces = HashSetPool<string>.Shared.Rent();
        foreach (var item in CustomRoleManager.Roles)
        {
            if (foundNamespaces.Contains(item.Key.PluginNamespace))
                continue;

            foundNamespaces.Add(item.Key.PluginNamespace);

            list.Add(
                new($"{item.Key.PluginNamespace}", (sender) =>
                {
                    if (!sender.CheckPermission(PlayerPermissions.ForceclassWithoutRestrictions)) return;
                    RegisterNameSpace(item.Key.PluginNamespace);
                })
            );
        }
        return list;
    }
}