using CustomAbilityLib.API;
using CustomPlayerEffects;
using CustomRoleLib.API;
using LabApi.Features.Wrappers;

namespace CustomRoleExamples.Example;

public class SpeedBoostAbility : ServerSpecificSettingAbility<SpeedBoostAbilityInstance>
{
    public override string Name => "Speed Boost";

    public override string Description => "Gives a minor temporary speed boost.";

    public override string Id => "speed_boost";

    protected override double Cooldown => 30;

    protected override int MaxUses => 2;
}

public class SpeedBoostAbilityInstance : AbilityInstanceBase
{
    public override void Create(Player player)
    {
        player.SendBroadcast($"USE .test to test the {Namespace}", 10);
    }

    public override void Destroy()
    {

    }

    public override bool Execute(out string response)
    {
        response = "Generic Error.";
        if (Owner == null) return false;
        Owner.EnableEffect<MovementBoost>(30, 10);
        return true;
    }
}