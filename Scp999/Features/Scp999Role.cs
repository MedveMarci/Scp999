using System.Collections.Generic;
using PlayerRoles;
using RoleAPI.API.Abilities;
using RoleAPI.API.Roles;
using RoleAPI.API.Schematics;
using Scp999.Features.Abilities;
using Scp999.Features.UcrCustmodModules;
using SecretLabNAudio.Core;
using UncomplicatedCustomRoles.API.Enums;
using UncomplicatedCustomRoles.API.Features;
using UncomplicatedCustomRoles.API.Features.Behaviour;
using UncomplicatedCustomRoles.API.Features.CustomModules;
using UncomplicatedCustomRoles.Manager;
using UnityEngine;
using YamlDotNet.Serialization;

namespace Scp999.Features;

public class Scp999Role : UcrRoleBase
{
    [YamlIgnore] public override int Id { get; set; } = 999;
    [YamlIgnore] public override string Name { get; set; } = "<color=#960018>SCP-999</color>";
    [YamlIgnore] public override bool OverrideRoleName { get; set; } = true;
    [YamlIgnore] public override string Nickname { get; set; } = null;
    public override string CustomInfo { get; set; } = "<color=#960018>Other Alive</color>";
    public override string BadgeName { get; set; } = "";
    public override string BadgeColor { get; set; } = "";
    [YamlIgnore] public override string SpawnHint { get; set; } = "";
    [YamlIgnore] public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;
    [YamlIgnore] public override Team? Team { get; set; } = PlayerRoles.Team.OtherAlive;
    [YamlIgnore] public override RoleTypeId RoleAppearance { get; set; } = RoleTypeId.Tutorial;
    public override List<Team> IsFriendOf { get; set; } = [];

    public override HealthBehaviour Health { get; set; } = new()
    {
        Amount = 1500,
        Maximum = 1500
    };

    public override List<Effect> Effects { get; set; } =
    [
        new()
        {
            EffectType = "Fade",
            Duration = -1,
            Intensity = 255,
            Removable = false
        },
        new()
        {
            EffectType = "Slowness",
            Duration = -1,
            Intensity = 25,
            Removable = false
        },
        new()
        {
            EffectType = "SilentWalk",
            Duration = -1,
            Intensity = 255,
            Removable = false
        },
        new()
        {
            EffectType = "Ghostly",
            Duration = -1,
            Intensity = 255,
            Removable = false
        }
    ];

    [YamlIgnore] public override bool CanEscape { get; set; } = false;

    public override string SpawnBroadcast { get; set; } =
        "<color=#ffa500>\ud83d\ude04 You are SCP-999 - The tickle monster! \ud83d\ude04\n" +
        "Heal Humans, dance and calm down SCPs in facility\n" +
        "Use abilities by clicking on the buttons</color>";

    public override ushort SpawnBroadcastDuration { get; set; } = 10;
    [YamlIgnore] public override List<ItemType> Inventory { get; set; } = [];
    [YamlIgnore] public override Dictionary<ItemType, ushort> Ammo { get; set; } = [];
    [YamlIgnore] public override float DamageMultiplier { get; set; } = 0;

    public override SpawnBehaviour SpawnSettings { get; set; } = new()
    {
        CanReplaceRoles =
        [
            RoleTypeId.FacilityGuard, RoleTypeId.Scientist, RoleTypeId.ClassD
        ],
        MinPlayers = 5,
        MaxPlayers = 1,
        SpawnChance = 5,
        Spawn = SpawnType.RoleSpawn,
        SpawnRoles = [RoleTypeId.Scp096]
    };

    public override SpeakerSettings? DefaultSpeakerSettings { get; } = new SpeakerSettings
    {
        Volume = 0.5f,
        IsSpatial = true,
        MinDistance = 5f,
        MaxDistance = 15f
    };

    public override RoleSchematic Schematic { get; } = new()
    {
        Name = "Scp999",
        PositionOffset = new Vector3(0f, -0.75f, 0f),
        HideCarrierModel = true
    };

    /// Abilities available to this role
    public override IReadOnlyList<AbilityBase> Abilities { get; } = new List<AbilityBase>
    {
        new YippeeAbility(),
        new HelloAbility(),
        new HealAbility(),
        new AnimationAbility()
    };

    public override void OnSpawned(SummonedCustomRole role)
    {
        role.AddModule(
            typeof(CustomScpAnnouncer),
            new Dictionary<string, object> { { "name", "SCP-999" } }
        );
        role.AddModule(typeof(ColorfulNickname), new Dictionary<string, object> { { "color", "#960018" } });
        role.AddModule(typeof(ColorfulRaName), new Dictionary<string, object> { { "color", "#960018" } });
        role.AddModule(typeof(NoBloodDecal));
        role.AddModule(typeof(CantPickupAnyItem));
        role.AddModule(typeof(DisableAnyInteraction));
        role.AddModule(typeof(RunAway));
        base.OnSpawned(role);
    }
}