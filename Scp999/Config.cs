using System.ComponentModel;
using System.IO;
using LabApi.Loader.Features.Paths;
using Scp999.Features;

namespace Scp999;

public class Config
{
    [Description("Enable debug messages in the console.")]
    public bool Debug { get; set; } = false;

    [Description("Distance in which the heal ability can heal players.")]
    public float Distance { get; set; } = 3f;

    [Description("The amount of healing for the Heal Ability.")]
    public float HealAmount { get; set; } = 100f;

    [Description("Where the audio directory is located. The built-in sounds are saved here on startup. Replace a file with your own .ogg while keeping its name to customize it.")]
    public string AudioDirectoryPath { get; set; } = Path.Combine(PathManager.Configs.FullName, "Scp999", "Audio");

    [Description("The config for the role")]
    public Scp999Role Scp999Role { get; set; } = new();
}