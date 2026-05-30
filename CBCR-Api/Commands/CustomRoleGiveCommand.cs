using CommandSystem;
using CustomRoleLib.API;
using LabApi.Features.Wrappers;
using Utils;

namespace CustomRoleLib.Commands
{
    [CommandHandler(typeof(CustomRoleCommand))]
    public class CustomRoleGiveCommand : ICommand, IUsageProvider
    {
        public string Command { get; } = "give";
        public string[] Aliases { get; } = new string[] { "g" };
        public string Description { get; } = "Gives a player a custom item.";
        public string[] Usage { get; } = new string[] { "%RoleNamespace%", "%Players%" };

        private bool TryGetRAPlayerIds(ArraySegment<string> arguments, int startIndex, out List<ReferenceHub> hubs)
        {
            hubs = RAUtils.ProcessPlayerIdOrNamesList(arguments, startIndex, out _);
            return hubs != null;
        }

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(PlayerPermissions.ForceclassWithoutRestrictions))
            {
                response = "You don't have the required permission to use this command.";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = $"You need to provide at least 1 argument. Usage: cr give {string.Join(" ", Usage)}";
                return false;
            }

            List<Player> targetPlayers = [];
            if (Player.TryGet(sender, out var runningPlayer) && arguments.Count == 1)
            {
                targetPlayers.Add(runningPlayer);
            }
            else if (arguments.Count == 2 && TryGetRAPlayerIds(arguments, 1, out var hubs))
            {
                targetPlayers.AddRange(hubs.Select(h => Player.Get(h)));
            }
            else
            {
                response = "You must either be a player to use this command or specify valid players to give the role to!";
                return false;
            }

            if (!RoleNamespace.TryGet(arguments.At(0), out var roleNamespace))
            {
                response = "Invalid role namespace format. Expected format: 'plugin_namespace:role_identifier'";
                return false;
            }

            if (!CustomRoleManager.TryGetRole(roleNamespace, out var role))
            {
                response = $"Could not find an role under the namespace of {roleNamespace}. Please make sure that item exists.";
                return false;
            }

            int count = 0;
            foreach (var player in targetPlayers)
            {
                if (role.TryGiveRole(player))
                    count++;
            }

            response = $"Role \"{role.Name}\"[{roleNamespace}] was given to {count} players.";
            return true;
        }
    }
}