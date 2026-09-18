using System;
using System.IO;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Paths;
using LabApi.Loader.Features.Plugins;
using RoleAPI.API.Audio;
using Scp999.Features;

namespace Scp999;

public class Scp999 : Plugin<Config>
{
    private readonly EventHandlers _eventHandler = new();

    public override string Name => "Scp999";

    public override string Description =>
        "Adds SCP-999, the tickling monster, as a custom role with unique abilities and features.";

    public override string Author => "MedveMarci";

    public override Version Version => new(1, 3, 0);

    public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);

    public static Scp999 Singleton { get; private set; }

    private Scp999Role Role { get; set; }

    public override void Enable()
    {
        Singleton = this;
        SetupAudioOverrides();
        RoleAPI.RoleAPI.RegisterRole(Role);
        CustomHandlersManager.RegisterEventsHandler(_eventHandler);
    }

    /// Audio ships embedded in the DLL, but a file of the same name placed in the audio directory replaces it.
    private static void SetupAudioOverrides()
    {
        string directory = Path.Combine(PathManager.Configs.FullName, "Scp999", "Audio");
        try
        {
            Directory.CreateDirectory(directory);
        }
        catch (Exception)
        {
            // ignored - the directory is optional, only used for overriding the embedded audio
        }

        AbilityAudio.SetOverrideDirectory(directory);
    }

    public override void LoadConfigs()
    {
        base.LoadConfigs();
        Role = Config.Scp999Role;
    }

    public override void Disable()
    {
        Singleton = null;
        AbilityAudio.SetOverrideDirectory(null);
        CustomHandlersManager.UnregisterEventsHandler(_eventHandler);
        RoleAPI.RoleAPI.UnregisterRole(Role);
    }
}