using System.Collections.Generic;
using LabApi.Events.Arguments.Interfaces;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.WarheadEvents;
using UncomplicatedCustomRoles.API.Features.CustomModules;

namespace Scp999.Features.UcrCustmodModules;

public class DisableAnyInteraction : CustomModule
{
    public override List<string> TriggerOnEvents { get; } =
    [
        "Starting",
        "InteractingDoor",
        "InteractingGenerator",
        "InteractingLocker",
        "InteractingScp330",
        "InteractingWarheadLever"
    ];

    public override bool OnEvent(string name, IPlayerEvent ev)
    {
        switch (ev)
        {
            case PlayerInteractingDoorEventArgs interactingDoorEventArgs:
                interactingDoorEventArgs.IsAllowed = false;
                break;
            case PlayerInteractingGeneratorEventArgs generatorEventArgs:
                generatorEventArgs.IsAllowed = false;
                break;
            case PlayerInteractingLockerEventArgs interactingLockerEventArgs:
                interactingLockerEventArgs.IsAllowed = false;
                break;
            case PlayerInteractingScp330EventArgs interactingScp330EventArgs:
                interactingScp330EventArgs.IsAllowed = false;
                break;
            case PlayerInteractingWarheadLeverEventArgs warheadLeverEventArgs:
                warheadLeverEventArgs.IsAllowed = false;
                break;
            case WarheadStartingEventArgs startingEventArgs:
                startingEventArgs.IsAllowed = false;
                break;
        }

        return base.OnEvent(name, ev);
    }
}