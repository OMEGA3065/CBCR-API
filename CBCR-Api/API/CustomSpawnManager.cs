using LabApi.Events.Arguments.PlayerEvents;
using PlayerRoles;
using Random = UnityEngine.Random;

namespace CustomRoleLib.API;

public static class CustomSpawnManager
{
    /// <summary>
    /// Used when the reason for the role change is a modification caused by the CR system.
    /// </summary>
    public const RoleChangeReason CustomRoleChange = (RoleChangeReason)76;

    /// <summary>
    /// Stores the information about what roles can spawn
    /// </summary>
    public static readonly Dictionary<string, IRoleGroup> WeightedRoleLookup = [];

    public static readonly RoleChangeReason[] RoleSpawnChangeReasons = [
        RoleChangeReason.Escaped, RoleChangeReason.LateJoin,
        RoleChangeReason.Respawn, RoleChangeReason.RespawnMiniwave,
        RoleChangeReason.Resurrected, RoleChangeReason.RoundStart
    ];

    /// <summary>
    /// Sets how many players can spawn in a group in total. (per round / spawn wave)
    /// </summary>
    /// <param name="groupName">The group to apply this setting to</param>
    /// <param name="tokenCount">Set to less than 1 in order to disable spawn caps.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the target group doesn't use the RoleGroup handler.
    /// This happens when it uses a custom derived type.
    /// </exception>
    public static void SetGroupMaxTokens(string groupName, int tokenCount)
    {
        if (!WeightedRoleLookup.TryGetValue(groupName, out var group))
        {
            group = new RoleGroup();
            WeightedRoleLookup[groupName] = group;
        }

        if (group is not RoleGroup roleGroup)
            throw new ArgumentException($"{groupName} is not a valid RoleGroup!");

        roleGroup.MaxPlayerSpawns = tokenCount;
    }

    /// <summary>
    /// When the used tokens reset for a specific group (each round/spawnWave/other).
    /// </summary>
    /// <remarks>
    /// TokenResetType is an enum of Flags which can be <code>|</code> together.
    /// </remarks>
    /// <param name="groupName">The group to apply this setting to</param>
    /// <param name="type">The type </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the target group doesn't use the RoleGroup handler.
    /// This happens when it uses a custom derived type.
    /// </exception>
    public static void SetGroupTokenReset(string groupName, TokenResetType type)
    {
        if (!WeightedRoleLookup.TryGetValue(groupName, out var group))
        {
            group = new RoleGroup();
            WeightedRoleLookup[groupName] = group;
        }

        if (group is not RoleGroup roleGroup)
            throw new ArgumentException($"{groupName} is not a valid RoleGroup!");

        roleGroup.ResetType = type;
    }

    public static void ResetAllSpentTokens(TokenResetType type)
    {
        foreach (var kvp in WeightedRoleLookup)
        {
            if (kvp.Value is not RoleGroup roleGroup
                || roleGroup.ResetType != type)
            {
                continue;
            }

            roleGroup.ResetSpentTokens();
        }
    }

    internal static void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
    {
        if (ev.ChangeReason == CustomRoleChange) return;
        if (!RoleSpawnChangeReasons.Contains(ev.ChangeReason)) return;

        var originalNewRole = ev.NewRole;

        var roles = CustomRoleManager.Roles.Values
            .Where(r =>
                r.NaturallySpawnable
                && r.RoleSpawnOriginalRoleIds.Contains(originalNewRole)
                && !r.Check(ev.Player)
                && (!WeightedRoleLookup.TryGetValue(r.RoleSpawnGroup, out var group) || group.CanSpawnAnother())
            ).ToList();

        var totalWeight = roles.Sum(g => g.RoleSpawnWeight + g.RoleNotSpawnWeight);
        var cutoff = Random.Range(0, totalWeight);

        var currentSum = 0f;
        foreach (var role in roles)
        {
            currentSum += role.RoleSpawnWeight;
            if (currentSum < cutoff) continue;

            if (!role.TryGiveRole(ev.Player, true))
                continue;

            if (role.Type.HasValue)
            {
                ev.ChangeReason = CustomRoleChange;
                ev.NewRole = role.Type.Value;
            }

            if (WeightedRoleLookup.TryGetValue(role.RoleSpawnGroup, out var group))
                group.AddSpawned();

            break;
        }
    }

    /// <summary>
    /// Can be used to derive custom role spawn groups.
    /// Can also define spawn conditions for said groups.
    /// </summary>
    public interface IRoleGroup
    {
        public bool CanSpawnAnother();
        public void AddSpawned();
    }

    public class RoleGroup : IRoleGroup
    {
        // public readonly List<ICustomRole<object>> Roles = [];
        public int MaxPlayerSpawns = -1;
        private int _playerSpawns = 0;
        public TokenResetType ResetType = TokenResetType.RoundRestart;

        public bool CanSpawnAnother()
        {
            if (MaxPlayerSpawns < 0) return true;
            LabApi.Features.Console.Logger.Info($"[ROLE GROUP] MAX={MaxPlayerSpawns} CURRENT={_playerSpawns}");
            return _playerSpawns < MaxPlayerSpawns;
        }

        public void AddSpawned() => _playerSpawns++;
        public void ResetSpentTokens() => _playerSpawns = 0;
    }

    [Flags]
    public enum TokenResetType
    {
        Never = 0,
        RoundRestart = 1 << 0,
        SpawnWaveSmall = 1 << 1,
        SpawnWaveLarge = 1 << 2,
        SpawnWaveAny = SpawnWaveSmall | SpawnWaveLarge,
        NukeDetonation = 1 << 3,
        Decontamination = 1 << 4
    }
}