using CustomRoleLib.API;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.Arguments.WarheadEvents;
using LabApi.Events.CustomHandlers;
using Respawning.Waves;

namespace CustomRoleLib;

public class EventHandlers : CustomEventsHandler
{
    public override void OnServerWaveRespawned(WaveRespawnedEventArgs ev)
    {
        CustomSpawnManager.ResetAllSpentTokens(CustomSpawnManager.TokenResetType.SpawnWaveAny);
        CustomSpawnManager.ResetAllSpentTokens(
            ev.Wave.Base is NtfMiniWave or ChaosMiniWave
            ? CustomSpawnManager.TokenResetType.SpawnWaveSmall
            : CustomSpawnManager.TokenResetType.SpawnWaveLarge
            );
    }

    public override void OnServerWaitingForPlayers()
    {
        CustomSpawnManager.ResetAllSpentTokens(CustomSpawnManager.TokenResetType.RoundRestart);
    }

    public override void OnWarheadDetonated(WarheadDetonatedEventArgs ev)
    {
        CustomSpawnManager.ResetAllSpentTokens(CustomSpawnManager.TokenResetType.NukeDetonation);
    }

    public override void OnServerLczDecontaminationStarted()
    {
        CustomSpawnManager.ResetAllSpentTokens(CustomSpawnManager.TokenResetType.Decontamination);
    }
}