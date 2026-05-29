using System.Collections.Generic;
using LabApi.Events.Arguments.Interfaces;
using LabApi.Events.Arguments.PlayerEvents;
using UncomplicatedCustomRoles.API.Features.CustomModules;

namespace Scp999.Features.UcrCustmodModules;

public class NoBloodDecal : CustomModule
{
    public override List<string> TriggerOnEvents { get; } = ["PlacingBlood"];

    public override bool OnEvent(string name, IPlayerEvent ev)
    {
        if (ev is PlayerPlacingBloodEventArgs placingBloodEventArgs)
            placingBloodEventArgs.IsAllowed = false;
        return base.OnEvent(name, ev);
    }
}