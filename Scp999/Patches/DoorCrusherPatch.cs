using HarmonyLib;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using PlayerRoles;
using PlayerStatsSystem;
using UncomplicatedCustomRoles.API.Features;
using UncomplicatedCustomRoles.Extensions;
using UnityEngine;

namespace Scp999.Patches;

[HarmonyPatch(typeof(DoorCrusherExtension), nameof(DoorCrusherExtension.OnTriggerEnter))]
public class DoorCrusherPatch
{
    [HarmonyPrefix]
    public bool OnTriggerEnter(DoorCrusherExtension __instance, Collider other)
    {
        ReferenceHub hub;
        if (!NetworkServer.active || !ReferenceHub.TryGetHub(other.transform.root.gameObject, out hub))
            return false;
        PlayerRoleBase currentRole = hub.roleManager.CurrentRole;

        if ((hub.TryGetSummonedInstance(out SummonedCustomRole role) && role.Role.Id == 999) || currentRole.RoleTypeId == RoleTypeId.Scp106)
            return false;

        bool flag = hub.GetTeam() == Team.SCPs;
        if (__instance.IgnoreScps & flag)
            return false;
        float damage = flag ? __instance.ScpCrushDamage : -1f;
        hub.playerStats.DealDamage(new UniversalDamageHandler(damage, DeathTranslations.Crushed));
        return false;
    }
}