using System.Collections.Generic;
using CustomPlayerEffects;
using LabApi.Events.Arguments.Interfaces;
using LabApi.Events.Arguments.PlayerEvents;
using UncomplicatedCustomRoles.API.Features.CustomModules;

namespace Scp999.Features.UcrCustmodModules;

public class RunAway : CustomModule
{
    public override List<string> TriggerOnEvents { get; } = ["Hurt"];

    public override bool OnEvent(string name, IPlayerEvent ev)
    {
        if (!(ev is PlayerHurtEventArgs playerHurtEventArgs) || playerHurtEventArgs.Player.HasEffect<MovementBoost>())
            return base.OnEvent(name, ev);
        if (playerHurtEventArgs.Player.TryGetEffect<Slowness>(out var slowness))
            playerHurtEventArgs.Player.EnableEffect<MovementBoost>((byte)(slowness.Intensity + 50), 10f);
        else
            playerHurtEventArgs.Player.EnableEffect<MovementBoost>(50, 10f);
        return base.OnEvent(name, ev);
    }
}