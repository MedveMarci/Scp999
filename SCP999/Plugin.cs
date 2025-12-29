using System;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using RoleAPI;
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
    public override Version Version => new(1, 1, 3);
    public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);
    public static Scp999 Instance { get; private set; }
    private Scp999Role Role { get; set; }

    public override void Enable()
    {
        Instance = this;
        Startup.SetupAPI(Name);
        Startup.RegisterCustomRole(Role);
        CustomHandlersManager.RegisterEventsHandler(_eventHandler);
    }

    public override void LoadConfigs()
    {
        base.LoadConfigs();
        Role = Config.Scp999Role;
    }

    public override void Disable()
    {
        Instance = null;
        Startup.UnRegisterCustomRole(Role);
        CustomHandlersManager.UnregisterEventsHandler(_eventHandler);
    }
}