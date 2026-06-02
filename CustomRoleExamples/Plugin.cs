using CustomRoleLib;
using CustomRoleLib.API;
using HarmonyLib;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;

namespace CustomRoleExamples
{
    public class CustomItemLibPlugin : Plugin<Config>
    {
        public static CustomItemLibPlugin Instance { get; private set; }
        public override string Name => "CBCI API Examples";
        public override string Description => "Example projects for the CBCI API.";

        public override string Author => "OMEGA3065";

        public List<ICustomRole<object>> items;

        public override Version Version => new(0, 1, 6);
        public override LoadPriority Priority => LoadPriority.Highest;
        public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);
        
        public CustomItemLibPlugin()
        {
            Instance = this;
        }

        private Harmony _harmony;

        public override void Enable()
        {
            _harmony = new Harmony("omega3065.cbciapi");
            _harmony.PatchAll();
            
            // You can register items individually
            // items = [
            //     new TestItem(),
            //     new TestCard(),
            //     new TestWeapon(),
            //     new InstantGrenade()
            // ];
            // items.ForEach(item => CustomItemManager.RegisterItem(item));

            CustomRoleManager.RegisterAllRoles();
        }

        public override void Disable()
        {
            CustomRoleManager.UnregisterAllRoles();
            _harmony.UnpatchAll();
        }
    }
}