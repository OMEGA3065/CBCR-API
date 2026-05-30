using CommandSystem;

namespace CustomRoleLib.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CustomRoleCommand : ParentCommand, IUsageProvider
    {
        public override string Command { get; } = "cbcr";

        public override string[] Aliases { get; } = new string[] { "cr" };

        public override string Description { get; } = "Gives a player a custom item.";

        public string[] Usage { get; } = new string[] { "%SubCommand%" };

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new CustomRoleGiveCommand());
            RegisterCommand(new CustomRoleListCommand());
        }

        public CustomRoleCommand()
        {
            LoadGeneratedCommands();
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(PlayerPermissions.ForceclassWithoutRestrictions))
            {
                response = "You don't have the required permission to use this command.";
                return false;
            }

            response = $"Use one of the following SubCommands: [\n{string.Join("\n", Commands.Values.Select(c => $"  - {c.Command}{(string.IsNullOrWhiteSpace(c.Description) ? "" : $" - {Description}")}{(c is IUsageProvider usageProvider ? $"\n   > Usage: {string.Join(", ", usageProvider.Usage)}" : "")}"))}\n]";
            return false;
        }
    }
}