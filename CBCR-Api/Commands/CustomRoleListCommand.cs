using CommandSystem;
using CustomRoleLib.API;

namespace CustomRoleLib.Commands
{
    [CommandHandler(typeof(CustomRoleCommand))]
    public class CustomRoleListCommand : ICommand, IUsageProvider
    {
        private const string PrintAllItemsFlag = "--all";
        public string Command { get; } = "list";
        public string[] Aliases { get; } = ["l"];
        public string Description { get; } = "Lists the available custom items";
        public string[] Usage { get; } = ["[PluginNamespace]"];

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(PlayerPermissions.ForceclassWithoutRestrictions))
            {
                response = "You don't have the required permission to use this command.";
                return false;
            }

            if (arguments.Count == 1)
            {
                string targetPluginSpace = arguments.First();
                if (targetPluginSpace == PrintAllItemsFlag)
                {
                    var list3 = CustomRoleManager.Roles.Select(kvp => $"{kvp.Key} => {kvp.Value.Name}").ToList();
                    response = $"{CustomRoleManager.Roles.Count} roles were found in all namespaces\nList of items: [\n{(list3.Count != 0 ? string.Join('\n', list3) : "")}\n]";
                    return true;
                }
                List<(string @namespace, string roleName)> roles = [];
                foreach (var (@namespace, role) in CustomRoleManager.Roles)
                {
                    if (@namespace.PluginNamespace == targetPluginSpace)
                    {
                        roles.Add((@namespace.ToString(), role.Name));
                    }
                }
                if (roles.Count == 0)
                {
                    var list2 = CustomRoleManager.Roles.Keys.Select(ns => ns.PluginNamespace).Distinct().ToList();
                    response = $"No roles could be found in {targetPluginSpace}:*\nList of namespaces registered: [\n{(list2.Count != 0 ? string.Join('\n', list2) : "")}\n]";
                    return false;
                }
                response = $"{roles.Count} roles were found in {targetPluginSpace}:*\nList of roles: [\n{string.Join('\n', roles.Select(tuple => $"{tuple.@namespace} => {tuple.roleName}"))}\n]";
                return true;
            }
            var list = CustomRoleManager.Roles.Keys.Select(ns => ns.PluginNamespace).Distinct().ToList();
            response = $"{list.Count} Plugin Namespaces have been registered.\nUse cbcr list <Namespace> for items in one of the namespaces found below. Or use cbcr list {PrintAllItemsFlag} to print all roles.\nList of Plugin Namespaces: [\n{(list.Count != 0 ? string.Join('\n', list) : "")}\n]";
            return true;
        }
    }
}