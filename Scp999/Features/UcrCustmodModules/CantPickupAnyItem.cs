using System.Collections.Generic;
using LabApi.Events.Arguments.Interfaces;
using LabApi.Events.Arguments.PlayerEvents;
using UncomplicatedCustomRoles.API.Features.CustomModules;

namespace Scp999.Features.UcrCustmodModules;

public class CantPickupAnyItem : CustomModule
{
    public override List<string> TriggerOnEvents { get; } =
    [
        "SearchingPickup",
        "InteractingScp330"
    ];

    public override bool OnEvent(string name, IPlayerEvent ev)
    {
        switch (ev)
        {
            case PlayerSearchingPickupEventArgs searchingPickupEventArgs:
                searchingPickupEventArgs.IsAllowed = false;
                break;
            case PlayerInteractingScp330EventArgs interactingScp330EventArgs:
                interactingScp330EventArgs.IsAllowed = false;
                break;
        }

        return base.OnEvent(name, ev);
    }
}