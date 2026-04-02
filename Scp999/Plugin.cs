using System;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using Scp999.Features;

namespace Scp999;

public class Scp999 : Plugin<Config>
{
    private readonly EventHandlers _eventHandler = new();
    public string githubRepo = "MedveMarci/Scp999";
    public override string Name => "Scp999";

    public override string Description =>
        "Adds SCP-999, the tickling monster, as a custom role with unique abilities and features.";

    public override string Author => "MedveMarci";
    public override Version Version => new(1, 2, 0);
    public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);
    public static Scp999 Singleton { get; private set; }
    private Scp999Role Role { get; set; }

    public override void Enable()
    {
        Singleton = this;
        RoleAPI.RoleAPI.RegisterRole(Role);
        CustomHandlersManager.RegisterEventsHandler(_eventHandler);
        AudioSetup.EnsureAudioFiles();
    }

    public override void LoadConfigs()
    {
        base.LoadConfigs();
        Role = Config.Scp999Role;
    }

    public override void Disable()
    {
        Singleton = null;
        CustomHandlersManager.UnregisterEventsHandler(_eventHandler);
        RoleAPI.RoleAPI.UnregisterRole(Role);
    }
}