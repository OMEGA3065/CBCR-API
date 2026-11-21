using CustomRoleLib.API;
using CustomRoleLib.API.DefaultComponents;
using HarmonyLib;
using LabApi.Events.CustomHandlers;
using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;

namespace CustomRoleLib
{
    public class CustomRoleLibPlugin : Plugin<Config>
    {
        public static CustomRoleLibPlugin Instance { get; private set; }
        public override string Name => "Component Based Custom Role API";
        public override string Description => "An API for creating and managing custom roles.";

        public override string Author => "OMEGA3065";

        public override Version Version => GetType().Assembly.GetName().Version;
        public override LoadPriority Priority => LoadPriority.Highest;
        public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

        public const string RueITag = "CBCR-TAG";

        public CustomRoleLibPlugin()
        {
            Instance = this;
        }

        private Harmony _harmony;
        private EventHandlers _handlers = new();

        public static bool RueIActive { get; private set; }

        public override void Enable()
        {
            _harmony = new Harmony("omega3065.custom_role_lib");
            _harmony.PatchAll();

#if IsRaCustomMenuBuild == false
            Compat.RaCustomMenuCompat.Init();
#endif
            RueIActive = false;
            try
            {
                var type = LabApi.Loader.PluginLoader.Plugins.Values.Any(
                    a => a.DefinedTypes.Any(t => t.FullName == "RueI.API.RueDisplay")
                );
                Logger.Debug($"Loaded compatibility with RueI. ({type})");
                RueIActive = true;
            }
            catch (Exception)
            {
                // ignore
            }

            PlayerEvents.ChangingRole += CustomSpawnManager.OnPlayerChangingRole;

            CustomHandlersManager.RegisterEventsHandler(_handlers);
            InitializeComponents();
        }

        public override void Disable()
        {
            _harmony.UnpatchAll();
            PlayerEvents.ChangingRole -= CustomSpawnManager.OnPlayerChangingRole;
            CustomHandlersManager.UnregisterEventsHandler(_handlers);
        }

        private static void InitializeComponents()
        {
            RoleIntroComponent.Init();
        }
    }
}